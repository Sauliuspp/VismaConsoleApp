using VismaConsoleApp;

namespace VismaConsoleAppTests
{
    public class UserManagerTests
    {
        [Fact]
        public void GetUsers_ReturnsUsersSuccessfully()
        {
            UserManager userManager = new UserManager();

            var user = new User("David");
            userManager.AddUser(user);

            Assert.Equal([user], userManager.GetUsers());
        }

        [Fact]
        public void AddUser_AddsUserSuccessfully()
        {
            UserManager userManager = new UserManager();

            var user = new User("David");
            userManager.AddUser(user);

            Assert.True(userManager.GetUsers().Count == 1);
        }

        [Fact]
        public void GetUser_GetsUserSuccessfully()
        {
            UserManager userManager = new UserManager();

            var user = new User("David");
            userManager.AddUser(user);

            Assert.Equal(user, userManager.GetUser("David"));
        }

        [Fact]
        public void SetCurrentUser_SetsCurrentUserSuccessfully()
        {
            UserManager userManager = new UserManager();

            var user = new User("David");
            userManager.AddUser(user);
            userManager.SetCurrentUser(ref user);

            Assert.Equal(user, userManager.GetCurrentUser());
        }
    }
}
