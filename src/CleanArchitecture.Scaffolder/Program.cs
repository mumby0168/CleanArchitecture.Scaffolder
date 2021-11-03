using CleanArchitecture.Scaffolder.Commands;
using CleanArchitecture.Scaffolder.Commands.Solution;
using CleanArchitecture.Scaffolder.Commands.Template;
using CleanArchitecture.Scaffolder.Settings;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddBranch("solution", configurator =>
    {
        configurator.AddCommand<SolutionNewCommand>("new")
            .WithDescription("Creates a new clean architecture solution");
    });
    
    config.AddBranch("template", configurator =>
    {
        configurator.AddCommand<TemplateSampleCommand>("sample")
            .WithDescription("Generates a json sample of a given template");
    });
});


await app.RunAsync(args);