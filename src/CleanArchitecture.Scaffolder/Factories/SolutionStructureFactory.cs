using CleanArchitecture.Scaffolder.Constants;
using CleanArchitecture.Scaffolder.Models;

namespace CleanArchitecture.Scaffolder.Factories;

public class SolutionStructureFactory
{
    public SolutionDetails? GetForSelection(string selection, string rootNamespace) =>
        selection switch
        {
            SolutionChoices.CleanWeb => CleanArchitectureWebProject.Create(rootNamespace),
            _ => null
        };
}