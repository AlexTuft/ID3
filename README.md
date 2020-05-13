# ID3 Algorithm

A pre-build version of the application exists in the direction _IDE3_prebuilt. 

The program contains three datasets:
- Income data (default)
- Zoo animals data
- Weather data

See the runtime args section for instructions on specifying which dataset to use.

The program was written in C# 8 using .NET Core 3.1. Requires the SDK (or runtime) which can be downloaded [here](https://dotnet.microsoft.com/download/dotnet-core/3.1).

## Building (VISUAL STUDIO 2019)

ID3.sln can be be opened and run from Visual Studio 2019. To specify runtime arguments, right click the project (not solution), open the properties window, select the debug tab, and input arguments in the 'Application aguments' text box.

## Building (COMMAND LINE)

To build the software go to the root of the project (not solution) and run the following commands:

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
`--draw-tree` | Draws the tree to the console.
`--data-split` | Sets the ratio of training data to test data. Expects an additional argument as a number between 0 and 1. The default value is 0.7.
`--dataset-weather` | Uses the weather dataset
`--dataset-zoo` | Uses the zoo animals dataset
`--test` | Sets the number of times to build/test the tree. Used for collecting performance data. Expects an additional argument as a non-negative integer. The default value is 1.
`--no-pause` | Prevents the application for pausing after finishing (useful for running in vs2019)

## Project Structure

The Data folder contains classes for storing data in an easy to work with format. The Tree folder contains all of the logic for building the ID3 decision tree, including the information gain and entropy functions. Loaders contains classes to help load in datasets.