using Microsoft.AspNetCore.Mvc;
using powerbc.Domain;
using powerbc.Services;
using powerbc.Shares;

namespace powerbc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private GroupService _groupService;
        private JwtHelper _jwtHelper;

        public GroupController(GroupService groupService, JwtHelper jwtHelper)
        {
            this._groupService = groupService;
            this._jwtHelper = jwtHelper;
        }

        [HttpPost("createGroup")]
        public ActionResult CreateGroup([FromBody] (int, string) w)
        {
            return Ok();
        }

        [HttpPost("createChannel")]
        public ActionResult CreateChannel([FromBody] (int, string) w)
        {
            return Ok();
        }

        [HttpPost("addMember")]
        public ActionResult AddMember([FromBody] (int, string) w)
        {
            return Ok();
        }

        [HttpPost("groupListOfUser")]
        public ActionResult GetGroupListOfUser([FromBody] (int, string) w)
        {
            return Ok();
        }

        [HttpPost("sendMessage")]
        public ActionResult SendMessage([FromBody] (int, string) w)
        {
            return Ok();
        }
    }
}