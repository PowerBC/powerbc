using Microsoft.VisualStudio.TestTools.UnitTesting;
using powerbc.Domain;

namespace powerbc.Tests
{
    [TestClass]
    public class GroupTest
    {
        [TestMethod]
        public void TestGetMemberByEmail()
        {
            string email = "abc@xyz.com";
            User creator = new User("0", "abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "0", "Group0", "");

            Assert.AreEqual(creator, group.GetMemberByEmail(email));
        }

        [TestMethod]
        public void TestDefaultChannel()
        {
            User creator = new User("0", "abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "0", "Group0", "");

            Assert.AreEqual(1, group.ChannelList.Count);

            Channel defaultChannel = group.ChannelList[0];
            Assert.AreEqual("General", defaultChannel.Name);
        }

        [TestMethod]
        public void TestGetCreateChannel()
        {
            User creator = new User("0", "abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "0", "Group0", "");

            group.CreateChannel("Gaming");

            Assert.AreEqual(2, group.ChannelList.Count);
            Assert.IsTrue(group.ChannelList.Exists(ch => ch.Name == "Gaming"));
        }

        [TestMethod]
        public void TestSaveMessage_1()
        {
            User user = new("0", "abc@xyz.com", "sender", "********");
            
            Group group = new(user, "0", "Group", "");
            group.CreateChannel("Channel_0");

            string msgId = System.Guid.NewGuid().ToString();
            Message message = new(msgId, user, "content");

            group.SaveMessage(message, group.ChannelList[0].Id);


            Assert.AreEqual(message, group.ChannelList[0].MessageList[0]);
            Assert.AreEqual(msgId, group.ChannelList[0].MessageList[0].Id);
            Assert.AreEqual("content", group.ChannelList[0].MessageList[0].Content);
        }
    }
}