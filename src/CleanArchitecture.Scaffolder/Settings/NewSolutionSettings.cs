using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Settings;

public class NewSolutionSettings : CommandSettings
{
    [CommandOption( "--root-namespace")]
    public string? RootNamespace { get; set; }
}