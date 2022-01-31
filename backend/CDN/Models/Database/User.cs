namespace CDN.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] TokenHash { get; set; }
        public byte[] TokenSalt { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<CDNEntry> Entries { get; set; }
    }
}
