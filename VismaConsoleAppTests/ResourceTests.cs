using VismaConsoleApp;

namespace VismaConsoleAppTests
{
    public class ResourceTests
    {
        [Fact]
        public void ToString_ReturnsCorrectString()
        {
            Resource resource = new Resource("title", "name", "room", "category", 5);
            string expected = "Title:" + resource.Title +
                ", Name: " + resource.Name +
                ", Room: " + resource.Room +
                ", Category: " + resource.Category +
                ", Priority: " + resource.Priority +
                ", CreatedOn: " + resource.CreatedOn;
            Assert.Equal(expected, resource.ToString());
        }
    }
}
