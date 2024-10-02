namespace VismaConsoleApp
{
    public class ConsoleInputReader : IConsoleInputReader
    {
        public ConsoleKey ReadKey()
        {
            ConsoleKey key = Console.ReadKey().Key;
            Console.WriteLine();
            return key;
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public int ReadNumber()
        {
            return Convert.ToInt32(Console.ReadLine());
        }

        public string GetTitle()
        {
            Console.WriteLine("Enter title:");
            return Console.ReadLine();
        }

        public string GetName()
        {
            Console.WriteLine("Enter user name:");
            return Console.ReadLine();
        }

        public string GetRoom()
        {
            Console.WriteLine("Enter room (meeting room / kitchen / bathroom):");
            return this.ReadLine();
        }

        public string GetCategory()
        {
            Console.WriteLine("Enter category (electronics / food / other):");
            return this.ReadLine();
        }

        public int GetPriority()
        {
            Console.WriteLine("Enter priority (1 - 10):");
            try
            {
                return this.ReadNumber();
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
