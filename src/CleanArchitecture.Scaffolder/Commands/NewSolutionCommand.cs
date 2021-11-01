using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CleanArchitecture.Scaffolder.DotNet;
using CleanArchitecture.Scaffolder.Models;
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

        settings.Path ??= Directory.GetCurrentDirectory();

        SolutionDetails? solutionDetails = null;

        if (selected == "clean-web")
        {
            solutionDetails = CleanArchitectureWebProject.Create(settings.RootNamespace!);
        }

        if (solutionDetails is null)
        {
            AnsiConsole.MarkupLine("[red] The current template is not implemented");
            return 0;
        }

        var rootWorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), settings.Path);
        
        AnsiConsole.Status()
            .Start($"Creating your {selected} project ", ctx => 
            {
                CreateSolutionFile(settings, rootWorkingDirectory);

                CreateProjects(settings, solutionDetails);
                
                AddProjectsToSolution(settings, solutionDetails, rootWorkingDirectory);
            });

        return 0;
    }

    private static void AddProjectsToSolution(NewSolutionSettings settings, SolutionDetails solutionDetails, string rootWorkingDirectory)
    {
        AnsiConsole.MarkupLine($"[aqua]Adding ({solutionDetails.ProjectDetails.Count}) projects to solution[/]");

        foreach (var projectDetail in solutionDetails.ProjectDetails)
        {
            string addPath = projectDetail.Name;

            if (projectDetail.RelativeDirectory is not null)
            {
                addPath = Path.Combine(projectDetail.RelativeDirectory, addPath);
            }

            DotNetCli.SlnAdd(addPath, rootWorkingDirectory, settings.DotnetLogs).WaitForExit();
        }
    }

    private static void CreateProjects(NewSolutionSettings settings, SolutionDetails solutionDetails)
    {
        foreach (var project in solutionDetails.ProjectDetails)
        {
            string directoryToUse = settings.Path;

            if (project.RelativeDirectory is not null)
            {
                directoryToUse = Path.Combine(directoryToUse, project.RelativeDirectory);
            }

            string fullDirectory = Path.Combine(Directory.GetCurrentDirectory(), directoryToUse);

            if (Directory.Exists(fullDirectory) is false)
            {
                Directory.CreateDirectory(fullDirectory);
            }

            AnsiConsole.MarkupLine($"[aqua]Creating project[/] [aqua underline]{project.Name}[/] [aqua bold]({project.Template})[/] ");

            DotNetCli.New(project.Name, project.Template, fullDirectory, settings.DotnetLogs).WaitForExit();
        }
    }

    private static void CreateSolutionFile(NewSolutionSettings settings, string rootWorkingDirectory)
    {
        AnsiConsole.MarkupLine($"[aqua]Creating[/] [aqua bold]sln[/]  [aqua underline]{settings.RootNamespace}.sln[/]");

        DotNetCli.New(settings.RootNamespace!, "sln", rootWorkingDirectory, settings.DotnetLogs).WaitForExit();
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