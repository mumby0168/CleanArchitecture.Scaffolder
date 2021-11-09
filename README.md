# CleanArchitecture.Scaffolder
A `dotnet` cli tool to create a project setup for clean architecture.

## Getting Started

Head over and install the package from [here](https://www.nuget.org/packages/Mumby0168.CleanArchitecture.Scaffolder) or run the command below

`dotnet tool install --global Mumby0168.CleanArchitecture.Scaffolder`

The next step is to get to know the CLI tool simply run the following command to gain some information on the different commands available.

`dnsg --help`

This will output the basic commands available for the cli tool.

## Commands

### `dnsg new`

This allows you to create a solution from a pre-defined template that are provided with the tool as a default.

#### Examples

`dnsg ./my-sln-directory --root-namespace MyApp`

> This will create an solution called MyApp.sln in the folder my-sln-directory defined relative to where the command has been run.


