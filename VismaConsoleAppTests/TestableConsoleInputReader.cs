using VismaConsoleApp;

namespace VismaConsoleAppTests
{
    public class TestableConsoleInputReader : IConsoleInputReader
    {
        public string GetCategory()
        {
            return "food";
        }

        public int GetPriority()
        {
            return 6;
        }

        public string GetRoom()
        {
            return "kitchen";
        }

        public string GetName()
        {
            return "Peter";
        }

        public string GetTitle()
        {
            return "Some title";
        }

        public ConsoleKey ReadKey()
        {
            return ConsoleKey.D1;
        }

        public string? ReadLine()
        {
            return "";
        }

        public int ReadNumber()
        {
            return 1;
        }
    }
}
