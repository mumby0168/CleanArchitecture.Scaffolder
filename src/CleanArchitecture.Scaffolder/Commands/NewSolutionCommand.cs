using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CleanArchitecture.Scaffolder.DotNet;
using CleanArchitecture.Scaffolder.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Commands;

public class NewSolutionCommand : Command<NewSolutionSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] NewSolutionSettings settings)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select a project type:")
                .AddChoices(new []
                {
                    "clean-web",
                    "clean-library"
                })
            );

        DotNetCli.New(settings.RootNamespace!, "console", settings.Path ?? Directory.GetCurrentDirectory()).WaitForExit();

        return 0;
    }

    public override ValidationResult Validate([NotNull] CommandContext context, [NotNull] NewSolutionSettings settings)
    {
        if (settings.RootNamespace is null)
        {
            return ValidationResult.Error("A root namespace is required");
        }

        if (settings.Path is not null && Uri.IsWellFormedUriString(settings.Path, UriKind.RelativeOrAbsolute) is false)
        {
            return ValidationResult.Error($"{settings.Path} is not a relative or absolute path");
        }
        
        return base.Validate(context, settings);
    }
}