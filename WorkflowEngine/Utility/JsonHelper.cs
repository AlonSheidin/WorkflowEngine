using System.Text.Json;
using System.Text.Json.Serialization;

namespace WorkflowEngine.Utility;

public static class JsonHelper
{
    public static Process? GetProcess(string jsonFilePath)
    {
        
        Console.WriteLine($"Looking for {jsonFilePath}");
        if (File.Exists(jsonFilePath))
        {
            string data = File.ReadAllText(jsonFilePath);
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            
            Process process = JsonSerializer.Deserialize<Process>(data, options);
            return process;
        }
        
        Console.WriteLine("No definition file found");
        
        return null;
    }
}