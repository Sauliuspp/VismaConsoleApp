namespace VismaConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fileName = "Resources.json";

            UserManager userManager = new UserManager();
            userManager.AddUser(new Admin("Admin"));
            userManager.AddUser(new RegularUser("John"));
            userManager.AddUser(new RegularUser("Michael"));
            User currentUser = userManager.GetUser("Admin"); // default user is "Admin"
            userManager.SetCurrentUser(ref currentUser);

            IConsoleInputReader inputReader = new ConsoleInputReader();
            JsonFileManager fileManager = new JsonFileManager(fileName);

            ConsoleKey commandKey;

            do
            {
                PrintMainMenu(userManager.GetCurrentUser());
                Console.WriteLine("Enter number (0-4):");
                commandKey = inputReader.ReadKey();

                switch (commandKey)
                {
                    case ConsoleKey.D1 or ConsoleKey.NumPad1:
                        Resource createdResource = CreateResourceShortage(userManager.GetCurrentUser().Name, inputReader);
                        if (createdResource == null)
                        {
                            Console.WriteLine("Resource could not be created");
                            break;
                        }
                        AddResourceShortage(createdResource, fileManager);
                        break;
                    case ConsoleKey.D2 or ConsoleKey.NumPad2:
                        DeleteResourceShortage(userManager.GetCurrentUser(), fileManager, inputReader);
                        break;
                    case ConsoleKey.D3 or ConsoleKey.NumPad3:
                        ListResourceShortages(userManager.GetCurrentUser(), fileManager, inputReader);
                        break;
                    case ConsoleKey.D4 or ConsoleKey.NumPad4:
                        ChangeUser(userManager, inputReader);
                        break;
                    default:
                        break;
                }
            }
            while (commandKey != ConsoleKey.D0 && commandKey != ConsoleKey.NumPad0);
        }

        static void PrintMainMenu(User user)
        {
            Console.WriteLine();
            Console.WriteLine("Resource shortage management application");
            Console.WriteLine("Current user: " + user.ToString());
            Console.WriteLine("Choose command:");
            Console.WriteLine("1 - register new shortage");
            Console.WriteLine("2 - delete shortage");
            Console.WriteLine("3 - see shortage list");
            Console.WriteLine("4 - choose user");
            Console.WriteLine("0 - exit app");
        }

        public static Resource CreateResourceShortage(string userName, IConsoleInputReader inputReader)
        {
            ValidityChecker validityChecker = new ValidityChecker();
            string title = inputReader.GetTitle();

            string room = inputReader.GetRoom();
            List<string> viableRooms = ["meeting room", "kitchen", "bathroom"];
            if (!validityChecker.StringExists(viableRooms, room))
            {
                Console.WriteLine("Invalid room");
                return null;
            }

            string category = inputReader.GetCategory();
            List<string> viableCategories = ["electronics", "food", "other"];
            if (!validityChecker.StringExists(viableCategories, category))
            {
                Console.WriteLine("Invalid category");
                return null;
            }

            int priority = inputReader.GetPriority();
            int minPriority = 1;
            int maxPriority = 10;
            if (!validityChecker.IsNumberInRange(minPriority, maxPriority, priority))
            {
                Console.WriteLine("Invalid priority");
                return null;
            }

            return new Resource(title, userName, room, category, priority);
        }

        public static void AddResourceShortage(Resource newResource, JsonFileManager jsonFileManager)
        {
            List<Resource> resourceList = jsonFileManager.ReadFile(jsonFileManager.FileName);
            Resource foundResource = resourceList.Find(i => i.Title == newResource.Title && i.Room == newResource.Room);
            if (foundResource != null)
            {
                if (foundResource.Priority < newResource.Priority)
                {
                    foundResource.Name = newResource.Name;
                    foundResource.Priority = newResource.Priority;
                    foundResource.Category = newResource.Category;
                    foundResource.CreatedOn = newResource.CreatedOn;
                }
                else
                {
                    Console.WriteLine("Resource shortage already registered");
                }
            }
            else
            {
                resourceList.Add(newResource);
            }

            jsonFileManager.WriteToFile(resourceList, jsonFileManager.FileName);
        }

        public static void DeleteResourceShortage(User user, JsonFileManager fileManager, IConsoleInputReader inputReader)
        {
            string title = inputReader.GetTitle();
            string room = inputReader.GetRoom();

            List<Resource> resourceList = fileManager.ReadFile(fileManager.FileName);

            Resource foundResource = resourceList.Find(i => i.Title == title && i.Room == room);
            if (foundResource != null)
            {
                bool isAdmin = user.GetType() == typeof(Admin);

                if (isAdmin || foundResource.Name == user.Name)
                {
                    resourceList.Remove(foundResource);
                }
                else
                {
                    Console.WriteLine("Resource could not deleted");
                }
            }
            else
            {
                Console.WriteLine("Resource does not exist");
            }

            fileManager.WriteToFile(resourceList, fileManager.FileName);
        }

        static void ListResourceShortages(User user, JsonFileManager fileManager, IConsoleInputReader inputReader)
        {
            Console.WriteLine("Do you want to filter data?");
            Console.WriteLine("1 - show unfiltered data");
            Console.WriteLine("2 - filter by title");
            Console.WriteLine("3 - filter by room");
            Console.WriteLine("4 - filter by category");
            Console.WriteLine("5 - fiter by date interval");

            ConsoleKey selectionKey = inputReader.ReadKey();

            string filterInput = "";
            string startDate = "";
            string endDate = "";

            List<Resource> resources = fileManager.ReadFile(fileManager.FileName);

            bool endWhile = false;

            switch (selectionKey)
            {
                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    filterInput = inputReader.GetTitle();
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    filterInput = inputReader.GetRoom();
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    filterInput = inputReader.GetCategory();
                    break;
                case ConsoleKey.D5 or ConsoleKey.NumPad5:
                    Console.WriteLine("Enter start date (yyyy-mm-dd):");
                    startDate = inputReader.ReadLine();
                    Console.WriteLine("Enter end date (yyyy-mm-dd):");
                    endDate = inputReader.ReadLine();
                    break;
                default:
                    break;
            }

            List<Resource> usersResources = GetUserResources(user, resources);

            switch (selectionKey)
            {
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    usersResources = usersResources.FindAll(i => i.Title.Contains(filterInput));
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    usersResources = usersResources.FindAll(i => i.Room == filterInput);
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    usersResources = usersResources.FindAll(i => i.Category == filterInput);
                    break;
                case ConsoleKey.D5 or ConsoleKey.NumPad5:
                    usersResources = usersResources.FindAll(i => i.CreatedOn > DateTime.Parse(startDate) && i.CreatedOn < DateTime.Parse(endDate).AddDays(1));
                    break;
                default:
                    break;
            }

            usersResources = usersResources.OrderByDescending(i => i.Priority).ToList();

            Console.WriteLine();
            foreach (var resource in usersResources)
            {
                Console.WriteLine(resource.ToString());
            }
        }

        public static List<Resource> GetUserResources(User user, List<Resource> resources)
        {
            bool isRegularUser = user.GetType() == typeof(RegularUser);
            return isRegularUser ? resources.FindAll(i => i.Name == user.Name) : resources;
        }

        public static void ChangeUser(UserManager userManager, IConsoleInputReader inputReader)
        {
            List<User> userList = userManager.GetUsers();
            foreach (var user in userList)
            {
                Console.WriteLine(user.ToString());
            }

            string userName = inputReader.GetName();

            User selectedUser = userManager.GetUser(userName);
            userManager.SetCurrentUser(ref selectedUser);
        }
    }
}
