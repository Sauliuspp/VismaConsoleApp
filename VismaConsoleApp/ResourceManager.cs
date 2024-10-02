namespace VismaConsoleApp
{
    internal class ResourceManager(IConsoleInputReader inputReader, JsonFileManager jsonFileManager)
    {
        private IConsoleInputReader inputReader = inputReader;
        private JsonFileManager jsonFileManager = jsonFileManager;

        public Resource CreateResourceShortage(string userName)
        {
            ConsoleInputReader inputReader = new ConsoleInputReader();
            string title = inputReader.GetTitle();

            List<string> viableRooms = ["meeting room", "kitchen", "bathroom"];
            Console.WriteLine("Enter room (meeting room / kitchen / bathroom):");
            string room = this.inputReader.ReadLine();
            if (!viableRooms.Any(i => room.Contains(i)))
            {
                Console.WriteLine("Invalid room");
                return null;
            }

            List<string> viableCategories = ["electronics", "food", "other"];
            Console.WriteLine("Enter Category (electronics / food / other):");
            string category = this.inputReader.ReadLine();
            if (!viableCategories.Any(i => category.Contains(i)))
            {
                Console.WriteLine("Invalid category");
                return null;
            }

            int minPriority = 1;
            int maxPriority = 10;

            Console.WriteLine("Enter priority (1 - 10):");
            int priority = this.inputReader.ReadNumber();
            if (priority < minPriority || priority > maxPriority)
            {
                Console.WriteLine("Invalid priority");
                return null;
            }

            return new Resource(title, userName, room, category, priority);
        }

        public void AddResourceShortage(Resource newResource)
        {
            List<Resource> resourceList = this.jsonFileManager.ReadFile(this.jsonFileManager.FileName);

            Resource foundResource = resourceList.Find(i => i.Title == newResource.Title && i.Room == newResource.Room);
            if (foundResource != null)
            {
                if (foundResource.Priority < newResource.Priority)
                {
                    foundResource.Title = newResource.Title;
                    foundResource.Room = newResource.Room;
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

            this.jsonFileManager.WriteToFile(resourceList, this.jsonFileManager.FileName);
        }

        public void DeleteResourceShortage(User user)
        {
            Console.WriteLine("Enter title:");
            string title = this.inputReader.ReadLine();
            Console.WriteLine("Enter room:");
            string room = this.inputReader.ReadLine();

            List<Resource> resourceList = this.jsonFileManager.ReadFile(this.jsonFileManager.FileName);

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

            this.jsonFileManager.WriteToFile(resourceList, this.jsonFileManager.FileName);
        }

        public void ListResourceShortages(User user)
        {
            Console.WriteLine("Do you want to filter data?");
            Console.WriteLine("1 - show unfiltered data");
            Console.WriteLine("2 - filter by title");
            Console.WriteLine("3 - filter by room");
            Console.WriteLine("4 - filter by category");
            Console.WriteLine("5 - fiter by date interval");

            ConsoleKey selectionKey = this.inputReader.ReadKey();

            string filterInput = "";
            string startDate = "";
            string endDate = "";

            List<Resource> resources = this.jsonFileManager.ReadFile(this.jsonFileManager.FileName);

            bool endWhile = false;

            switch (selectionKey)
            {
                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    Console.WriteLine("Enter title:");
                    filterInput = this.inputReader.ReadLine();
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    Console.WriteLine("Enter room:");
                    filterInput = this.inputReader.ReadLine();
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    Console.WriteLine("Enter category:");
                    filterInput = this.inputReader.ReadLine();
                    break;
                case ConsoleKey.D5 or ConsoleKey.NumPad5:
                    Console.WriteLine("Enter start date (yyyy-mm-dd):");
                    startDate = this.inputReader.ReadLine();
                    Console.WriteLine("Enter end date (yyyy-mm-dd):");
                    endDate = this.inputReader.ReadLine();
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

        List<Resource> GetUserResources(User user, List<Resource> resources)
        {
            bool isRegularUser = user.GetType() == typeof(RegularUser);
            return isRegularUser ? resources.FindAll(i => i.Name == user.Name) : resources;
        }
    }
}
