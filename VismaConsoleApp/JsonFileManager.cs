using System.Text.Json;

namespace VismaConsoleApp
{
    public class JsonFileManager(string fileName)
    {
        public string FileName { get; set; } = fileName;

        public List<Resource> ReadFile(string fileName)
        {
            List<Resource> resources = null;
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, JsonSerializer.Serialize<List<Resource>>(
                        new List<Resource>(),
                        new JsonSerializerOptions { WriteIndented = true })
                    );
            }
            string json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<Resource>>(json);
        }

        public void WriteToFile(List<Resource> resources, string fileName)
        {
            string jsonString = JsonSerializer.Serialize(resources, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, jsonString);
        }
    }
}
