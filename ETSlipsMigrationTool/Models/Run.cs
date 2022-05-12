namespace ETSlipsMigrationTool.Models
{
    /// <summary>
    /// A vehicles run
    /// </summary>
    internal class Run
    {
        /// <summary>
        /// The timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The racenumber
        /// </summary>
        public string Racenumber { get; set; }

        /// <summary>
        /// The ID of the prefix
        /// </summary>
        public int PrefixId { get; set; }

        /// <summary>
        /// The drivers name
        /// </summary>
        public string Drivername { get; set; }

        /// <summary>
        /// The lane
        /// </summary>
        public char Lane { get; set; }

        /// <summary>
        /// The index
        /// </summary>
        public decimal Index { get; set; }

        /// <summary>
        /// The reaction
        /// </summary>
        public decimal Reaction { get; set; }

        /// <summary>
        /// The time to 60ft
        /// </summary>
        public decimal ET60 { get; set; }

        /// <summary>
        /// The time to 330ft
        /// </summary>
        public decimal ET330 { get; set; }

        /// <summary>
        /// The time to 594ft
        /// </summary>
        public decimal ET594 { get; set; }

        /// <summary>
        /// The time to 660ft
        /// </summary>

        public decimal ET660 { get; set; }

        /// <summary>
        /// The speed at 660
        /// </summary>
        public decimal SP660 { get; set; }

        /// <summary>
        /// The time to 934ft
        /// </summary>
        public decimal? ET934 { get; set; }

        /// <summary>
        /// The time to 1000ft
        /// </summary>
        public decimal ET1000 { get; set; }

        /// <summary>
        /// The speed at 1000
        /// </summary>
        public decimal SP1000 { get; set; }

        /// <summary>
        /// The time to 1254ft
        /// </summary>
        public decimal? ET1254 { get; set; }

        /// <summary>
        /// The time to 1320ft
        /// </summary>
        public decimal ET1320 { get; set; }

        /// <summary>
        /// The speed at 1000
        /// </summary>
        public decimal SP1320 { get; set; }

        /// <summary>
        /// The runs result
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Any remarks, [Foul, No car staged, breakout...]
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Run"/> class.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="racenumber">The racenumber.</param>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <param name="drivername">The drivername.</param>
        /// <param name="lane">The lane.</param>
        /// <param name="index">The index.</param>
        /// <param name="reaction">The reaction.</param>
        /// <param name="et60">The et60.</param>
        /// <param name="et330">The et330.</param>
        /// <param name="et594">The et594.</param>
        /// <param name="et660">The et660.</param>
        /// <param name="sp660">The SP660.</param>
        /// <param name="et934">The et934.</param>
        /// <param name="et1000">The et1000.</param>
        /// <param name="sp1000">The SP1000.</param>
        /// <param name="et1254">The et1254.</param>
        /// <param name="et1320">The et1320.</param>
        /// <param name="sp1320">The SP1320.</param>
        /// <param name="result">The result.</param>
        /// <param name="remarks">The remarks.</param>
        public Run(DateTime timestamp, string racenumber, int prefixId, string drivername,
            char lane, decimal index, decimal reaction, decimal et60, decimal et330, decimal et594,
            decimal et660, decimal sp660, decimal? et934, decimal et1000, decimal sp1000,
            decimal? et1254, decimal et1320, decimal sp1320, string result, string remarks)
        {
            Timestamp = timestamp;
            Racenumber = racenumber;
            PrefixId = prefixId;
            Drivername = drivername;
            Lane = lane;
            Index = index;
            Reaction = reaction;
            ET60 = et60;
            ET330 = et330;
            ET594 = et594;
            ET660 = et660;
            SP660 = sp660;
            ET934 = et934;
            ET1000 = et1000;
            SP1000 = sp1000;
            ET1254 = et1254;
            ET1320 = et1320;
            SP1320 = sp1320;
            Result = result;
            Remarks = remarks;
        }
    }
}