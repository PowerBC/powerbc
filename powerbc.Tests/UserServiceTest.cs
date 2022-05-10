using Microsoft.VisualStudio.TestTools.UnitTesting;
using powerbc.Domain;
using powerbc.Services;

namespace powerbc.Tests
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void TestCreateUser()
        {
            UserService userService = new();
            userService.CreateUser(
                new() { Email = "abc@xyz.com", Name = "User", Password = "*****" });

            User user = userService.GetUserByEmail("abc@xyz.com");
            Assert.IsNotNull(user);
            Assert.AreEqual("abc@xyz.com", user.Email);
            Assert.AreNotEqual("user", user.Name);
            Assert.AreNotEqual("1234", user.Name);
        }

        [TestMethod]
        public void TestVerifyRegistration()
        {
            UserRegistrationBody body = new()
            {
                Name = "Joe", Email = "joe@abc.co.jp", Password = "*******"
            };

            UserService userService = new();

            Assert.AreEqual((200, "Success."), userService.VerifyRegistration(body));

            userService.CreateUser(body);

            Assert.AreEqual(
                (409, "This email is already registered."), 
                userService.VerifyRegistration(body));
        }

        [TestMethod]
        public void TestVerifyLogin_1()
        {
            UserRegistrationBody body = new()
            {
                Name = "Joe",
                Email = "joe@abc.co.jp",
                Password = "*******"
            };

            UserService userService = new();
            userService.CreateUser(body);

            UserAuthenticationBody authBody1 = new()
            {
                Email = "Joe@abc.co.jp", Password = "*******"
            };

            Assert.AreEqual(
                (401, "The email or the password is incorrect."),
                userService.VerifyLogin(authBody1));
        }

        [TestMethod]
        public void TestVerifyLogin_2()
        {
            UserRegistrationBody body = new()
            {
                Name = "Joe",
                Email = "joe@abc.co.jp",
                Password = "*******"
            };

            UserService userService = new();
            userService.CreateUser(body);
            UserAuthenticationBody authBody2 = new()
            {
                Email = "joe@abc.co.jp",
                Password = "*********"
            };

            Assert.AreEqual(
                (401, "The email or the password is incorrect."),
                userService.VerifyLogin(authBody2));
        }

        [TestMethod]
        public void TestVerifyLogin_3()
        {
            UserRegistrationBody body = new()
            {
                Name = "Joe",
                Email = "joe@abc.co.jp",
                Password = "*******"
            };

            UserService userService = new();
            userService.CreateUser(body);
            UserAuthenticationBody authBody = new()
            {
                Email = "joe@abc.co.jp",
                Password = "*******"
            };

            Assert.AreEqual(
                (200, "Success."),
                userService.VerifyLogin(authBody));
        }
    }
}
