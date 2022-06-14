using powerbc.Domain;

namespace powerbc.Services
{
    public class ChatService
    {
        private readonly List<ChatRoom> _chatRoomList = new();

        public ChatRoom GetChatRoomByUserPair(User user1, User user2)
        {
            return _chatRoomList.FirstOrDefault(room => (room.UserPair.user1 == user1 && room.UserPair.user2 == user2) || (room.UserPair.user1 == user2 && room.UserPair.user2 == user1));
        }
        
        public ChatRoom GetChatRoomByChatRoomId(string chatRoomId)
        {
            return _chatRoomList.FirstOrDefault(room => room.Id == chatRoomId);
        }

        public void AddChatRoom(ChatRoom chatRoom)
        {
            _chatRoomList.Add(chatRoom);
        }

        public void SendMessage(
            string chatRoomId,
            Message message)
        {
            ChatRoom chatRoom = GetChatRoomByChatRoomId(chatRoomId);
            chatRoom.SaveMessage(message);
        }
    }
}
