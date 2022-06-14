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
            User creator = new User("abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");

            Assert.AreEqual(creator, group.GetMemberByEmail(email));
        }

        [TestMethod]
        public void TestGetMemberByEmail_NotFound()
        {
            string email = "abc@xyz.com";
            User creator = new User(email, "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");

            Assert.AreEqual(null, group.GetMemberByEmail("abc@xyz.coM"));
            Assert.AreNotEqual(creator, group.GetMemberByEmail("abc@xyz.coM"));
        }

        [TestMethod]
        public void TestDefaultChannel()
        {
            User creator = new User("abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");

            Assert.AreEqual(1, group.ChannelList.Count);

            Channel defaultChannel = group.ChannelList[0];
            Assert.AreEqual("General", defaultChannel.Name);
        }

        [TestMethod]
        public void TestGetCreateChannel()
        {
            User creator = new User("abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");

            group.CreateChannel("Gaming");

            Assert.AreEqual(2, group.ChannelList.Count);
            Assert.IsTrue(group.ChannelList.Exists(ch => ch.Name == "Gaming"));
        }

        [TestMethod]
        public void TestSaveMessage()
        {
            User user = new("abc@xyz.com", "sender", "********");
            
            Group group = new(user, "Group", "");
            group.CreateChannel("Channel_0");

            Message message = new(user, "content");

            group.SaveMessage(message, group.ChannelList[0].Id);


            Assert.AreEqual(message, group.ChannelList[0].MessageList[0]);
            Assert.AreEqual("content", group.ChannelList[0].MessageList[0].Content);
        }

        [TestMethod]
        public void TestGetChannelById_NotFound()
        {
            User creator = new User("abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");
            group.CreateChannel("Channel01");
            
            Assert.AreEqual(null, group.GetChannelById("123"));
            Assert.AreEqual(null, group.GetChannelById(""));
            Assert.AreEqual(null, group.GetChannelById("ds/df,gfd"));
        }

        [TestMethod]
        public void TestGetChannelById_Found()
        {
            User creator = new User("abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");
            group.CreateChannel("Channel01");

            string channelId = group.ChannelList[0].Id;

            Assert.AreNotEqual(null, group.GetChannelById(channelId));
        }

        [TestMethod]
        public void TestAddMember()
        {
            User creator = new User("abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");
            group.CreateChannel("Channel01");

            Assert.AreEqual(1, group.MemberList.Count);

            group.AddMember(new User("poi@xyz.com", "Joe", "*******"));

            Assert.AreEqual(2, group.MemberList.Count);
        }

        [TestMethod]
        public void TestCreateChannel()
        {
            User creator = new User("abc@xyz.com", "Jeff", "*******");
            Group group = new Group(creator, "Group0", "");

            Assert.AreEqual(1, group.ChannelList.Count);

            group.CreateChannel("Channel0");

            Assert.AreEqual(2, group.ChannelList.Count);
        }


    }
}