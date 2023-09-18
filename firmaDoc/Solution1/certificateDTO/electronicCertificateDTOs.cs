namespace certificateDTO
{
    public class electronicCertificateDTO
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string pfx { get; set; } = null!;
        public string cer { get; set; } = null!;
        public string pass { get; set; } = null!;
        public DateTime creationDate { get; set; }
        public DateTime expiratioDate { get; set; }
        public bool valid { get; set; }
    }

    public class readElectronicCertificateDTO
    {
        public int userId { get; set; }
        public string pfx { get; set; } = null!;
        public string cer { get; set; } = null!;
        public string pass { get; set; } = null!;
        public DateTime creationDate { get; set; }
        public DateTime expiratioDate { get; set; }
        public bool valid { get; set; }
    }

    public class newElectronicCertificateDTO
    {
        public string userName { get; set; }
        public int userId { get; set; }
        public string pass { get; set; } = null!;
        public int validDays { get; set; }
    }
}