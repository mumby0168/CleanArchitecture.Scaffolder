using System.Runtime.Serialization.Json;
using CleanArchitecture.Scaffolder.Commands;
using CleanArchitecture.Scaffolder.Commands.Solution;
using CleanArchitecture.Scaffolder.Commands.Template;
using CleanArchitecture.Scaffolder.Settings;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.Settings.ApplicationName = "dnsg";

    config.AddCommand<SolutionNewCommand>("new")
        .WithDescription("Creates a new clean architecture solution");


    config.AddBranch("templates", configurator =>
    {
        configurator.AddCommand<TemplateSampleCommand>("sample")
            .WithDescription("Generates a json sample of a given template");

        configurator.AddCommand<TemplateSaveCommand>("save")
            .WithDescription("Save a template for use with the tool.");
    });
});


await app.RunAsync(args);