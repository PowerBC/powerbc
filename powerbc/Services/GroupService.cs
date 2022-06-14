using powerbc.Domain;

namespace powerbc.Services
{
    public class GroupService
    {
        private readonly List<Group> _groupList = new();

        private readonly Dictionary<User, HashSet<Group>> _memberships = new();

        public void CreateGroup(User creator, string name, string desc)
        {
            Group newGroup = new (creator, name, desc);
            _groupList.Add(newGroup);
            UpdateMembership(creator, newGroup);
        }

        public List<GroupInfo> GetGroupListOfUser(User user)
        {
            try
            {
                return _memberships[user].Select(e => e.Info).ToList();
            }
            catch (KeyNotFoundException)
            {
                return new();
            }
        }

        public List<ChannelInfo> GetChannelListOfGroup(string groupId)
        {
            var group = GetGroupById(groupId);
            return group.ChannelList.Select(ch => ch.Info).ToList();
        }

        public Group? GetGroupById(string groupId)
        {
            return _groupList.Find(g => g.Id == groupId);
        }

        public Group? GetGroupByInvite(string inviteLink)
        {
            return _groupList.Find(g => g.Invite == inviteLink);
        }

        public List<Message> GetMessageListOfChannel(string groupId, string channelId)
        {
            Group group = GetGroupById(groupId);
            Channel channel = group.GetChannelById(channelId);
            return channel.MessageList;
        }

        public void AddMemberByInvite(User member, string inviteLink)
        {
            Group group = GetGroupByInvite(inviteLink);
            group.AddMember(member);
            UpdateMembership(member, group);
        }

        private void UpdateMembership(User member, Group group)
        {
            if (_memberships.ContainsKey(member))
            {
                _memberships[member].Add(group);
            }
            else
            {
                _memberships.Add(member, new() { group });
            }
        }

        public void SendMessage(
            string groupId,
            string channelId,
            Message message)
        {
            Group group = GetGroupById(groupId);
            group.SaveMessage(message, channelId);
        }
    }
}
