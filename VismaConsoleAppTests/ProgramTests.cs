using VismaConsoleApp;

namespace VismaConsoleAppTests
{
    public class ProgramTests
    {
        [Fact]
        public void CreateResourceShortage_CreatesSuccessfully()
        {
            string userName = "admin";

            IConsoleInputReader inputReader = new TestableConsoleInputReader();
            Resource expected = new Resource("Some title", "admin", "kitchen", "food", 6);

            Resource createdResource = Program.CreateResourceShortage(userName, inputReader);

            Assert.Equal(expected.Title, createdResource.Title);
            Assert.Equal(expected.Name, createdResource.Name);
            Assert.Equal(expected.Room, createdResource.Room);
            Assert.Equal(expected.Category, createdResource.Category);
            Assert.Equal(expected.Priority, createdResource.Priority);
        }

        [Fact]
        public void AddResourceShortage_AddsSuccessfully()
        {
            string fileName = "Test_AddResourceShortage_AddsSuccessfully.json";

            JsonFileManager fileManager = new JsonFileManager(fileName);
            List<Resource> resources = [new Resource("Title1", "admin", "kitchen", "food", 6)];

            fileManager.WriteToFile(resources, fileName);
            List<Resource> readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);

            Resource resourceToAdd = new Resource("Title2", "admin", "bathroom", "other", 2);
            Program.AddResourceShortage(resourceToAdd, fileManager);
            readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 2);
        }

        [Fact]
        public void AddResourceShortage_ShortageAlreadyExists()
        {
            string fileName = "Test_AddResourceShortage_ShortageAlreadyExists.json";

            JsonFileManager fileManager = new JsonFileManager(fileName);
            List<Resource> resources = [new Resource("Title1", "admin", "kitchen", "food", 6)];

            fileManager.WriteToFile(resources, fileName);
            List<Resource> readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);

            Resource resourceToAdd = new Resource("Title1", "admin", "kitchen", "other", 2);
            Program.AddResourceShortage(resourceToAdd, fileManager);
            readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);
        }

        [Fact]
        public void AddResourceShortage_ShortageUpdated()
        {
            string fileName = "Test_AddResourceShortage_ShortageUpdated.json";

            JsonFileManager fileManager = new JsonFileManager(fileName);
            List<Resource> resources = [new Resource("Title1", "admin", "kitchen", "food", 6)];

            fileManager.WriteToFile(resources, fileName);
            List<Resource> readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);

            Resource resourceToAdd = new Resource("Title1", "john", "kitchen", "other", 8);
            Program.AddResourceShortage(resourceToAdd, fileManager);
            readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);
            Assert.Equal(readResources[0].ToString(), resourceToAdd.ToString());
        }

        [Fact]
        public void DeleteResourceShortage_DeletesSuccessfully()
        {
            string fileName = "Test_DeleteResourceShortage_DeletesSuccessfully.json";
            User user = new User("admin");

            JsonFileManager fileManager = new JsonFileManager(fileName);
            IConsoleInputReader inputReader = new TestableConsoleInputReader();
            List<Resource> resources = [new Resource("Some title", "admin", "kitchen", "food", 6)];

            fileManager.WriteToFile(resources, fileName);
            List<Resource> readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);

            Program.DeleteResourceShortage(user, fileManager, inputReader);
            readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 0);
        }

        [Fact]
        public void DeleteResourceShortage_ResourceDoesNotExist()
        {
            string fileName = "Test_DeleteResourceShortage_ResourceDoesNotExist.json";
            User user = new User("admin");

            JsonFileManager fileManager = new JsonFileManager(fileName);
            IConsoleInputReader inputReader = new TestableConsoleInputReader();
            List<Resource> resources = [new Resource("Title1", "admin", "meeting room", "food", 6)];

            fileManager.WriteToFile(resources, fileName);
            List<Resource> readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);

            Program.DeleteResourceShortage(user, fileManager, inputReader);
            readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);
        }

        [Fact]
        public void DeleteResourceShortage_UserDoesNotHavePermission()
        {
            string fileName = "Test_DeleteResourceShortage_UserDoesNotHavePermission.json";
            User user = new User("John");

            JsonFileManager fileManager = new JsonFileManager(fileName);
            IConsoleInputReader inputReader = new TestableConsoleInputReader();
            List<Resource> resources = [new Resource("Some title", "admin", "kitchen", "food", 6)];

            fileManager.WriteToFile(resources, fileName);
            List<Resource> readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);

            Program.DeleteResourceShortage(user, fileManager, inputReader);
            readResources = fileManager.ReadFile(fileName);

            Assert.Equal(readResources.Count, 1);
        }

        [Fact]
        public void GetUserResources_RegularUserGetsResourcesSuccessfully()
        {
            User user = new RegularUser("John");

            List<Resource> resources = new List<Resource>()
            {
                new Resource("title", "John", "room", "category", 2),
                new Resource("title2", "John", "room2", "category2", 1),
                new Resource("title2", "Admin", "room2", "category2", 8)
            };

            List<Resource> usersResources = Program.GetUserResources(user, resources);

            Assert.Equal(usersResources.Count, 2);
        }

        [Fact]
        public void GetUserResources_AdminGetsResourcesSuccessfully()
        {
            User user = new Admin("John");

            List<Resource> resources = new List<Resource>()
            {
                new Resource("title", "John", "room", "category", 2),
                new Resource("title2", "John", "room2", "category2", 1),
                new Resource("title2", "Admin", "room2", "category2", 8)
            };

            List<Resource> usersResources = Program.GetUserResources(user, resources);

            Assert.Equal(usersResources.Count, 3);
        }

        [Fact]
        public void ChangeUser_ChangesUserSuccessfully()
        {
            string userName = "John";
            string expected = "Peter";

            IConsoleInputReader inputReader = new TestableConsoleInputReader();
            UserManager userManager = new UserManager();
            User user1 = new RegularUser("John");
            User user2 = new RegularUser("Peter");
            userManager.AddUser(user1);
            userManager.AddUser(user2);
            userManager.SetCurrentUser(ref user1);

            Program.ChangeUser(userManager, inputReader);

            Assert.Equal(userManager.GetCurrentUser().Name, expected);
        }
    }
}
