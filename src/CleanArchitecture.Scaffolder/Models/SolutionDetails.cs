namespace CleanArchitecture.Scaffolder.Models;

public class SolutionDetails
{
    public string RootNamespace { get; set; } = "UndefinedNamespace";

    public List<ProjectDetails> ProjectDetails { get; set; } = new();
}