using Microsoft.VisualStudio.TestTools.UnitTesting;
using powerbc.Domain;

namespace powerbc.Tests
{
    [TestClass]
    public class ChannelTest
    {
        [TestMethod]
        public void TestSaveMessage()
        {
            Channel channel = new("0", "Chat");
            
            User sender = new("0", "abc@xyz.com", "sender", "********");
            
            Message message = new(System.Guid.NewGuid().ToString(), sender, "content");
            
            channel.SaveMessage(message);

            Assert.AreEqual(1, channel.MessageList.Count);
            Assert.AreEqual("content", channel.MessageList[0].Content);
        }
    }
}
