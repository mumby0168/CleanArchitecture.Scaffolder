using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using CleanArchitecture.Scaffolder.Json;
using CleanArchitecture.Scaffolder.Models;
using CleanArchitecture.Scaffolder.Providers;
using CleanArchitecture.Scaffolder.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Commands.Template;

public class TemplateSaveCommand : Command<TemplateSaveSettings>
{
    private TemplatesProvider _templatesProvider = new();

    public override int Execute([NotNull] CommandContext context, [NotNull] TemplateSaveSettings settings)
    {
        var json = File.ReadAllText(settings.RelativePath is not null
            ? Path.Combine(Directory.GetCurrentDirectory(), settings.RelativePath)
            : settings.FullPath!);

        var solutionDetails = JsonSerializer.Deserialize<SolutionDetails>(json, JsonDefaults.SerializerOptions);

        _templatesProvider.SaveTemplate(settings.Name, solutionDetails!);

        return 1;
    }

    public override ValidationResult Validate([NotNull] CommandContext context, [NotNull] TemplateSaveSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.FullPath) && string.IsNullOrWhiteSpace(settings.RelativePath))
        {
            return ValidationResult.Error("Either a relative or full path must be provided to a template file");
        }

        return ValidationResult.Success();
    }
}