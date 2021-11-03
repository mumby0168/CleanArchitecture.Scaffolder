using System.Text.Json;

namespace CleanArchitecture.Scaffolder.Json;

public static class JsonDefaults
{
    public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };
}