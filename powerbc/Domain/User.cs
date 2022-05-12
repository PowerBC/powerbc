namespace powerbc.Domain
{
    public class User
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public User(string email, string name, string password)
        {
            Email = email;
            Name = name;
            Password = password;
        }
    }

    public record UserInfo(string Id, string Name);
}
