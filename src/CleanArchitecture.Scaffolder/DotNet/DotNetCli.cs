using System.Diagnostics;
using Spectre.Console;

namespace CleanArchitecture.Scaffolder.DotNet;

public static class DotNetCli
{
    public static Process New(string projectName, string projectType, string? directory = null) =>
        ProcessExtensions.StartProcess(
            "dotnet",
            $"new {projectType} -n {projectName}",
            directory,
            log => AnsiConsole.Markup($"[green]{log}[/]{Environment.NewLine}"),
            error => AnsiConsole.Markup($"[red]{error}[/]{Environment.NewLine}"));
}