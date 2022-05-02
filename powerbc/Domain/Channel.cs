namespace powerbc.Domain
{
    public class Channel
    {
        public string Id { get; init; }
        public string Name { get; set; }

        private List<Message> _messageList = new();
        public List<Message> MessageList
        {
            get => _messageList;
        }

        public Channel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public void CreateMessage(User sender, string content)
        {
            string id = _messageList.Count.ToString();
            _messageList.Add(new Message(id, sender, content));
        }
    }
}
