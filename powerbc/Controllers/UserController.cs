using Microsoft.AspNetCore.Mvc;
using powerbc.Domain;
using powerbc.Services;
using powerbc.Shares;

namespace powerbc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService userService;
        private JwtHelper jwtHelper;

        public UserController(UserService userService, JwtHelper jwtHelper)
        {
            this.userService = userService;
            this.jwtHelper = jwtHelper;
        }

        [HttpPost("signup")]
        public IActionResult SignUp(
            [FromBody] UserRegistrationBody userRegistrationBody)
        {
            (int, string) verification = userService.VerifyRegistration(userRegistrationBody);

            int code = verification.Item1;
            string message = verification.Item2;

            if (code == 409)
            {
                return Conflict(message);
            }
            else
            {
                userService.CreateUser(userRegistrationBody);
                return Ok(message);
            }
        }

        [HttpPost("login")]
        public ActionResult Login(
            [FromBody] UserAuthenticationBody userAuthenticationBody)
        {
            (int, string) verification = userService.VerifyLogin(userAuthenticationBody);



            int code = verification.Item1;
            string message = verification.Item2;
            if (code == 200)
            {
                return Ok(new Dictionary<string, string>()
                {
                    {"message", message},
                    {"token", jwtHelper.GenerateToken("123")},
                });
            }
            else
            {
                return Unauthorized(message);
            }
        }
    }
}