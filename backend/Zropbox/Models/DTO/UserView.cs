namespace Zropbox.Models
{
    public class UserView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        public UserView(User user)
        {
            Id = user.Id;
            Name = user.Name;
            IsAdmin = user.IsAdmin;
        }
    }
}
