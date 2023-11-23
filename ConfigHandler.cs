using System.Diagnostics;
using System.Text.Json;

public class ConfigHandler<T> where T : new()
{
    private string configPath;
    public T data;

    public ConfigHandler(string configPath)
    {
        this.configPath = configPath;
        data = new(); // Default config

        if (File.Exists(configPath)) LoadConfig();
    }

    public void LoadConfig()
    {
        try
        {
            T? newData = JsonSerializer.Deserialize<T>(File.ReadAllText(configPath));
            if (newData != null) data = newData;
        }
        catch (Exception) { Console.WriteLine("Error loading config file. Using default instead."); }
    }

    public void EditConfig()
    {
        if (!File.Exists(configPath)) SaveCurrentConfig();
        Process.Start(new ProcessStartInfo("explorer", configPath));
    }

    public void SaveCurrentConfig()
    {
        File.WriteAllText(configPath, JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true, IncludeFields = true }));
    }
}
