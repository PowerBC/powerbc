using Microsoft.AspNetCore.SignalR;
using powerbc.Hubs;

namespace powerbc.Domain
{
    public class Channel
    {
        public string Id { get; init; }
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

        public Channel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public void CreateMessage(User sender, string content, IHubContext<GroupServiceHub> hubContext)
        {
            string id = _messageList.Count.ToString();
            Message message = new Message(id, sender, content);
            _messageList.Add(message);

            // Boardcast
            hubContext.Clients.All.SendAsync("ReceiveMessage");
        }
    }

    public record ChannelInfo(string id, string name);
}
