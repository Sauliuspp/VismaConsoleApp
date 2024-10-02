namespace VismaConsoleApp
{
    public class RegularUser(string name) : User(name)
    {
        public override string ToString()
        {
            return this.Name + " (regular user)";
        }
    }
}
