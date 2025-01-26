using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Squeak
{
    internal class SqueakSpeakAssembly : SqueakSpeakBaseVisitor<object>
    {
        private List<string> assemblyCode = new List<string>();
        private HashSet<string> declaredVariables = new HashSet<string>();

        // -------------------------------------------------------
        // HELPER: Convert a parse‐tree operand (variable or number)
        //         into a string operand for assembly (e.g. "[x]" or "5").
        // -------------------------------------------------------
        private string AsOperand(object raw)
        {
            // If raw is already a string that represents a register, just return it:
            if (raw is string s)
            {
                // If it's something like "eax" or a memory reference "[someVar]", return as is
                if (s == "eax" || s == "ebx" || s.StartsWith("[") || char.IsLetter(s[0]))
                {
                    return s;
                }
                // Otherwise, it might be a number in string form
                if (int.TryParse(s, out _))
                    return s; // e.g. "42"
            }

            // If it's an int, just return that
            if (raw is int i)
            {
                return i.ToString();
            }

            // If it's a variable name, return [varName]
            // Because some parse methods just return the string varName
            string possibleVar = raw?.ToString() ?? "0";
            if (declaredVariables.Contains(possibleVar))
            {
                // It's a known variable
                return $"[{possibleVar}]";
            }
            // Otherwise assume numeric immediate
            return possibleVar;
        }


        // -------------------------------------------------------
        // WriteAssemblyToFile
        // -------------------------------------------------------
        public void WriteAssemblyToFile(string filePath)
        {
            // 1) Insert data + bss at the top
            assemblyCode.InsertRange(0, new[]
            {
                "extern printf",
                "section .data",
                "format db \"%d\", 0",
                "section .bss"
            });

            // 2) Insert .text + _start: lines after BSS lines
            int bssIndex = assemblyCode.IndexOf("section .bss");

            foreach (var item in declaredVariables)
            {
                bssIndex++;
                assemblyCode.Insert(bssIndex,$"{item} resd 1");
            }

            // after 'section .bss', you might also skip the lines for declared variables
            // if you do them dynamically. For simplicity:
            assemblyCode.InsertRange(bssIndex + 1, new[]
            {
                "section .text",
                "global _start",
                "_start:"
            });

            // 3) Insert the exit code *at the very end*
            assemblyCode.AddRange(new[]
            {
                "mov rax, 60",
                "xor rdi, rdi",
                "syscall"
            });

            File.WriteAllLines(filePath, assemblyCode);
        }

        // -------------------------------------------------------
        // Cuddle x = 123;  =>  VisitHugThis
        // -------------------------------------------------------
        public override object VisitHugThis(SqueakSpeakParser.HugThisContext context)
        {
            string varName = context.ID().GetText();
            declaredVariables.Add(varName);

            if (context.purrOperation() != null)
            {
                var value = Visit(context.purrOperation());
                // value might be "42", or "eax", or a variable name
                string operand = AsOperand(value);
                assemblyCode.Add($"mov eax, {operand}");
                assemblyCode.Add($"mov [rel {varName}], eax");
            }
            else
            {
                // default to 0
                assemblyCode.Add($"mov dword [rel {varName}], 0");
            }
            return null;
        }

        // -------------------------------------------------------
        // x = 10 + y;  =>  VisitPurrMath
        // -------------------------------------------------------
        public override object VisitPurrMath(SqueakSpeakParser.PurrMathContext context)
        {
            // Right side
            var value = Visit(context.purrOperation());

            // Left side: parse the leftExpr
            var leftNode = context.leftExpr();
            string baseVar = leftNode.ID(0).GetText();

            // For now, we only support basic variable assignment
            // No field access or array indexing yet
            if (leftNode.ID().Length > 1 || leftNode.purrOperation().Length > 0)
            {
                Console.WriteLine("[Error] Field access and array indexing not yet supported in assembly");
                return null;
            }

            declaredVariables.Add(baseVar);

            // purrOperation should leave final result in "eax"
            // or it might literally return "eax" as the "result register".

            // If purrOperation returns "eax", we're good.
            // But if it returns a number or [var], we just do a mov.
            string operand = AsOperand(value);

            // If the final is not already "eax", load it
            assemblyCode.Add($"mov eax, {operand}");
            // store in [varName]
            assemblyCode.Add($"mov [{baseVar}], eax");

            return null;
        }

        // -------------------------------------------------------
        // Squeak "Hello world!";
        // We’ll do a simple approach: store the string in the data section,
        // then call printf with that string.
        // -------------------------------------------------------
        public override object VisitSqueakOut(SqueakSpeakParser.SqueakOutContext context)
        {
            var value = Visit(context.purrOperation());
            // Typically purrOperation might produce a string or a numeric result.

            // For simplicity, let's assume it's a string literal. If you want to handle integers,
            // you'd do a check here if it’s numeric, etc.
            string msgLabel = $"msg_{assemblyCode.Count}";

            // We place the string in the .data section. We'll do that *inline* here.
            // Easiest is to do an assembly directive right now, but that can appear out of place
            // if we’re in the middle of .text. One approach is to just do it anyway.
            assemblyCode.Add($"section .data");
            // Make sure we escape quotes if needed. For a quick fix, assume value has no quotes.
            assemblyCode.Add($"{msgLabel} db \"{value}\", 0");
            assemblyCode.Add($"section .text");

            // SysV calling convention:
            //   rdi = address of string
            //   then call printf
            assemblyCode.Add($"mov rdi, {msgLabel}");
            assemblyCode.Add($"xor rax, rax");       // 0 float registers used
            assemblyCode.Add($"call printf");

            return null;
        }

        // -------------------------------------------------------
        // SnuggleLoop(...) { ... }
        // We'll generate a label, do a comparison, jump if false, then the body, then jump back.
        // -------------------------------------------------------
        public override object VisitSnugLoop(SqueakSpeakParser.SnugLoopContext context)
        {
            string loopStart = $"loop_start_{assemblyCode.Count}";
            string loopEnd = $"loop_end_{assemblyCode.Count}";

            assemblyCode.Add($"{loopStart}:");

            // Generate the comparison instructions.
            // We'll jump to loopEnd if the condition is false.
            GenerateCondition(context.condition(), out string jumpIfFalse);
            assemblyCode.Add($"{jumpIfFalse} {loopEnd}");

            // Loop body
            foreach (var stmt in context.adorableStatement())
            {
                Visit(stmt);
            }

            // Jump back to start
            assemblyCode.Add($"jmp {loopStart}");
            assemblyCode.Add($"{loopEnd}:");
            return null;
        }

        // -------------------------------------------------------
        // IfSnuggle(...) { ifBlock } ElseSnuggle { elseBlock }
        // We'll generate a compare, jumpIfFalse -> else, then if block, jump end, else block, end:
        //
        //   cmp ...
        //   jumpIfFalse labelElse
        //   ... if block ...
        //   jmp labelEnd
        //   labelElse:
        //   ... else block ...
        //   labelEnd:
        // -------------------------------------------------------
        public override object VisitSnuggleIf(SqueakSpeakParser.SnuggleIfContext context)
        {
            string labelTrue = $"label_true_{assemblyCode.Count}";
            string labelEnd = $"label_end_{assemblyCode.Count}";
            string labelElse = $"label_else_{assemblyCode.Count}";

            // Generate condition. We get the jump instruction that triggers if "false".
            GenerateCondition(context.condition(), out string jumpIfFalse);
            // e.g. if condition is ==, jumpIfFalse might be "jne"

            // jump to else if false
            // i.e. the condition is "false" => go to else
            assemblyCode.Add($"{jumpIfFalse} {labelElse}");

            // True block
            foreach (var stmt in context._ifBlock)
            {
                Visit(stmt);
            }

            // jump to end
            assemblyCode.Add($"jmp {labelEnd}");

            // Else block
            assemblyCode.Add($"{labelElse}:");
            if (context._elseBlock != null)
            {
                foreach (var stmt in context._elseBlock)
                {
                    Visit(stmt);
                }
            }

            // End
            assemblyCode.Add($"{labelEnd}:");

            return null;
        }

        // -------------------------------------------------------
        // purrOperation: e.g. "10 + 20 * x"
        // We'll generate instructions that end with the result in EAX.
        // We'll return "eax" from this visitor so the caller can keep going or store EAX, etc.
        // -------------------------------------------------------
        //public override object VisitPurrOperation(SqueakSpeakParser.PurrOperationContext context)
        //{
        //    // Evaluate the first term
        //    var leftVal = Visit(context.purrTerm(0));
        //    string leftOp = AsOperand(leftVal);

        //    // If no operators, just return that operand
        //    if (context.purrOperator().Length == 0)
        //    {
        //        // If it's a variable or an immediate, ensure it's handled correctly
        //        string operand = leftOp;

        //        // Wrap variable names in brackets to access memory
        //        if (declaredVariables.Contains(leftOp))
        //        {
        //            operand = $"[rel {leftOp}]";
        //        }

        //        assemblyCode.Add($"mov eax, {operand}");
        //        return "eax";
        //    }


        //    // We'll do leftOp in EAX
        //    assemblyCode.Add($"mov eax, {leftOp}");

        //    // Then apply each operator in sequence
        //    for (int i = 0; i < context.purrOperator().Length; i++)
        //    {
        //        string op = context.purrOperator(i).GetText(); // +, -, *, /
        //        // Right term
        //        var rightVal = Visit(context.purrTerm(i + 1));
        //        string rightOp = AsOperand(rightVal);

        //        // move right into EBX
        //        assemblyCode.Add($"mov ebx, {rightOp}");

        //        switch (op)
        //        {
        //            case "+":
        //                assemblyCode.Add("add eax, ebx");
        //                break;
        //            case "-":
        //                assemblyCode.Add("sub eax, ebx");
        //                break;
        //            case "*":
        //                assemblyCode.Add("imul ebx");
        //                // EAX = EAX * EBX
        //                break;
        //            case "/":
        //                // 32‐bit signed division: EDX:EAX / EBX => EAX
        //                assemblyCode.Add("cdq");       // sign‐extend EAX into EDX
        //                assemblyCode.Add("idiv ebx");
        //                break;
        //            default:
        //                throw new NotSupportedException($"Operator '{op}' not supported");
        //        }
        //    }

        //    // Final result is in EAX
        //    return "eax";
        //}

        // -------------------------------------------------------
        // Normally you'd also implement VisitPurrTerm, VisitBaseTerm, etc.
        // for the grammar to handle numbers, variable references, parentheses, etc.
        // For brevity, let's assume purrTerm simply returns the text (ID or NUMBER).
        // You can adapt the approach from your working interpreter if you need to
        // handle strings vs. floats vs. ints, etc.
        //
        // If your grammar has "purrTerm -> ID | NUMBER | STRING | '(' purrOperation ')'":
        // you'd do something like:
        // -------------------------------------------------------
        public override object VisitPurrTerm(SqueakSpeakParser.PurrTermContext ctx)
        {
            // If purrTerm -> baseTerm, call Visit on baseTerm()
            if (ctx.baseTerm() != null)
            {
                return Visit(ctx.baseTerm());
            }
            // If purrTerm -> '(' purrOperation ')', do:
            if (ctx.purrOperation() != null)
            {
                foreach (var pop in ctx.purrOperation())
                {
                    Visit(pop);
                }
            }

            // Fallback:
            return "0";
        }

        public override object VisitBaseTerm(SqueakSpeakParser.BaseTermContext ctx)
        {
            if (ctx.ID() != null)
            {
                // Return the variable name as a string
                return ctx.ID().GetText();
            }
            else if (ctx.NUMBER() != null)
            {
                // Return the numeric literal
                return ctx.NUMBER().GetText();
            }
            else if (ctx.STRING() != null)
            {
                // Strip off the quotes or handle them as needed
                string raw = ctx.STRING().GetText(); // e.g. "\"Hello\""
                return raw.Substring(1, raw.Length - 2);
            }
            else if (ctx.arrayLiteral() != null)
            {
                // Possibly handle arrays
                return Visit(ctx.arrayLiteral());
            }
            // etc.

            return "0";
        }


        // -------------------------------------------------------
        // Condition code‐generation
        // For a condition "x == 10" or "x < y", we do:
        //   mov eax, [x]
        //   cmp eax, [y or immediate]
        // We'll not jump right here; we just do the compare.
        // Then we pass back a "jump if false" instruction, e.g. for "==" => "jne".
        // -------------------------------------------------------
        private void GenerateCondition(SqueakSpeakParser.ConditionContext ctx, out string jumpIfFalse)
        {
            // left side
            string leftId = ctx.ID(0).GetText();
            // right side might be second ID or a NUMBER
            string op = ctx.GetChild(1).GetText(); // e.g. "==", "!=", "<", ">", "<=", ">="

            string rightOperand;
            if (ctx.ID().Length > 1) // x == y
            {
                rightOperand = ctx.ID(1).GetText();
            }
            else
            {
                // x == 5
                rightOperand = ctx.NUMBER().GetText();
            }

            // compare
            assemblyCode.Add($"mov eax, [{leftId}]");
            assemblyCode.Add($"cmp eax, {rightOperand}");

            // For an if/while, we want "jump if false." So if the condition is "==" => jump if not equal => "jne"
            jumpIfFalse = op switch
            {
                "==" => "jne",
                "!=" => "je",
                "<" => "jge",
                ">" => "jle",
                "<=" => "jg",
                ">=" => "jl",
                _ => throw new NotSupportedException($"Operator {op} not supported in condition")
            };
        }
    }
}
