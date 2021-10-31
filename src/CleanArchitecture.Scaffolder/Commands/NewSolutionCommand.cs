using CleanArchitecture.Scaffolder.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Commands;

public class NewSolutionCommand : AsyncCommand<NewSolutionSettings>
{
    public async override Task<int> ExecuteAsync(CommandContext context, NewSolutionSettings settings)
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
        
        AnsiConsole.WriteLine($"You selected {selected}");

        return 0;
    }

    public override ValidationResult Validate(CommandContext context, NewSolutionSettings settings)
    {
        if (settings.RootNamespace is null)
        {
            return ValidationResult.Error("A root namespace is required");
        }
        
        return base.Validate(context, settings);
    }
}