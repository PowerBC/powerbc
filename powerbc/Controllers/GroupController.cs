using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using powerbc.Domain;
using powerbc.Services;
using powerbc.Shares;
using powerbc.Hubs;

namespace powerbc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private UserService _userService;
        private GroupService _groupService;
        private readonly IHubContext<GroupServiceHub> _hubContext;

        public GroupController(
            GroupService groupService, 
            UserService userService,
            IHubContext<GroupServiceHub> hubContext)
        {
            _userService = userService;
            _groupService = groupService;
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpPost("createGroup")]
        public ActionResult CreateGroup([FromBody] GroupCreationBody body)
        {
            // 用Email取得User參考，因為建立Group建立需要指定creator
            User? creator = _userService.GetUserByEmail(User.Identity?.Name);
            string name = body.Name;
            string desc = body.Description;
            _groupService.CreateGroup(creator, name, desc);


            // 當系統新建立一個群組，由Server通知Client更新UI的Group清單
            _hubContext.Clients.All.SendAsync("UpdateGroupList");

            return Ok();
        }

        [Authorize]
        [HttpPost("createChannel")]
        public ActionResult CreateChannel([FromBody] (int, string) w)
        {
            return Ok();
        }

        [Authorize]
        [HttpPost("addMember")]
        public ActionResult AddMember([FromBody] (int, string) w)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("groupList")]
        public ActionResult GetGroupListOfUser()
        {
            User user = _userService.GetUserByEmail(User.Identity.Name);
            return Ok(_groupService.GetGroupListOfUser(user));
        }

        [Authorize]
        [HttpGet("channelList/{groupId}")]
        public ActionResult GetChannelList(string groupId)
        {
            return Ok(_groupService.GetChannelListOfGroup(groupId));
        }

        [Authorize]
        [HttpGet("messageList/{groupId}/{channelId}")]
        public ActionResult GetMessageList(string groupId, string channelId)
        {
            return Ok(_groupService.GetMessageListOfChannel(groupId, channelId));
        }

        public record SendMessageBody(string GroupId, string ChannelId, string Content);
        [Authorize]
        [HttpPost("sendMessage")]
        public ActionResult SendMessage([FromBody] SendMessageBody body)
        {
            string groupId = body.GroupId;
            string channelId = body.ChannelId;
            string content = body.Content;

            User sender = _userService.GetUserByEmail(User.Identity.Name);

            Message message = new(Guid.NewGuid().ToString() , sender, content);

            _groupService.SendMessage(groupId, channelId, message);

            Broadcast("ReceiveMessage");

            return Ok();
        }

        private void Broadcast(string method)
        {
            _hubContext.Clients.All.SendAsync(method);
        }

        [Authorize]
        [HttpGet("invite/{groupId}")]
        public ActionResult GetInvite(string groupId)
        {
            return Ok(_groupService.GetGroupById(groupId).Invite);
        }

        public record JoinGroupBody(string InviteLink);
        [Authorize]
        [HttpPost("joinGroupByInvite")]
        public ActionResult JoinGroupByInvite([FromBody] JoinGroupBody body)
        {
            string inviteLink = body.InviteLink;
            User member = _userService.GetUserByEmail(User.Identity.Name);
            _groupService.AddMemberByInvite(member, inviteLink);
            Broadcast("UpdateGroupList");
            return Ok();
        }
    }

    public record GroupCreationBody(string Name, string Description);
}