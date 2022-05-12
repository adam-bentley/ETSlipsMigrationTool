namespace ETSlipsMigrationTool.Models
{
    /// <summary>
    /// A race category or class
    /// </summary>
    internal class Category
    {
        /// <summary>
        /// The id of the category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}