using VismaConsoleApp;

namespace VismaConsoleAppTests
{
    public class AdminTests
    {
        [Fact]
        public void ToString_ReturnsCorrectString()
        {
            User user = new Admin("Brad");
            string expected = user.Name + " (admin)";
            Assert.Equal(expected, user.ToString());
        }
    }
}
