using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Settings;

public class TemplateSampleSettings : CommandSettings
{
    [CommandArgument(0, "[relativeOutputPath]")]
    public string? RelativeOutputPath { get; set; }
}