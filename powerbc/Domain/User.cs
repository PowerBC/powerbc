namespace powerbc.Domain
{
    public class User
    {
        public string Id { get; init; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }


        public User(string id, string email, string name, string password)
        {
            Id = id;
            Email = email;
            Name = name;
            Password = password;
        }

        
    }
}
