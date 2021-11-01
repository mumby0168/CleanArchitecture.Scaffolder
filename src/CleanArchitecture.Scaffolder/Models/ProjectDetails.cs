namespace CleanArchitecture.Scaffolder.Models;

public class ProjectDetails
{
    public string Template { get; set; } = "web";

    public string Name { get; set; } = "Undefined";

    public string? RelativeDirectory { get; set; }

    public List<string> PackagesToInstall { get; set; } = new();

    public List<string> ProjectReferences { get; set; } = new();
}