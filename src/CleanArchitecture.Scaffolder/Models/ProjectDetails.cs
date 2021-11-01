namespace CleanArchitecture.Scaffolder.Models;

public class ProjectDetails
{
    public string Template { get; set; } = "web";

    public string Name { get; set; } = "Undefined";

    public string? RelativeDirectory { get; set; }

    public List<ProjectNugetReference> ProjectNugetReferences { get; set; } = new();

    public List<ProjectReference> ProjectReferences { get; set; } = new();

    public string GetFullDirectoryPath(string rootDirectory)
    {
        string projectPath = Path.Combine(rootDirectory, Name);

        if (RelativeDirectory is not null)
        {
            projectPath = Path.Combine(rootDirectory, Path.Combine(RelativeDirectory, Name));
        }

        return projectPath;
    }
}

public record ProjectReference(string Name, string? RelativePath = null);

public record ProjectNugetReference(string Name, string? Version = null);