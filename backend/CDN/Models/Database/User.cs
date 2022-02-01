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

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                User u = (User)obj;
                return Id == u.Id;
            }
        }
    }
}
