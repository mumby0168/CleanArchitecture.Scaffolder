using CleanArchitecture.Scaffolder.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddBranch("solution", configurator =>
    {
        configurator.AddCommand<NewSolutionCommand>("new")
            .WithDescription("Creates a new clean architecture solution");
    });
});


await app.RunAsync(args);