using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Settings;

public class TemplateSaveSettings : CommandSettings
{
    [CommandOption( "--relative-path")]
    public string? RelativePath { get; set; }
    
    [CommandOption( "--full-path")]
    public string? FullPath { get; set; }

    [CommandArgument(0, "[name]")]
    public string Name { get; set; } = $"template-{Guid.NewGuid().ToString().Substring(0, 5)}";
}