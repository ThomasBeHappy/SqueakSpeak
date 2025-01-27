# 🐭 SqueakSpeak Syntax Guide

Welcome to the SqueakSpeak syntax guide! This document will help you understand how to write code in the most adorable programming language ever created! 🐁✨

## Basic Syntax

### Output
Use `Squeak` to print to the console:
```squeakspeak
Squeak "Hello little mouse!";
Squeak 42;
Squeak myVariable;
```

### Variables
Declare variables using `Cuddle`:
```squeakspeak
Cuddle myNumber = 42;
Cuddle myText = "Cheese is yummy!";
```

### Input
Use `Listen` to get user input:
```squeakspeak
Listen(userInput);  // Stores input in userInput variable
```

### Objects
Create objects using `SnuggleObject`:
```squeakspeak
SnuggleObject Mouse {
    Mouse->name = "Pip";
    Mouse->age = 2;
    Mouse->favoriteFood = "Cheese";
}
```

Access object fields using `->`:
```squeakspeak
Squeak Mouse->name;  // Outputs: Pip
```

### Control Flow

#### If Statements (`Peek`/`Purr`)
```squeakspeak
Peek (counter == 0) {
    Squeak "Counter is zero!";
} Purr {
    Squeak "Counter is not zero!";
}
```

#### Loops (`Nuzzle`)
```squeakspeak
Nuzzle (x < 5) {
    Squeak x;
    x = x + 1;
}
```

### Functions

Define functions using `FluffMagic`:
```squeakspeak
FluffMagic greetMouse(name) {
    Squeak "Hello, " + name + "!";
}
```

Call functions using their name:
```squeakspeak
greetMouse("Pip");
```

Return values using `PawReturn`:
```squeakspeak
FluffMagic add(a, b) {
    PawReturn a + b;
}
```

### Math Operations
Basic math operations are supported:
```squeakspeak
Cuddle result = 5 + 3;  // Addition
result = 10 - 4;        // Subtraction
result = 6 * 7;         // Multiplication
result = 15 / 3;        // Division
result = 10 % 3;        // Modulo
result = 2 ^ 3;         // Power
```

Use `Brain` for math functions:
```squeakspeak
Brain(sin, 45) -> result;    // Sine
Brain(cos, 45) -> result;    // Cosine
Brain(tan, 45) -> result;    // Tangent
Brain(sqrt, 16) -> result;   // Square root
Brain(pow, 2, 3) -> result;  // Power
```

### Network Operations
Make HTTP requests using `BeepBoop`:
```squeakspeak
BeepBoop("https://example.com") -> response;
```

### File Operations
Import other SqueakSpeak files using `BringWarmth`:
```squeakspeak
BringWarmth "otherfile.ssp";
```

### Comments
```squeakspeak
// Single line comment

/* 
   Multi-line
   comment
*/
```

### Switch Statement (`SnipChoose`)
```squeakspeak
SnipChoose (value) {
    SnipCase 1: {
        Squeak "One!";
    }
    SnipCase "test": {
        Squeak "Test case!";
    }
    SnipDefault: {
        Squeak "Default case!";
    }
}
```

### Native Function Calls
Call .NET functions using `NativeCall`:
```squeakspeak
NativeCall "System.Math" "Abs" (-42) -> result;
```

## Data Types

- Numbers (integers and floating-point)
- Strings (enclosed in double quotes)
- Objects (created with SnuggleObject)
- Arrays (using square brackets)

## Best Practices

1. Use descriptive variable names
2. Comment your code to explain complex operations
3. Break down complex operations into smaller functions
4. Keep your code organized and properly indented
5. Use proper error handling where appropriate

Need more help? Join our mouse community at [GitHub](https://github.com/ThomasBeHappy/SqueakSpeak)! 🐭✨ 