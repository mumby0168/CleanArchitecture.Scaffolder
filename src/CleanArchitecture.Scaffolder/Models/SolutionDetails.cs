using System.Text.Json.Serialization;

namespace CleanArchitecture.Scaffolder.Models;

public class SolutionDetails
{
    [JsonIgnore]
    public string RootNamespace { get; set; } = "UndefinedNamespace";

    public List<ProjectDetails> ProjectDetails { get; set; } = new();

    public void SetRootNamespace(string rootNamespace) => 
        ProjectDetails.ForEach(x => x.Name = $"{rootNamespace}.{x.Name}");
}