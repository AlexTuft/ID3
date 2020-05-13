# ID3 Algorithm

## Building (VISUAL STUDIO 2019)

VS 2019 is required to open this project. 

## Building (COMMAND LINE)

The program was written in C# 8 using .NET Core 3.1. Requires the SDK which can be downloaded [here](https://dotnet.microsoft.com/download/dotnet-core/3.1).

To build the software go to the root of the project and run the following commands:

```
dotnet build
```

The build dll can be found at `\ID3\bin\Debug\netcoreapp3.1\ID3.dll`.

## Running (COMMAND LINE)

Navigate to the directory containing `ID3.dll`, and run the following commands:

```
dotnet ID3.dll
```

This can executed from any directory, as long as the path to the DLL is adjusted accordingly.

## Runtime Args

**Command** | **Description**
-|-
`--show-errors` | Displays exceptions when attempting to classify data.
`--draw-tree` | Draws the tree to the console.
`--data-split` | Sets the ratio of training data to test data. Expects an additional argument as a number between 0 and 1. The default value is 0.7.
`--dataset-weather` | Uses the weather dataset
`--dataset-zoo` | Uses the zoo animals dataset
`--test` | Sets the number of times to build/test the tree. Used for collecting performance data. Expects an additional argument as a non-negative integer. The default value is 1.


