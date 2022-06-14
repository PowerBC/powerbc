namespace powerbc.Domain
{
    public class Group
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        public string Description { get; set; } = "";
        
        private readonly List<User> _memberList = new();
        public List<User> MemberList
        {
            get => _memberList;
        }

        private readonly List<Channel> _channelList = new()
        {
            new Channel("General"),
        };
        public List<Channel> ChannelList
        { 
            get => _channelList;
        }

        public string Invite = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("==", "");

        public GroupInfo Info
        {
            get => new(Id, Name, Description);
        }

        public Group(User creator, string name, string desc)
        {
            Name = name;
            Description = desc;
            _memberList.Add(creator);
        }

        public void CreateChannel(string name)
        {
            _channelList.Add(new Channel(name));
        }

        public void AddMember(User member)
        {
            _memberList.Add(member);
        }

        public User? GetMemberByEmail(string email)
        {
            return _memberList.Find(m => m.Email == email);
        }

        public Channel? GetChannelById(string channelId)
        {
            return _channelList.Find(ch => ch.Id == channelId);
        }

        public void SaveMessage(Message message, string channelId)
        {
            Channel channel = GetChannelById(channelId);
            channel.SaveMessage(message);
        }
    }

    public record GroupInfo(
        string Id,
        string Name,
        string Description
    );
}
