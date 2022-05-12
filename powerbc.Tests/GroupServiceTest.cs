using Microsoft.VisualStudio.TestTools.UnitTesting;
using powerbc.Domain;
using powerbc.Services;

namespace powerbc.Tests
{
    [TestClass]
    public class GroupServiceTest
    {
        [TestMethod]
        public void TestGetGroupListOfUser()
        {
            GroupService groupService = new GroupService();

            User creator = new("abc@xyz.com", "creator", "********");

            groupService.CreateGroup(creator, "Group_0", "");

            Assert.AreEqual(1, groupService.GetGroupListOfUser(creator).Count);
            Assert.AreEqual("Group_0", groupService.GetGroupListOfUser(creator)[0].Name);
        }

        [TestMethod]
        public void TestGetChannelListOfGroup()
        {
            GroupService groupService = new GroupService();

            User creator = new("abc@xyz.com", "creator", "********");

            groupService.CreateGroup(creator, "Group_0", "");

            string groupId = groupService.GetGroupListOfUser(creator)[0].Id;

            Assert.AreEqual(1, groupService.GetChannelListOfGroup(groupId).Count);
            Assert.AreEqual("General", groupService.GetChannelListOfGroup(groupId)[0].name);
        }

        [TestMethod]
        public void TestSendMessage()
        {
            GroupService groupService = new GroupService();

            User creator = new("abc@xyz.com", "creator", "********");

            groupService.CreateGroup(creator, "Group_0", "");

            string groupId = groupService.GetGroupListOfUser(creator)[0].Id;

            string channelId = groupService.GetChannelListOfGroup(groupId)[0].id;

            Message message = new(creator, "content");
            groupService.SendMessage(groupId, channelId, message);

            Assert.AreEqual(1, groupService.GetMessageListOfChannel(groupId, channelId).Count);
            Assert.AreEqual("content", groupService.GetMessageListOfChannel(groupId, channelId)[0].Content);
        }
    }
}
