namespace ETSlipsMigrationTool.Models
{
    internal class Run
    {
        public DateTime Timestamp { get; set; }
        public string Racenumber { get; set; }
        public int PrefixId { get; set; }

        public string Drivername { get; set; }

        public char Lane { get; set; }

        public decimal Index { get; set; }
        public decimal Reaction { get; set; }
        public decimal ET60 { get; set; }
        public decimal ET330 { get; set; }
        public decimal ET594 { get; set; }

        public decimal ET660 { get; set; }
        public decimal SP660 { get; set; }
        public decimal? ET934 { get; set; }
        public decimal ET1000 { get; set; }
        public decimal SP1000 { get; set; }

        public decimal? ET1254 { get; set; }
        public decimal ET1320 { get; set; }
        public decimal SP1320 { get; set; }
        public string Result { get; set; }
        public string Remarks { get; set; }

        public Run(DateTime timestamp, string racenumber, int prefixId, string drivername,
            char lane, decimal index, decimal reaction, decimal eT60, decimal eT330, decimal eT594,
            decimal eT660, decimal sP660, decimal? eT934, decimal eT1000, decimal sP1000,
            decimal? eT1254, decimal eT1320, decimal sP1320, string result, string remarks)
        {
            Timestamp = timestamp;
            Racenumber = racenumber;
            PrefixId = prefixId;
            Drivername = drivername;
            Lane = lane;
            Index = index;
            Reaction = reaction;
            ET60 = eT60;
            ET330 = eT330;
            ET594 = eT594;
            ET660 = eT660;
            SP660 = sP660;
            ET934 = eT934;
            ET1000 = eT1000;
            SP1000 = sP1000;
            ET1254 = eT1254;
            ET1320 = eT1320;
            SP1320 = sP1320;
            Result = result;
            Remarks = remarks;
        }
    }
}