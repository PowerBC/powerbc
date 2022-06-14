namespace powerbc.Domain
{
    public class ChatRoom
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public (User user1, User user2) UserPair { get; set; }

        public ChatRoom(User user1, User user2)
        {
            UserPair = (user1, user2);
        }

        private readonly List<Message> _messages = new();

        public List<Message> Messages { get => _messages; }

        public List<MessageInfo> Info { get { return _messages.Select(m => m.Info).ToList(); } }

        public void SaveMessage(Message message)
        {
            _messages.Add(message);
        }
    }
}
