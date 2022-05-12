namespace ETSlipsMigrationTool.Models
{
    /// <summary>
    /// A class prefix
    /// </summary>
    internal class Prefix
    {
        /// <summary>
        /// The ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Id of the category
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Prefix"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="name">The name.</param>
        public Prefix(int id, int categoryId, string name)
        {
            Id = id;
            CategoryId = categoryId;
            Name = name;
        }
    }
}