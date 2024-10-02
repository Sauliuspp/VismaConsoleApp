namespace VismaConsoleApp
{
    public class UserManager
    {
        private List<User> Users = new List<User>();

        private User CurrentUser;

        public List<User> GetUsers()
        {
            return Users;
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public User? GetUser(string name)
        {
            foreach (var user in Users)
            {
                if (user.Name == name)
                {
                    return user;
                }
            }
            return null;
        }

        public User GetCurrentUser()
        {
            return CurrentUser;
        }

        public void SetCurrentUser(ref User user)
        {
            if (user == null)
            {
                Console.WriteLine("Could not set user");
                return;
            }
            CurrentUser = user;
        }
    }
}
