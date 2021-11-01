namespace CleanArchitecture.Scaffolder.Models;

public static class CleanArchitectureWebProject
{
    public static SolutionDetails Create(string rootNamespace) => new()
    {
        RootNamespace = rootNamespace,
        ProjectDetails = new List<ProjectDetails>
        {
            new()
            {
                Name = BuildProjectName(rootNamespace, "Domain"),
                RelativeDirectory = "src",
                Template = "classlib"
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Application"),
                RelativeDirectory = "src",
                Template = "classlib"
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Infrastructure"),
                RelativeDirectory = "src",
                Template = "classlib"
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Web"),
                RelativeDirectory = "src"
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Domain.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit"
            },  
            new()
            {
                Name = BuildProjectName(rootNamespace, "Application.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit"
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Infrastructure.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit"
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Web.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit"
            }
        }
    };

    private static string BuildProjectName(string rootNamespace, string suffix) => $"{rootNamespace}.{suffix}";
}