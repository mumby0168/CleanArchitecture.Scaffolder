using System.Reflection;
using System.Text.Json;
using CleanArchitecture.Scaffolder.Json;
using CleanArchitecture.Scaffolder.Models;
using Spectre.Console;

namespace CleanArchitecture.Scaffolder.Providers;

public class TemplatesProvider
{
    private string TemplatesDirectory => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "templates");

    public void SaveTemplate(string name, SolutionDetails solution)
    {
        var json = JsonSerializer.Serialize(solution, JsonDefaults.SerializerOptions);
        EnsureTemplatesDirectoryExists();
        File.WriteAllText(Path.Combine(TemplatesDirectory, $"{name}.json"), json);
    }

    private void EnsureTemplatesDirectoryExists()
    {
        if (Directory.Exists(TemplatesDirectory))
        {
            return;
        }

        Directory.CreateDirectory(TemplatesDirectory);
    }

    public string[] GetTemplateNames()
    {
        EnsureTemplatesDirectoryExists();

        var files = Directory.GetFiles(TemplatesDirectory);

        return files.Select(Path.GetFileNameWithoutExtension).ToArray()!;
    }

    public SolutionDetails? GetTemplate(string name)
    {
        EnsureTemplatesDirectoryExists();
        var files = Directory.GetFiles(TemplatesDirectory);
        var path =  files.FirstOrDefault(x => x.Contains(name));

        if (path is null)
        {
            throw new Exception($"No template found with name {name}");
        }

        var json = File.ReadAllText(path);
        
        AnsiConsole.WriteLine(json);

        return JsonSerializer.Deserialize<SolutionDetails>(json, JsonDefaults.SerializerOptions);
    }
}