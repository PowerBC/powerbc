using Microsoft.AspNetCore.SignalR;
using powerbc.Domain;
using powerbc.Hubs;

namespace powerbc.Services
{
    public class GroupService
    {
        private List<Group> _groupList = new();

        private Dictionary<User, HashSet<Group>> _memberships = new();

        public void CreateGroup(User creator, string name, string desc)
        {
            string id = _groupList.Count.ToString();
            Group newGroup = new (creator, id, name, desc);
            _groupList.Add(newGroup);
            if (_memberships.ContainsKey(creator))
            {
                _memberships[creator].Add(newGroup);
            }
            else
            {
                _memberships.Add(creator, new() { newGroup });
            }

            // for debug
            Console.WriteLine("[CreateGroup]");
            Console.WriteLine("Id\tGroup\tDescription");
            foreach (var group in _groupList)
            {
                Console.WriteLine($"{group.Id}\t{group.Name}\t{group.Description}");
            }
            Console.WriteLine("------------");


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
            var group = _groupList.First(g => groupId.Equals(groupId));
            return group.ChannelList.Select(ch => ch.Info).ToList();
        }

        public Group GetGroupById(string groupId)
        {
            return _groupList.First(g => g.Id == groupId);
        }

        public List<Message> GetMessageListOfChannel(string groupId, string channelId)
        {
            Group group = GetGroupById(groupId);
            Channel channel = group.GetChannelById(channelId);
            return channel.MessageList;
        }

        public void SendMessage(
            string email, 
            string groupId,  
            string channelId, 
            string message, 
            IHubContext<GroupServiceHub> hubContext)
        {
            Group group = GetGroupById(groupId);
            User sender = group.GetMemberByEmail(email);
            group.SendMessage(sender, channelId, message, hubContext);
        }
    }
}
