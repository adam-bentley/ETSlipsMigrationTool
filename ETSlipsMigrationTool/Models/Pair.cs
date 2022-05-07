namespace ETSlipsMigrationTool.Models
{
    internal class Pair
    {
        public DateTime Timestamp { get; set; }
        public int EventId { get; set; }

        public int CategoryId { get; set; }

        public string Round { get; set; }

        public int Finish { get; set; }

        public Pair(DateTime timestamp, int eventId, int categoryId, string round, int finish)
        {
            Timestamp = timestamp;
            EventId = eventId;
            CategoryId = categoryId;
            Round = round;
            Finish = finish;
        }
    }
}