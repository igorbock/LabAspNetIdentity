namespace ShieldJWTLib.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
