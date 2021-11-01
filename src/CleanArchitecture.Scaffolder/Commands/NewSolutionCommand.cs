using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CleanArchitecture.Scaffolder.Constants;
using CleanArchitecture.Scaffolder.DotNet;
using CleanArchitecture.Scaffolder.Factories;
using CleanArchitecture.Scaffolder.Models;
using CleanArchitecture.Scaffolder.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CleanArchitecture.Scaffolder.Commands;

public class NewSolutionCommand : Command<NewSolutionSettings>
{
    private readonly SolutionStructureFactory _solutionStructureFactory = new();
    private static readonly List<string> TestFrameworks = new() {"xunit", "mstest", "nunit"};

    public override int Execute([NotNull] CommandContext context, [NotNull] NewSolutionSettings settings)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select a project type:")
                .AddChoices(SolutionChoices.CleanWeb)
            );

        settings.Path ??= Directory.GetCurrentDirectory();

        SolutionDetails? solutionDetails = _solutionStructureFactory.GetForSelection(selected, settings.RootNamespace!);

        if (solutionDetails is null)
        {
            AnsiConsole.WriteLine($"[red]Project selection {selected} not found[/]");
            return -1;
        }

        var rootWorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), settings.Path);
        
        AnsiConsole.Status()
            .Start($"Creating your {selected} project ", ctx => 
            {
                if (settings.SdkVersion is not null)
                {
                    AnsiConsole.WriteLine("");
                    AnsiConsole.Write(new Rule($"Setting Sdk Version ({settings.SdkVersion})") {Alignment = Justify.Left});
                    AnsiConsole.WriteLine("");

                    DotNetCli.NewGlobalJson(settings.SdkVersion, rootWorkingDirectory);
                    AnsiConsole.MarkupLine("[aqua]Successfully created global.json file[/]");
                }
                
                AnsiConsole.WriteLine("");
                CreateSolutionFile(settings, rootWorkingDirectory);
                AnsiConsole.WriteLine("");
                
                CreateProjects(settings, solutionDetails);
                AnsiConsole.WriteLine("");
                
                AddProjectsToSolution(settings, solutionDetails, rootWorkingDirectory);
                AnsiConsole.WriteLine("");
                
                AddProjectReferences(solutionDetails, rootWorkingDirectory);
                AnsiConsole.WriteLine("");

                AddNugetPackages(solutionDetails, rootWorkingDirectory);
            });

        return 0;
    }

    private static void AddNugetPackages(SolutionDetails solutionDetails, string rootWorkingDirectory)
    {
        AnsiConsole.Write(new Rule("Project Nuget References") {Alignment = Justify.Left});
        AnsiConsole.WriteLine("");

        foreach (var project in solutionDetails.ProjectDetails.Where(x => x.ProjectNugetReferences.Any()))
        {
            var projectPath = project.GetFullDirectoryPath(rootWorkingDirectory);

            AnsiConsole.MarkupLine($"[aqua]Adding ({project.ProjectNugetReferences.Count}) nuget project references to {project.Name}[/]");
            
            foreach (var nugetReference in project.ProjectNugetReferences)
            {
                DotNetCli.AddNugetPackage(projectPath, nugetReference.Name, nugetReference.Version);
            }
        }
    }

    private static void AddProjectReferences(SolutionDetails solutionDetails, string rootWorkingDirectory)
    {
        AnsiConsole.Write(new Rule("Project References"){Alignment = Justify.Left});
        AnsiConsole.WriteLine("");

        foreach (var project in solutionDetails.ProjectDetails.Where(x => x.ProjectReferences.Any()))
        {
            AnsiConsole.MarkupLine($"[aqua]Adding ({project.ProjectReferences.Count}) nuget packages for[/] [aqua underline]{project.Name}[/]");

            var projectPath = project.GetFullDirectoryPath(rootWorkingDirectory);

            foreach (var reference in project.ProjectReferences)
            {
                string referencePath = reference.Name;

                if (reference.RelativePath is not null)
                {
                    referencePath = Path.Combine(reference.RelativePath, reference.Name);
                }

                referencePath = Path.Combine(rootWorkingDirectory, referencePath);

                DotNetCli.AddProject(projectPath, referencePath).WaitForExit();
            }
        }
    }

    private static void AddProjectsToSolution(NewSolutionSettings settings, SolutionDetails solutionDetails, string rootWorkingDirectory)
    {
        AnsiConsole.Write(new Rule("Solution References"){Alignment = Justify.Left});
        AnsiConsole.WriteLine("");
        
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
        AnsiConsole.Write(new Rule("Project Creation"){Alignment = Justify.Left});
        AnsiConsole.WriteLine("");
        
        foreach (var project in solutionDetails.ProjectDetails)
        {
            string directoryToUse = settings.Path ?? Directory.GetCurrentDirectory();

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
        AnsiConsole.Write(new Rule("Solution Creation"){Alignment = Justify.Left});
        AnsiConsole.WriteLine("");
        
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