using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Squeak;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public class SqueakSpeakInterpreterVisitor : SqueakSpeakBaseVisitor<object>
{
    // Add this class to handle returns
    private class ReturnValue
    {
        public object Value { get; }
        public ReturnValue(object value)
        {
            Value = value;
        }
    }

    // Holds variable values: variable name -> object
    private Dictionary<string, object> memory = new Dictionary<string, object>();

    // Track which files have been imported to avoid re-importing them
    private readonly HashSet<string> importedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    // The directory from which this file was loaded, so relative imports can work
    private string currentDirectory;

    // If you want to do async networking, you might store an HttpClient globally
    private static readonly HttpClient httpClient = new HttpClient();

    private readonly ILogger<SqueakSpeakInterpreterVisitor> logger;
    private readonly IDebuggerService _debugService;

    // ------------------------------------------------------
    // CONSTRUCTOR
    // ------------------------------------------------------
    public SqueakSpeakInterpreterVisitor(Dictionary<string, object> memory,
                                         string currentDirectory = "",
                                         ILogger<SqueakSpeakInterpreterVisitor> logger = null,
                                         IDebuggerService debugService = null)
    {
        // If no dictionary provided, make a new one
        this.memory = memory ?? new Dictionary<string, object>();
        this.currentDirectory = currentDirectory;
        this.logger = logger;
        _debugService = debugService;
    }


    public void ResetMemory(Dictionary<string, object> memory)
    {
        this.memory = memory;
    }

    public void SetCurrentPath(string path)
    {
        currentDirectory = path;
    }

    // -------------------------------------------------------
    // PROGRAM
    // program : (adorableStatement)* EOF
    // -------------------------------------------------------
    public override object VisitProgram(SqueakSpeakParser.ProgramContext context)
    {
        try
        {
            logger.LogInformation("=== Starting SqueakSpeak Program ===");

            // Visit each child statement in order
            foreach (var stmt in context.adorableStatement())
            {
                // Check breakpoints before each statement
                if (_debugService != null)
                {
                    var line = stmt.Start.Line;
                    var variables = memory.Select(kv => new DebugVariable 
                    { 
                        Name = kv.Key, 
                        Value = kv.Value,
                        Type = kv.Value?.GetType()
                    });
                    var stack = new StackTrace().GetFrames();
                    _debugService.CheckBreakpoint(line, variables, stack);
                }
                
                Visit(stmt);
            }

            logger.LogInformation("=== End of SqueakSpeak Program ===");
        }
        catch (Exception ex)
        {
            _debugService?.ReportException(ex);
            throw;
        }
        return null;
    }

    // ---------------------------------------------
    // 1) BeepBoop("https://...") -> varName;
    // ---------------------------------------------
    public override object VisitSqueakNetGet(SqueakSpeakParser.SqueakNetGetContext ctx)
    {
        // Evaluate the purrOperation inside the parentheses. We expect a string or something we can convert to a string
        object urlVal = Visit(ctx.purrOperation());

        // The '-> ID' part
        string varName = ctx.ID().GetText();

        // Convert urlVal to string
        string url = urlVal?.ToString() ?? "";

        // Perform the network request (synchronously or asynchronously).
        // For simplicity, do a blocking call to .Result
        try
        {
            logger.LogInformation("[Network] GET: {Url}", url);
            var result = httpClient.GetAsync(url).Result;

            // Store in the variable
            memory[varName] = new Dictionary<string, object>
            {
                ["html"] = result.Content.ReadAsStringAsync().Result,
                ["status"] = result.StatusCode
                // maybe more fields
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[Network] Error fetching {Url}: {Message}", url, ex.Message);
            // Possibly store null or an error message
            memory[varName] = null;
        }

        return null;
    }

    // ---------------------------------------------
    // 2) Listen(myVar);
    // ---------------------------------------------
    public override object VisitSqueakIn(SqueakSpeakParser.SqueakInContext context)
    {
        string varName = context.ID().GetText();
        logger.LogInformation("[Input] Please enter a value:");
        string input = Console.ReadLine() ?? "";
        memory[varName] = input;
        logger.LogDebug("[Input] Stored value '{Value}' in variable '{VarName}'", input, varName);
        return input;
    }

    // ---------------------------------------------
    // 3) Brain(sin, 45) -> resultVar;
    //    or Brain(pow, 2, 3) -> outVar;
    // ---------------------------------------------
    public override object VisitSqueakMathCall(SqueakSpeakParser.SqueakMathCallContext ctx)
    {
        // The function name, e.g. 'sin', 'cos', 'pow', 'sqrt'
        string funcName = ctx.mathFunc.Text.ToLowerInvariant();

        // We allow up to two purrOperation expressions
        // e.g. Brain(pow, 2, 3)
        var ops = ctx.purrOperation();
        object arg1 = null;
        object arg2 = null;

        if (ops.Length > 0)
            arg1 = Visit(ops[0]);
        if (ops.Length > 1)
            arg2 = Visit(ops[1]);

        double a = ToDouble(arg1);
        double b = ToDouble(arg2);

        double result = 0.0;
        switch (funcName)
        {
            case "sin":
                result = Math.Sin(a);
                break;
            case "cos":
                result = Math.Cos(a);
                break;
            case "tan":
                result = Math.Tan(a);
                break;
            case "sqrt":
                result = Math.Sqrt(a);
                break;
            case "pow":
                result = Math.Pow(a, b);
                break;
            // Add more as desired
            default:
                logger.LogWarning("[Math] Unknown function: {FunctionName}", funcName);
                break;
        }

        // -> ID? means we might or might not have a variable to store into
        if (ctx.ID().Length == 2)
        {
            string varName = ctx.ID(1).GetText();
            memory[varName] = result;
        }
        else
        {
            // If no var specified, maybe just print it or do nothing
            logger.LogInformation("[Math] {FunctionName} result = {Result}", funcName, result);
        }

        return null;
    }



    // -------------------------------------------------------
    // STATEMENTS
    // -------------------------------------------------------

    /**
     * squeakOut : 'Squeak' purrOperation ';'
     * E.g.: Squeak "Hello World!";
     */
    public override object VisitSqueakOut(SqueakSpeakParser.SqueakOutContext context)
    {
        // Evaluate the expression
        var value = Visit(context.purrOperation());
        Console.WriteLine(value?.ToString() ?? "null");
        return null;
    }

    /**
     * hugThis : 'Cuddle' ID ('=' purrOperation)? ';'
     * E.g.: Cuddle x = 42;
     */
    public override object VisitHugThis(SqueakSpeakParser.HugThisContext context)
    {
        string varName = context.ID().GetText();
        object val = null;
        
        if (context.purrOperation() != null)
        {
            val = Visit(context.purrOperation());
        }
        else if (context.invokeWhimsy() != null)
        {
            val = Visit(context.invokeWhimsy());
        }
        
        memory[varName] = val;
        logger.LogDebug("[Variable Assignment] {VarName} = {Value} (Type: {ValueType})", 
            varName, val, val?.GetType().Name);
        return val;
    }

    /**
     * purrMath : ID '=' purrOperation ';'
     * E.g.: x = 10 + y;
     */
    public override object VisitPurrMath(SqueakSpeakParser.PurrMathContext ctx)
    {
        // Evaluate the right-hand side (either expression or function call)
        object newValue = ctx.purrOperation() != null ? 
            Visit(ctx.purrOperation()) : 
            Visit(ctx.invokeWhimsy());

        // The left-hand side
        var leftNode = ctx.leftExpr();

        // 1) Get the base object (the first ID)
        string baseVarName = leftNode.ID(0).GetText();
        object targetObject = memory.ContainsKey(baseVarName) ? memory[baseVarName] : null;

        // 2) "Arrow count" is how many ->field references we have after the base
        int arrowCount = leftNode.ID().Length - 1; // total IDs minus the first

        // 3) Traverse *all but the last* arrow to keep targetObject = parent dictionary
        //    The last arrow is our final field name to assign into.
        for (int i = 0; i < arrowCount - 1; i++)
        {
            string fieldName = leftNode.ID(i + 1).GetText();

            // If the target object is a dictionary, try to get sub-dict
            if (targetObject is Dictionary<string, object> dict)
            {
                if (!dict.TryGetValue(fieldName, out var subVal) || subVal == null)
                {
                    // Maybe create a new sub-dict if it doesn't exist
                    var newSub = new Dictionary<string, object>();
                    dict[fieldName] = newSub;
                    subVal = newSub;
                }
                targetObject = subVal;
            }
            else
            {
                logger.LogError("[Error] Not a dictionary. Nested field assignment fails here.");
                return null;
            }
        }

        // 4) The final arrow's field name (if any). If arrowCount == 0, that means no ->
        //    so the "field" is really just the top-level variable name itself.
        if (arrowCount == 0)
        {
            // No arrows: e.g. "x = 5;"
            // Just assign directly in memory
            memory[baseVarName] = newValue;
            return null;
        }

        // If arrowCount > 0, then we do have a final "->someField"
        string finalFieldName = leftNode.ID(arrowCount).GetText();

        // 5) Also handle bracket indexing if you allow arrays in leftExpr
        for (int i = 0; i < leftNode.purrOperation().Length; i++)
        {
            object indexVal = Visit(leftNode.purrOperation(i));
            if (targetObject is List<object> list)
            {
                int idx = ToInt(indexVal);
                if (idx < 0 || idx >= list.Count)
                {
                    logger.LogError("[Error] Array index out of bounds.");
                    return null;
                }
                // Move into that sub-element
                targetObject = list[idx];
            }
            else
            {
                logger.LogError("[Error] Attempting to index a non-list object.");
                return null;
            }
        }

        // Now targetObject is the dictionary (or object) that we want to set "finalFieldName" on.
        if (targetObject is Dictionary<string, object> finalDict)
        {
            finalDict[finalFieldName] = newValue;
        }
        else
        {
            // Possibly top-level variable is not a dict but we used -> anyway
            logger.LogError("[Error] Attempting to assign a field in a non-dictionary object.");
        }

        return null;
    }

    /**
     * snuggleIf:
     *    'Peek' '(' condition ')' '{' ifBlock* '}'
     *      ( 'Purr' '{' elseBlock* '}' )?
     *
     * With a visitor, we can simply:
     * 1) Evaluate the condition
     * 2) If true => visit ifBlock
     * 3) Else => visit elseBlock (if present)
     */
    public override object VisitSnuggleIf(SqueakSpeakParser.SnuggleIfContext context)
    {
        bool condValue = EvaluateCondition(context.condition());

        if (condValue)
        {
            // Visit all statements in the ifBlock
            foreach (var stmt in context._ifBlock)
            {
                Visit(stmt);
            }
        }
        else
        {
            // If there's a 'Purr' block, visit all statements in there
            if (context._elseBlock != null && context._elseBlock.Count > 0)
            {
                foreach (var stmt in context._elseBlock)
                {
                    Visit(stmt);
                }
            }
        }
        return null;
    }

    /**
     * snugLoop:
     *   'Nuzzle' '(' condition ')' '{' adorableStatement* '}'
     * 
     * With a visitor, we can do:
     * while( condition == true ) {
     *    for each statement => visit
     * }
     */
    public override object VisitSnugLoop(SqueakSpeakParser.SnugLoopContext context)
    {
        while (EvaluateCondition(context.condition()))
        {
            // Visit each statement
            foreach (var stmt in context.adorableStatement())
            {
                Visit(stmt);
            }
        }
        return null;
    }

    /**
     * fluffMagic:
     *  'FluffMagic' ID '(' paramList? ')' '{' adorableStatement* '}'
     *
     * A real interpreter might store the function's AST in a dictionary
     * (like memory) for later calls. For now, we'll just print a message.
     */
    public override object VisitFluffMagic(SqueakSpeakParser.FluffMagicContext context)
    {
        string funcName = context.ID().GetText();

        // Save the function body (context) in memory for later invocation
        if (memory.ContainsKey(funcName))
        {
            logger.LogWarning("[Warning] Function '{FunctionName}' is being redefined!", funcName);
        }

        memory[funcName] = context;
        logger.LogInformation("[Function definition] -> {FunctionName}", funcName);

        return null;
    }

    /**
     * snipChoose / snipCase / snipDefault
     * If you want to implement switch-like logic, you can do it similarly:
     *  1) Evaluate the variable
     *  2) Compare to each case
     *  3) Visit matching case statements (and maybe break, etc.)
     * For brevity, not fully implemented here, but the same approach applies.
     */

    // ------------------------------------------------------
    // BRINGWARMTH: Load & parse another file
    // bringWarmth : 'BringWarmth' STRING ';'
    // ------------------------------------------------------
    public override object VisitBringWarmth(SqueakSpeakParser.BringWarmthContext context)
    {
        // Extract the file path from the string literal
        string rawPath = context.STRING().GetText(); // e.g. "\"SomeFile.squeakspeak\""
        // Strip off the quotes
        string importPath = rawPath.Substring(1, rawPath.Length - 2);

        // If it's a relative path, combine with the current directory
        string fullPath = importPath;
        if (!Path.IsPathRooted(fullPath) && !string.IsNullOrEmpty(currentDirectory))
        {
            fullPath = Path.Combine(currentDirectory, importPath);
        }

        // Normalize the path
        fullPath = Path.GetFullPath(fullPath);

        // Check if we've already imported this file
        if (importedFiles.Contains(fullPath))
        {
            logger.LogInformation("[BringWarmth] Already imported '{ImportPath}'. Skipping...", importPath);
            return null;
        }

        logger.LogInformation("[BringWarmth] Importing file: {FullPath}", fullPath);
        importedFiles.Add(fullPath);

        // Parse and visit the imported file
        try
        {
            string code = File.ReadAllText(fullPath);

            AntlrInputStream input = new AntlrInputStream(code);
            var lexer = new SqueakSpeakLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new SqueakSpeakParser(tokens);

            // Get the top-level parse tree
            var fileTree = parser.program();

            // Create another instance of this visitor
            // but share the same memory + pass the directory of the import
            string importDir = Path.GetDirectoryName(fullPath) ?? "";
            var importVisitor = new SqueakSpeakInterpreterVisitor(memory, importDir, logger);

            // Merge our "importedFiles" so that the new visitor also knows what we've already seen
            importVisitor.importedFiles.UnionWith(this.importedFiles);

            // Now visit
            importVisitor.Visit(fileTree);

            // After returning, re-sync the sets in case the imported file imported more
            this.importedFiles.UnionWith(importVisitor.importedFiles);
        }
        catch (Exception e)
        {
            logger.LogError("[BringWarmth] ERROR loading file: {ErrorMessage}", e.Message);
        }

        return null;
    }

    // ---------------------------------------------
    // purrOperation
    // ---------------------------------------------
    public override object VisitPurrOperation(SqueakSpeakParser.PurrOperationContext context)
    {
        logger.LogDebug("===== Starting VisitPurrOperation =====");
        logger.LogDebug("Operation text: {Text}", context.GetText());

        // Handle function call case
        if (context.invokeWhimsy() != null)
        {
            return Visit(context.invokeWhimsy());
        }

        // Handle single term case
        if (context.ChildCount == 1 && context.fieldAccess() != null && context.fieldAccess().Length > 0)
        {
            var result = Visit(context.fieldAccess(0));
            logger.LogDebug("Single term result: {Result} (Type: {ResultType})", result, result?.GetType().Name);
            return result;
        }

        // Process terms and operators
        if (context.fieldAccess() != null && context.fieldAccess().Length > 0)
        {
            object value = Visit(context.fieldAccess(0));
            for (int i = 1; i < context.fieldAccess().Length; i++)
            {
                string op = context.purrOperator(i-1).GetText();
                object right = Visit(context.fieldAccess(i));
                value = ApplyOperator(value, op, right);
            }
            return value;
        }

        return null;
    }
    public override object VisitPurrTerm(SqueakSpeakParser.PurrTermContext context)
    {
        logger.LogDebug("===== Starting VisitPurrTerm =====");
        logger.LogDebug("Term text: {Text}", context.GetText());

        object value = null;
        
        // Handle parenthesized expressions
        if (context.baseTerm()?.purrOperation() != null)
        {
            value = Visit(context.baseTerm().purrOperation());
            logger.LogDebug("Parenthesized expression result: {Value}", value);
        }
        // Handle variables
        else if (context.baseTerm()?.ID() != null)
        {
            string varName = context.baseTerm().ID().GetText();
            value = memory.ContainsKey(varName) ? memory[varName] : varName;
            logger.LogDebug("Variable resolved: {Value}", value);
        }
        // Handle strings
        else if (context.baseTerm()?.STRING() != null)
        {
            string str = context.baseTerm().STRING().GetText();
            value = str.Substring(1, str.Length - 2); // Remove quotes
        }
        // Handle numbers
        else if (double.TryParse(context.baseTerm()?.GetText(), NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
        {
            value = num;
        }
        // Handle array literals
        else if (context.baseTerm()?.arrayLiteral() != null)
        {
            value = Visit(context.baseTerm().arrayLiteral());
        }

        // Handle array indexing if present
        foreach (var indexExpr in context.purrOperation())
        {
            if (value is List<object> list)
            {
                int index = Convert.ToInt32(Visit(indexExpr));
                if (index >= 0 && index < list.Count)
                {
                    value = list[index];
                }
            }
        }

        logger.LogDebug("Term final value: {Value}", value);
        return value;
    }

    private object ApplyOperator(object left, string op, object right)
    {
        logger.LogDebug("===== Starting ApplyOperator =====");
        logger.LogDebug("Operator: {Operator}", op);
        logger.LogDebug("Left initial: {Left} (Type: {LeftType})", left, left?.GetType().Name);
        logger.LogDebug("Right initial: {Right} (Type: {RightType})", right, right?.GetType().Name);

        // First resolve any variables
        if (left is string leftStr && memory.ContainsKey(leftStr))
        {
            logger.LogDebug("Resolving left variable '{LeftVariable}'", leftStr);
            left = memory[leftStr];
            logger.LogDebug("Left resolved to: {Left} (Type: {LeftType})", left, left?.GetType().Name);
        }
        if (right is string rightStr && memory.ContainsKey(rightStr))
        {
            logger.LogDebug("Resolving right variable '{RightVariable}'", rightStr);
            right = memory[rightStr];
            logger.LogDebug("Right resolved to: {Right} (Type: {RightType})", right, right?.GetType().Name);
        }

        // Handle field access operator
        if (op == "->")
        {
            logger.LogDebug("Processing field access operator");
            if (left is Dictionary<string, object> dict && right is string field)
            {
                logger.LogDebug("Accessing field '{Field}' from dictionary");
                if (dict.TryGetValue(field, out object value))
                {
                    logger.LogDebug("Field value found: {Value}", value);
                    return value;
                }
                logger.LogDebug("Field not found in dictionary");
                return null;
            }
        }

        // Regular operators
        if (TryGetNumber(left, out double leftNum) && TryGetNumber(right, out double rightNum))
        {
            logger.LogDebug("Processing numeric operation");
            var result = op switch
            {
                "+" => leftNum + rightNum,
                "-" => leftNum - rightNum,
                "*" => leftNum * rightNum,
                "/" => rightNum != 0 ? leftNum / rightNum : 0,
                _ => 0
            };
            logger.LogDebug("Numeric result: {Result}", result);
            return result;
        }

        // Default to string concatenation for +
        if (op == "+")
        {
            logger.LogDebug("Processing string concatenation");
            var result = ConvertToString(left) + ConvertToString(right);
            logger.LogDebug("Concatenation result: {Result}", result);
            return result;
        }

        logger.LogDebug("No applicable operation found");
        logger.LogDebug("===== End ApplyOperator =====\n");
        return null;
    }

    private bool TryGetNumber(object value, out double result)
    {
        result = 0;
        if (value is double d) { result = d; return true; }
        if (value is int i) { result = i; return true; }
        return double.TryParse(value?.ToString(), out result);
    }

    private string ConvertToString(object obj)
    {
        if (obj == null) return "null";
        
        // If obj is a string that represents a variable name, resolve it
        if (obj is string strValue && memory.ContainsKey(strValue))
        {
            obj = memory[strValue];
        }
        
        if (obj is Dictionary<string, object> dict)
        {
            // If we're accessing a specific field (indicated by ->), return that field's value
            if (dict.TryGetValue("__accessing_field", out object fieldName) && 
                fieldName is string field &&
                dict.TryGetValue(field, out object fieldValue))
            {
                dict.Remove("__accessing_field"); // Clean up after use
                return ConvertToString(fieldValue);
            }
            
            // Default dictionary representation
            return "{" + string.Join(", ", dict.Select(kv => $"{kv.Key}: {ConvertToString(kv.Value)}")) + "}";
        }
        else if (obj is List<object> list)
        {
            return "[" + string.Join(", ", list.Select(item => ConvertToString(item))) + "]";
        }
        
        return obj.ToString();
    }

    public override object VisitObjectCreation(SqueakSpeakParser.ObjectCreationContext context)
    {
        string objectName = context.ID().GetText();

        var newObject = new Dictionary<string, object>();

        foreach (var fieldCtx in context.fieldAssignment())
        {
            string fieldName = fieldCtx.ID(1).GetText();
            object fieldValue = Visit(fieldCtx.purrOperation());
            newObject[fieldName] = fieldValue;
        }

        memory[objectName] = newObject;

        return null;
    }


    /**
     * invokeWhimsy: ID '(' paramList? ')' ';'
     * E.g.: doSomething(10, 20);
     *
     * If we had stored function definitions, we would retrieve them and pass parameters.
     */
    public override object VisitInvokeWhimsy(SqueakSpeakParser.InvokeWhimsyContext context)
    {
        string funcName = context.ID().GetText();

        // Check if the function exists
        if (!memory.ContainsKey(funcName))
        {
            logger.LogWarning("[Warning] Function '{FunctionName}' is not defined!", funcName);
            return null;
        }

        // Get parameter values if any
        List<object> paramValues = new List<object>();
        if (context.paramList() != null)
        {
            foreach (var param in context.paramList().param())
            {
                // Evaluate the parameter expression
                object paramValue = Visit(param);
                
                // If the parameter is a variable name, get its value from memory
                if (paramValue is string paramName && memory.ContainsKey(paramName))
                {
                    paramValue = memory[paramName];
                }
                
                paramValues.Add(paramValue);
                logger.LogDebug("[Function Parameter] {Value} (Type: {Type})", 
                    paramValue, paramValue?.GetType().Name);
            }
        }

        // Retrieve the function definition
        if (memory[funcName] is SqueakSpeakParser.FluffMagicContext funcContext)
        {
            logger.LogInformation("[Function call] -> {FunctionName}", funcName);

            // Create a new scope for the function
            var previousMemory = new Dictionary<string, object>(memory);

            // If the function has parameters, bind them to the parameter values
            var paramNodes = funcContext.paramList()?.param();
            if (paramNodes != null)
            {
                for (int i = 0; i < paramNodes.Length && i < paramValues.Count; i++)
                {
                    string paramName = paramNodes[i].ID().GetText();
                    memory[paramName] = paramValues[i];
                    logger.LogDebug("[Function Binding] {ParamName} = {Value}", 
                        paramName, paramValues[i]);
                }
            }

            // Execute the function body
            object result = null;
            foreach (var stmt in funcContext.adorableStatement())
            {
                result = Visit(stmt);
                
                // Check if it's a return value
                if (IsReturnValue(result, out object returnValue))
                {
                    // Restore the previous memory state
                    memory = previousMemory;
                    return returnValue;  // Return the actual value, not wrapped in ReturnValue
                }
            }

            // Restore the previous memory state
            memory = previousMemory;
            return result;
        }
        else
        {
            logger.LogWarning("[Warning] '{FunctionName}' is not a callable function!", funcName);
        }

        return null;
    }


    // -------------------------------------------------------
    // EXPRESSIONS
    // -------------------------------------------------------

    public override object VisitArrayLiteral(SqueakSpeakParser.ArrayLiteralContext ctx)
    {
        var elements = new List<object>();
        for (int i = 0; i < ctx.purrOperation().Length; i++)
        {
            object val = Visit(ctx.purrOperation(i));
            elements.Add(val);
        }
        return elements;
    }
    public override object VisitNativeCall(SqueakSpeakParser.NativeCallContext ctx)
    {
        // 1) Extract the type name and method name from the STRING tokens
        string rawTypeName = StripQuotes(ctx.STRING(0).GetText()); // e.g. "System.Math"
        string rawMethodName = StripQuotes(ctx.STRING(1).GetText()); // e.g. "Abs"

        // 2) Evaluate paramList if present
        var argValues = new List<object>();
        if (ctx.paramList() != null)
        {
            foreach (var p in ctx.paramList().param())
            {
                object val = Visit(p);
                argValues.Add(val);
            }
        }

        // 3) Attempt to call the C# method via reflection
        object returnValue = CallDotNetMethod(rawTypeName, rawMethodName, argValues);

        // 4) If there's a '-> varName', store the result
        if (ctx.ID() != null)
        {
            string varName = ctx.ID().GetText();
            memory[varName] = returnValue;
        }

        return null;
    }

    private string StripQuotes(string s)
    {
        return s.Substring(1, s.Length - 2);
    }

    private object CallDotNetMethod(string typeName, string methodName, List<object> argValues)
    {
        try
        {
            // Step A: Acquire the Type
            Type type = ResolveType(typeName);
            if (type == null)
            {
                logger.LogWarning("[NativeCall Warning] Type '{TypeName}' not found.", typeName);
                return null;
            }

            // Step B: Gather candidate methods (same name, same paramCount)
            MethodInfo[] candidates = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                          .Where(m => m.Name == methodName
                                                   && m.GetParameters().Length == argValues.Count)
                                          .ToArray();
            if (candidates.Length == 0)
            {
                logger.LogWarning("[NativeCall Warning] No method named '{MethodName}' with {ArgCount} args in {TypeName}.", methodName, argValues.Count, typeName);
                return null;
            }

            // Step C: Try to find the "best" method
            MethodInfo bestMethod = null;
            int bestScore = -1;

            foreach (var method in candidates)
            {
                var paramInfos = method.GetParameters();
                int totalScore = 0;
                bool allCompatible = true;

                for (int i = 0; i < paramInfos.Length; i++)
                {
                    Type paramType = paramInfos[i].ParameterType;
                    object arg = argValues[i];

                    int score = GetCompatibilityScore(arg, paramType);
                    if (score < 0)
                    {
                        allCompatible = false;
                        break;
                    }
                    else
                    {
                        totalScore += score;
                    }
                }

                if (allCompatible)
                {
                    if (totalScore > bestScore)
                    {
                        bestScore = totalScore;
                        bestMethod = method;
                    }
                }
            }

            if (bestMethod == null)
            {
                logger.LogWarning("[NativeCall Warning] Method '{MethodName}' with {ArgCount} args found, but no compatible overload for given arguments.", methodName, argValues.Count);
                return null;
            }

            // Step D: Convert each argument to the exact parameter type, then invoke
            object[] finalArgs = new object[argValues.Count];
            var finalParamInfos = bestMethod.GetParameters();
            for (int i = 0; i < argValues.Count; i++)
            {
                finalArgs[i] = ConvertArgToType(argValues[i], finalParamInfos[i].ParameterType);
            }

            // If the method is non-static, create an instance (or get from somewhere)
            object instance = null;
            if (!bestMethod.IsStatic)
            {
                instance = Activator.CreateInstance(type);
            }

            // Finally, invoke
            object result = bestMethod.Invoke(instance, finalArgs);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[NativeCall Exception] {ErrorMessage}", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Returns -1 if <paramref name="arg"/> cannot possibly go into <paramref name="paramType"/>.
    /// Otherwise returns an integer "score" that indicates how good a match it is.
    /// Higher score => better match.
    /// </summary>
    private int GetCompatibilityScore(object arg, Type paramType)
    {
        if (arg == null)
        {
            if (!paramType.IsValueType || (Nullable.GetUnderlyingType(paramType) != null))
            {
                return 50; // null can match a reference type or nullable
            }
            return -1;
        }

        Type argType = arg.GetType();
        if (paramType.IsAssignableFrom(argType))
        {
            // Perfect match
            return 100;
        }

        // Numeric conversions
        if (paramType == typeof(double))
        {
            if (argType == typeof(double)) return 90;
            if (argType == typeof(int)) return 80;
            if (argType == typeof(float)) return 70;
        }
        else if (paramType == typeof(int))
        {
            if (argType == typeof(int)) return 90;
            if (argType == typeof(double)) return 70;
        }
        else if (paramType == typeof(string))
        {
            return 50; // We can always .ToString()
        }

        return -1;
    }

    public override object VisitBaseTerm(SqueakSpeakParser.BaseTermContext ctx)
    {
        if (ctx.NUMBER() != null)
        {
            string text = ctx.NUMBER().GetText();
            if (int.TryParse(text, out int val))
            {
                return val;
            }
            return null;
        }
        else if (ctx.STRING() != null)
        {
            string raw = ctx.STRING().GetText();
            return raw.Substring(1, raw.Length - 2);
        }
        else if (ctx.FLOAT() != null)
        {
            string text = ctx.FLOAT().GetText();
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
            {
                return val;
            }
            return null;
        }
        else if (ctx.arrayLiteral() != null)
        {
            return Visit(ctx.arrayLiteral());
        }
        else if (ctx.purrOperation() != null)
        {
            return Visit(ctx.purrOperation());
        }
        else if (ctx.ID() != null)
        {
            string varName = ctx.ID().GetText();
            if (memory.TryGetValue(varName, out object val))
            {
                return val;
            }
            logger.LogError("[Error] Variable not found: {VariableName}", varName);
            return null;
        }

        logger.LogError("[Error] Unknown baseTerm encountered.");
        return null;
    }

    private object ConvertArgToType(object arg, Type targetType)
    {
        if (arg == null)
        {
            if (!targetType.IsValueType || Nullable.GetUnderlyingType(targetType) != null)
            {
                return null;
            }
            throw new InvalidOperationException($"Cannot pass null to parameter of type '{targetType}'");
        }

        if (targetType.IsAssignableFrom(arg.GetType()))
        {
            return arg;
        }

        if (targetType == typeof(double))
        {
            if (arg is int i) return (double)i;
            if (arg is double d) return d;
            if (arg is float f) return (double)f;
            if (arg is long l) return (double)l;
        }
        else if (targetType == typeof(int))
        {
            if (arg is double d) return (int)d;
            if (arg is float f) return (int)f;
        }
        else if (targetType == typeof(string))
        {
            return arg.ToString();
        }

        throw new InvalidOperationException(
           $"Cannot convert argument '{arg}' of type '{arg.GetType()}' to '{targetType}'");
    }

    private Type ResolveType(string typeName)
    {
        // 1. Hard-coded fallback for common BCL types
        if (typeName == "System.Math") return typeof(System.Math);
        if (typeName == "System.Console") return typeof(System.Console);

        var asmType = typeof(Program).Assembly.GetType(typeName, throwOnError: false);
        if (asmType != null) return asmType;

        var direct = Type.GetType(typeName, throwOnError: false);
        return direct;
    }

    private bool EvaluateCondition(SqueakSpeakParser.ConditionContext context)
    {
        // Left side
        string leftId = context.ID(0).GetText();
        object leftVal = memory.ContainsKey(leftId) ? memory[leftId] : 0;

        // Right side
        object rightVal;
        if (context.ID().Length > 1)
        {
            string rightId = context.ID(1).GetText();
            rightVal = memory.ContainsKey(rightId) ? memory[rightId] : 0;
        }
        else if (context.FLOAT() != null)
        {
            double parsedF = 0;
            double.TryParse(context.FLOAT().GetText(), NumberStyles.Any, CultureInfo.InvariantCulture, out parsedF);
            rightVal = parsedF;
        }
        else
        {
            rightVal = int.Parse(context.NUMBER().GetText());
        }

        double l = ToDouble(leftVal);
        double r = ToDouble(rightVal);
        string op = context.GetChild(1).GetText(); // e.g. "==", "!=", "<", etc.

        return op switch
        {
            "==" => (Math.Abs(l - r) < Double.Epsilon),  // for floats it might be approximate
            "!=" => (Math.Abs(l - r) >= Double.Epsilon),
            "<" => (l < r),
            ">" => (l > r),
            "<=" => (l <= r),
            ">=" => (l >= r),
            _ => false
        };
    }

    private int ToInt(object val)
    {
        if (val is int i) return i;
        if (val is double d) return (int)d;
        if (val is string s && int.TryParse(s, out int parsed)) return parsed;
        return 0;
    }

    // Helper for numeric conversions
    private double ToDouble(object val)
    {
        if (val is double d) return d;
        if (val is int i) return i;
        if (double.TryParse(val?.ToString(), out double result)) return result;
        return 0.0;
    }
    public override object VisitFieldAccess(SqueakSpeakParser.FieldAccessContext context)
    {
        object value = Visit(context.purrTerm());
        
        foreach (var field in context.ID())
        {
            if (value is Dictionary<string, object> dict)
            {
                string fieldName = field.GetText();
                if (dict.TryGetValue(fieldName, out object fieldValue))
                {
                    value = fieldValue;
                    logger.LogDebug("Field '{FieldName}' value: {Value}", fieldName, value);
                }
            }
        }
        
        return value;
    }

    // Add this method to check if something is a return value
    private bool IsReturnValue(object obj, out object value)
    {
        if (obj is ReturnValue ret)
        {
            value = ret.Value;
            return true;
        }
        value = null;
        return false;
    }

    // Add the return statement visitor
    public override object VisitReturnStatement(SqueakSpeakParser.ReturnStatementContext context)
    {
        object returnValue = null;
        if (context.purrOperation() != null)
        {
            returnValue = Visit(context.purrOperation());
        }
        return new ReturnValue(returnValue);
    }

    public override object VisitParam(SqueakSpeakParser.ParamContext context)
    {
        logger.LogDebug("===== Starting VisitParam =====");
        
        if (context.purrOperation() != null)
        {
            var result = Visit(context.purrOperation());
            logger.LogDebug("[Parameter Evaluation] Expression result: {Result} (Type: {ResultType})", 
                result, result?.GetType().Name);
            return result;
        }
        else if (context.ID() != null)
        {
            string varName = context.ID().GetText();
            if (memory.TryGetValue(varName, out var value))
            {
                logger.LogDebug("[Parameter Evaluation] Variable '{VarName}' resolved to: {Value}", 
                    varName, value);
                return value;
            }
            logger.LogDebug("[Parameter Evaluation] Variable '{VarName}' not found in memory", varName);
            return varName;
        }
        
        logger.LogWarning("[Parameter Evaluation] No value found for parameter");
        return null;
    }
}