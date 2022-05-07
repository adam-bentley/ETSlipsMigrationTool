namespace ETSlipsMigrationTool.Models
{
    internal class RaceEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RaceEvent(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}