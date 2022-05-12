namespace ETSlipsMigrationTool.Models
{
    /// <summary>
    /// A race event
    /// </summary>
    internal class RaceEvent
    {
        /// <summary>
        /// The ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RaceEvent"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public RaceEvent(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}