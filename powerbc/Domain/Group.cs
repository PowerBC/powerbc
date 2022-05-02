namespace powerbc.Domain
{
    public class Group
    {
        public string Id { get; init; } = "";

        public string Name { get; set; } = "";
        
        private List<User> _memberList = new();
        public List<User> MemberList
        {
            get => _memberList;
        }

        private List<Channel> _channelList = new();
        public List<Channel> ChannelList
        { 
            get => _channelList;
        }

        public Group(User creator, string id, string name)
        {
            Id = id;
            Name = name;
            _memberList.Add(creator);
        }

        public void CreateChannel(string name)
        {
            string id = _channelList.Count.ToString();
            _channelList.Add(new Channel(id, name));
        }

        public void AddMember(User member)
        {
            _memberList.Add(member);
        }

    }
}
