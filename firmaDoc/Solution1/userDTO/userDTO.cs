namespace usersDTO
{
    public class newUserDTO
    {
        public string nick { get; set; } = null!;
        public string email { get; set; } = null!;
    }

    public class userDTO
    {
        public int id { get; set; }
        public string nick { get; set; } = null!;
        public string email { get; set; } = null!;
    }

    public class userWithPermitDTO :userDTO
    {
        public List<permitDTO> permits { get; set; }
    }
}