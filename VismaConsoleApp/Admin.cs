namespace VismaConsoleApp
{
    public class Admin(string name) : User(name)
    {
        public override string ToString()
        {
            return this.Name + " (admin)";
        }
    }
}
