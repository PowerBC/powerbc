namespace powerbc.Domain
{
    public class User
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        private readonly List<User> _friendList = new();
        public List<User> friendList
        {
            get => _friendList;
        }

        public User(string email, string name, string password)
        {
            Email = email;
            Name = name;
            Password = password;
        }

        public bool IsInFriendList(User friend)
        {
            return _friendList.Contains(friend);
        }

        public void AddFriend(User friend)
        {
            _friendList.Add(friend);
        }
    }

    public record UserInfo(string Id, string Name);
}
