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
            Channel channel = new("Chat");
            
            User sender = new("abc@xyz.com", "sender", "********");
            
            Message message = new(sender, "content");
            
            channel.SaveMessage(message);

            Assert.AreEqual(1, channel.MessageList.Count);
            Assert.AreEqual("content", channel.MessageList[0].Content);
        }
    }
}
