using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Settings;

public class NewSolutionSettings : CommandSettings
{
    [CommandOption( "--root-namespace")]
    public string? RootNamespace { get; set; }
    
    [CommandOption( "--dotnet-logs")]
    public bool DotnetLogs { get; set; } = false;
    
    [CommandArgument(0, "[path]")]
    public string? Path { get; set; }
    
    
}