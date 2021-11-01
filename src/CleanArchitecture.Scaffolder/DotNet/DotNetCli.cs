using System.Diagnostics;
using Spectre.Console;

namespace CleanArchitecture.Scaffolder.DotNet;

public static class DotNetCli
{
    public static Process New(string projectName, string projectType, string? directory = null, bool infoLogs = false) =>
        ProcessExtensions.StartProcess(
            "dotnet",
            $"new {projectType} -n {projectName}",
            directory,
            infoLogs ? log => AnsiConsole.MarkupLine($"[grey62]{log}[/]") : null,
            error => AnsiConsole.MarkupLine($"[red]{error}[/]"));
    
    public static Process SlnAdd(string projectRelativePath, string? directory = null, bool infoLogs = false) =>
        ProcessExtensions.StartProcess(
            "dotnet",
            $"sln add {projectRelativePath}",
            directory,
            infoLogs ? log => AnsiConsole.MarkupLine($"[grey62]{log}[/]") : null,
            error => AnsiConsole.MarkupLine($"[red]{error}[/]"));
}