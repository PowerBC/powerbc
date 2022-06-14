using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using powerbc.Domain;
using powerbc.Services;
using powerbc.Shares;

namespace powerbc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private ChatService _chatService;
        private JwtHelper _jwtHelper;

        public UserController(UserService userService, ChatService chatService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _chatService = chatService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("signup")]
        public IActionResult SignUp(
            [FromBody] UserRegistrationBody userRegistrationBody)
        {
            (int, string) verification = _userService.VerifyRegistration(userRegistrationBody);

            int code = verification.Item1;
            string message = verification.Item2;

            if (code == 409)
            {
                return Conflict(message);
            }
            else
            {
                _userService.CreateUser(userRegistrationBody);
                return Ok(message);
            }
        }

        [HttpPost("login")]
        public ActionResult Login(
            [FromBody] UserAuthenticationBody userAuthenticationBody)
        {
            UserService.VerifyLoginResult verification = _userService.VerifyLogin(userAuthenticationBody);

            int code = verification.code;
            string message = verification.message;
            if (code == 200)
            {
                return Ok(new Dictionary<string, string>()
                {
                    {"message", message},
                    {"token", _jwtHelper.GenerateToken(userAuthenticationBody.Email)},
                });
            }
            else
            {
                return Unauthorized(message);
            }
        }

        [Authorize]
        [HttpGet("name")]
        public ActionResult GetUsername()
        {
            User user = _userService.GetUserByEmail(User.Identity.Name);
            return Ok(user.Name);
        }

        [Authorize]
        [HttpGet("userId")]
        public ActionResult GetUserId()
        {
            User user = _userService.GetUserByEmail(User.Identity.Name);
            return Ok(user.Id);
        }

        [Authorize]
        [HttpPost("sendFriendRequest/{userId}")]
        public ActionResult SendFriendRequest(string userId)
        {
            User? friend = _userService.GetUserById(userId);
            User? user = _userService.GetUserByEmail(User.Identity.Name);

            if (friend == null)
                return BadRequest("此userId不存在");
            else if (user.IsInFriendList(friend))
                return BadRequest("此User已是你的朋友了");

            user.AddFriend(friend);
            friend.AddFriend(user);
            _chatService.AddChatRoom(new ChatRoom(user, friend));

            return Ok("成功加入好友");
        }


        public record FriendListInfo(string friendName, string chatRoomId);
        [Authorize]
        [HttpGet("friendList")]
        public ActionResult GetFriendList()
        {
            User? user = _userService.GetUserByEmail(User.Identity.Name);

            List<FriendListInfo> infos = new List<FriendListInfo>();

            foreach (User friend in user.friendList)
            {
                ChatRoom chatRoom = _chatService.GetChatRoomByUserPair(user, friend);
                infos.Add(new FriendListInfo(friend.Name, chatRoom.Id));
            }

            return Ok(infos);
        }

        [Authorize]
        [HttpGet("chat/{chatRoomId}")]
        public ActionResult GetMessagesOfChatRoom(string chatRoomId)
        {
            User? user = _userService.GetUserByEmail(User.Identity.Name);
            ChatRoom chatRoom = _chatService.GetChatRoomByChatRoomId(chatRoomId);

            return Ok(chatRoom.Info);
        }
    }
}