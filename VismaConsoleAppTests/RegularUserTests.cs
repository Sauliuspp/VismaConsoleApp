using VismaConsoleApp;

namespace VismaConsoleAppTests
{
    public class RegularUserTests
    {
        [Fact]
        public void ToString_ReturnsCorrectString()
        {
            User user = new RegularUser("John");
            string expected = user.Name + " (regular user)";
            Assert.Equal(expected, user.ToString());
        }
    }
}
