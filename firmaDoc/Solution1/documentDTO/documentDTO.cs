namespace documentDTO
{
    public class userDocumentDTO
    {
        public int id { get; set; }
        public string title { get; set; } = null!;
        public int userId { get; set; }
        public int? userFileId { get; set; }
        public DateTime? userFileStorageDate { get; set; }
        public int? signedUserFileId { get; set; }
        public DateTime? userFileSignDate { get; set; }
    }
    public class newUserDocumentDTO
    {
        public string title { get; set; } = null!;
        public int userId { get; set; }

        public bool sign { get; set; }
        public bool storeCipher { get; set; }

    }
}