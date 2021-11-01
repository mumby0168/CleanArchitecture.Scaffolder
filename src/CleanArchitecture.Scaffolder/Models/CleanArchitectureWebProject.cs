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
                Template = "classlib",
                ProjectReferences = new List<ProjectReference>()
                {
                    new(BuildProjectName(rootNamespace, "Domain"), "src")
                },
                ProjectNugetReferences = new List<ProjectNugetReference>()
                {
                    new ProjectNugetReference("MediatR")
                }
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Infrastructure"),
                RelativeDirectory = "src",
                Template = "classlib",
                ProjectReferences = new List<ProjectReference>()
                {
                    new(BuildProjectName(rootNamespace, "Application"), "src")
                },
                ProjectNugetReferences = new List<ProjectNugetReference>()
                {
                    new("IEvangelist.Azure.CosmosRepository", "2.5.3")
                }
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Web"),
                RelativeDirectory = "src",
                ProjectReferences = new List<ProjectReference>()
                {
                    new(BuildProjectName(rootNamespace, "Application"), "src"),
                    new(BuildProjectName(rootNamespace, "Infrastructure"), "src")
                }
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Domain.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit",
                ProjectReferences = new List<ProjectReference>()
                {
                    new(BuildProjectName(rootNamespace, "Domain"), "src")
                }
                
            },  
            new()
            {
                Name = BuildProjectName(rootNamespace, "Application.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit",
                ProjectReferences = new List<ProjectReference>()
                {
                    new(BuildProjectName(rootNamespace, "Application"), "src")
                }
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Infrastructure.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit",
                ProjectReferences = new List<ProjectReference>()
                {
                    new(BuildProjectName(rootNamespace, "Infrastructure"), "src")
                }
            },
            new()
            {
                Name = BuildProjectName(rootNamespace, "Web.Tests"),
                RelativeDirectory = "tests",
                Template = "xunit",
                ProjectReferences = new List<ProjectReference>()
                {
                    new(BuildProjectName(rootNamespace, "Web"), "src")
                }
            }
        }
    };

    private static string BuildProjectName(string rootNamespace, string suffix) => $"{rootNamespace}.{suffix}";
}