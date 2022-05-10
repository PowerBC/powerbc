﻿namespace powerbc.Domain
{
    public class Group
    {
        public string Id { get; init; }

        public string Name { get; set; }
        public string Description { get; set; } = "";
        
        private readonly List<User> _memberList = new();
        public List<User> MemberList
        {
            get => _memberList;
        }

        private readonly List<Channel> _channelList = new()
        {
            new Channel("0", "General"),
        };
        public List<Channel> ChannelList
        { 
            get => _channelList;
        }

        public GroupInfo Info
        {
            get => new(Id, Name, Description);
        }

        public Group(User creator, string id, string name, string desc)
        {
            Id = id;
            Name = name;
            Description = desc;
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

        public User GetMemberByEmail(string email)
        {
            return _memberList.First(m => m.Email == email);
        }

        public Channel GetChannelById(string channelId)
        {
            return _channelList.First(ch => ch.Id == channelId);
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
