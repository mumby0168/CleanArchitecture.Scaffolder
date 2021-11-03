using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using CleanArchitecture.Scaffolder.Constants;
using CleanArchitecture.Scaffolder.Factories;
using CleanArchitecture.Scaffolder.Json;
using CleanArchitecture.Scaffolder.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Commands.Template;

public class TemplateSampleCommand : Command<TemplateSampleSettings>
{
    private readonly SolutionStructureFactory _structureFactory = new();

    public override int Execute([NotNull] CommandContext context, [NotNull] TemplateSampleSettings settings)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select a project type to generate the template from:")
                .AddChoices(SolutionChoices.CleanWeb)
        );

        if (selected != SolutionChoices.CleanWeb)
        {
            return -1;
        }

        var solution = _structureFactory.GetForSelection(selected, "{namespace-inserted-by-tool}");

        var json = JsonSerializer.Serialize(solution, JsonDefaults.SerializerOptions);

        if (settings.RelativeOutputPath is not null)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), settings.RelativeOutputPath);
            File.WriteAllText(path, json);
        }
        else
        {
            AnsiConsole.Write(new Rule("[aqua] JSON Sample:[/]"){Alignment = Justify.Left});
            AnsiConsole.WriteLine(json);
        }

        return 0;
    }
}