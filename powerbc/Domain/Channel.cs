namespace powerbc.Domain
{
    public class Channel
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public ChannelInfo Info 
        {
            get => new(Id, Name);
        }

        private List<Message> _messageList = new();
        public List<Message> MessageList
        {
            get => _messageList;
        }

        public Channel(string name)
        {
            Name = name;
        }

        public void SaveMessage(Message message)
        {
            _messageList.Add(message);
        }
    }

    public record ChannelInfo(string id, string name);
}