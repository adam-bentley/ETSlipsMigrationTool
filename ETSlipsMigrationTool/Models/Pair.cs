using ETSlipsMigrationTool.Models.Enums;

namespace ETSlipsMigrationTool.Models
{
    /// <summary>
    /// A pair of vehicles going down the track
    /// </summary>
    internal class Pair
    {
        /// <summary>
        /// The timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The id of the event
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Id of the category
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// The round
        /// </summary>
        public string Round { get; set; }

        /// <summary>
        /// The finish distance
        /// </summary>
        public Finishes Finish { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair"/> class.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="round">The round.</param>
        /// <param name="finish">The finish.</param>
        public Pair(DateTime timestamp, int eventId, int categoryId, string round, int finish)
        {
            Timestamp = timestamp;
            EventId = eventId;
            CategoryId = categoryId;
            Round = round;
            Finish = (Finishes)finish;
        }
    }
}