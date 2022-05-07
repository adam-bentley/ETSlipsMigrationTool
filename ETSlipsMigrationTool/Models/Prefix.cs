namespace ETSlipsMigrationTool.Models
{
    internal class Prefix
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public Prefix(int id, int categoryId, string name)
        {
            Id = id;
            CategoryId = categoryId;
            Name = name;
        }
    }
}