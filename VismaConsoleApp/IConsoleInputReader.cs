namespace VismaConsoleApp
{
    public interface IConsoleInputReader
    {
        ConsoleKey ReadKey();

        string? ReadLine();

        int ReadNumber();

        string GetTitle();

        string GetName();

        string GetRoom();

        string GetCategory();

        int GetPriority();
    }
}
