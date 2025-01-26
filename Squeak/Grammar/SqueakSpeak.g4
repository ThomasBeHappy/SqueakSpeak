grammar SqueakSpeak;

// ----------------------------------------------------------------
// 1. PROGRAM ENTRY
// ----------------------------------------------------------------
program
    : (adorableStatement)* EOF
    ;

// ----------------------------------------------------------------
// 2. STATEMENTS
// ----------------------------------------------------------------
adorableStatement
    : squeakOut
    | hugThis
    | snugLoop
    | fluffMagic
    | snuggleIf
    | purrMath
    | snipChoose
    | bringWarmth
    | invokeWhimsy
	| squeakNetGet      // new
    | squeakIn          // new
    | squeakMathCall    // new
	| nativeCall
    | objectCreation
    | fieldAssignment
    | returnStatement    // new
    ;

// ----------------------------------------------------------------
// 2a. EXTRA STATEMENTS (Newly added for more functionality)
// ----------------------------------------------------------------
extraStatement
    // Example: SqueakNetGet("https://example.com") -> resultVar;
    : squeakNetGet
    // Example: SqueakIn(myVar);
    | squeakIn
    // Example: SqueakMath( sin, 3 ) -> resultVar;
    | squeakMathCall
    ;

nativeCall
    // Example usage:
    // NativeCall "System.Math" "Abs" (  -42 ) -> myResult;
    :
      'NativeCall'
      STRING   // fully qualified type name, e.g. "System.Math"
      STRING   // method name, e.g. "Abs"
      '(' paramList? ')'            // arguments
      ( '->' ID )?                  // optional result variable
      ';'
    ;


// 2a(i). Network
squeakNetGet
    : 'BeepBoop' '(' purrOperation ')' '->' ID ';'
    ;

// 2a(ii). Input
squeakIn
    : 'Listen' '(' ID ')' ';'
    ;

// 2a(iii). Math
//    SqueakMath( sin, 45 ) -> resultVar;
// or SqueakMath( sqrt, x ) -> resultVar;
// or SqueakMath( pow, 2, 3 ) -> resultVar;  // You could expand for multiple params
squeakMathCall
    : 'Brain' '(' mathFunc=ID ( ',' purrOperation (',' purrOperation)? )? ')' '->' ID? ';'
    ;

// Print statement: Squeak "Hello" ;
squeakOut
    : 'Squeak' purrOperation ';'
    ;

// Declaration: Cuddle x = "someValue";
hugThis
    : 'Cuddle' ID ('=' (purrOperation | invokeWhimsy))? ';'
    ;

// While-like loop: SnuggleLoop ( condition ) { ... }
snugLoop
    : 'Nuzzle' '(' condition ')' '{' (adorableStatement)* '}'
    ;

// Function definition: FluffMagic foo(bar) { ... }
fluffMagic
    : 'FluffMagic' ID '(' paramList? ')' '{' (adorableStatement)* '}'
    ;

// If/Else statement
snuggleIf
    : 'Peek' '(' condition ')' '{' ifBlock+=adorableStatement* '}'
      ( 'Purr' '{' elseBlock+=adorableStatement* '}' )?
    ;

/*
    The above snuggleIf rule means:
    - "IfSnuggle (condition) { (zero or more adorableStatement)* }"
      optionally followed by
    - "ElseSnuggle { (zero or more adorableStatement)* }"
    By grouping them in one rule, ANTLR sees them as a single parse tree node.
    So you won't get separate statements for 'IfSnuggle' and 'ElseSnuggle'.
*/

// Switch-like: SnipChoose (someVar) { SnipCase ... SnipDefault ... }
snipChoose
    : 'SnipChoose' '(' ID ')' '{'
        (snipCase)* (snipDefault)?
      '}'
    ;

snipCase
    : 'SnipCase' (ID | NUMBER | STRING)
      ':' '{' (adorableStatement)* '}'
    ;


snipDefault
    : 'SnipDefault'
      ':' '{' (adorableStatement)* '}'
    ;

// Import-like statement: BringWarmth "SomeFilePath";
bringWarmth
    : 'BringWarmth' STRING ';'
    ;

// Function call: InvokeWhimsy someFunc(...) ;
invokeWhimsy
    : ID '(' paramList? ')' ';'?    // Make the semicolon optional
    ;

// Assignment or math: x = purrOperation ;
purrMath
    : leftExpr '=' (purrOperation | invokeWhimsy) ';'
    ;

leftExpr
    // Start with a regular ID (the base object), then allow multiple ->field or [expr] array accesses
    : ID ( '->' ID )* ( '[' purrOperation ']' )*
    ;

objectCreation
    : 'SnuggleObject' ID '{' (fieldAssignment)* '}' // create an objectCreation
    ;

fieldAssignment
    : ID '->' ID '=' purrOperation ';'
    ;

// Add this new rule for return statements
returnStatement
    : 'PawReturn' purrOperation? ';'    // Optional expression for the return value
    ;

// ----------------------------------------------------------------
// 3. EXPRESSIONS
// ----------------------------------------------------------------

// A purrOperation is a left-associative chain of terms with operators:
// Example:   5 + x - (3 + y)
purrOperation
    : fieldAccess (purrOperator fieldAccess)*
    | invokeWhimsy    // Add this line to allow function calls in expressions
    ;

// Math/string operators (depending on your language definition)
purrOperator
    : '+' | '-' | '*' | '/' | '%' | '^'
    ;

// New rule for field access
fieldAccess
    : purrTerm ('->' ID)*
    ;

// Simplify purrTerm to not handle field access
purrTerm
    : baseTerm ('[' purrOperation ']')*  // keep array access
    | nativeCallExpr
    ;

nativeCallExpr
  : 'NativeFunc' ID '(' paramList? ')'
  ;

baseTerm
    : ID
    | NUMBER
    | FLOAT
    | STRING
    | '(' purrOperation ')' 
    | arrayLiteral
    ;

arrayLiteral
    : '[' purrOperation ( ',' purrOperation )* ']'
    ;

// ----------------------------------------------------------------
// 4. CONDITIONS
// ----------------------------------------------------------------

// Simple condition:  identifier <comparison> identifier or number
// e.g. (counter == 0), (x < y), etc.
condition
    : ID ( '==' | '!=' | '<' | '>' | '<=' | '>=' ) (ID | NUMBER | FLOAT)
    ;

// ----------------------------------------------------------------
// 5. FUNCTION PARAMETERS
// ----------------------------------------------------------------
paramList
    : param (',' param)*
    ;

param
    : ID
    | purrOperation
    ;

// ----------------------------------------------------------------
// 6. TOKENS
// ----------------------------------------------------------------
STRING
    : '"' ( ~["\r\n] )* '"'
    ;

ID
    : [a-zA-Z_] [a-zA-Z0-9_]*
    ;
	
FLOAT
    : '-'? [0-9]+ '.' [0-9]+
    ;
	
NUMBER
    : '-'? [0-9]+
    ;

// Whitespace
WS
    : [ \t\r\n]+ -> skip
    ;

// Single-line and block comments
LINE_COMMENT
    : '//' ~[\r\n]* -> skip
    ;

BLOCK_COMMENT
    : '/*' .*? '*/' -> skip
    ;
