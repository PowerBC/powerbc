using powerbc.Domain;
using System.Collections.Generic;

namespace powerbc.Services
{
    public class GroupService
    {
        private List<Group> _groupList = new();

        private Dictionary<User, HashSet<Group>> _memberships = new();

        public void CreateGroup(User creator, string name)
        {
            string id = _groupList.Count.ToString();
            
            Group group = new (creator, id, name);
            group.AddMember(creator);
        }

        public void SendMessage()
        {

        }
    }
}
