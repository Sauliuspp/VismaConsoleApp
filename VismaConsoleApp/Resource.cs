namespace VismaConsoleApp
{
    public class Resource(string title, string name, string room, string category, int priority)
    {
        public string Title { get; set; } = title;
        public string Name { get; set; } = name;
        public string Room { get; set; } = room;
        public string Category { get; set; } = category;
        public int Priority { get; set; } = priority;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return
                "Title:" + this.Title +
                ", Name: " + this.Name +
                ", Room: " + this.Room +
                ", Category: " + this.Category +
                ", Priority: " + this.Priority +
                ", CreatedOn: " + this.CreatedOn;
        }
    }
}
