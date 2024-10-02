using VismaConsoleApp;

namespace VismaConsoleAppTests
{
    public class JsonFileManagerTests
    {
        [Fact]
        public void WriteToFile_ReadFile_WritesAndReadsFileSuccessfully()
        {
            string fileName = "ReadFile_Test.json";
            JsonFileManager jsonFileManager = new JsonFileManager(fileName);

            List<Resource> resources = new List<Resource>() { new Resource("title", "name", "room", "category", 5) };

            jsonFileManager.WriteToFile(resources, fileName);

            List<Resource> readResources = jsonFileManager.ReadFile(fileName);

            Assert.Equal(readResources[0].ToString(), resources[0].ToString());
        }
    }
}
