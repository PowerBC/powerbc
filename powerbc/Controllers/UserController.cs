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
        private JwtHelper _jwtHelper;

        public UserController(UserService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
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
    }
}