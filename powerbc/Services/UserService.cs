﻿using powerbc.Domain;

namespace powerbc.Services
{
    public class UserService
    {
        private List<User> userList = new();

        public void CreateUser(
            UserRegistrationBody userRegistrationBody)
        {
            string id = userList.Count.ToString();
            string email = userRegistrationBody.Email;
            string name = userRegistrationBody.Name;
            string password = userRegistrationBody.Password;

            userList.Add(new User(id, email, name, password));

            Console.WriteLine("[CreateUser]");
            Console.WriteLine("Id\tEmail\tName\t");
            foreach (var user in userList)
            {
                Console.WriteLine($"{user.Id}\t{user.Email}\t{user.Name}");
            }
            Console.WriteLine("------------");

        }

        public (int, string) VerifyRegistration(
            UserRegistrationBody userRegistrationBody)
        {
            int code;
            string message;
            if (IsEmailRegistered(userRegistrationBody.Email))
            {
                code = 409;
                message = "This email is already registered.";
                return (code, message);
            }
            else
            {
                code = 200;
                message = "Success.";
                return (code, message);
            }

        }

        public (int, string) VerifyLogin(
            UserAuthenticationBody userAuthenticationBody)
        {
            int code;
            string message;
            if (IsAuthenticationCorrect(userAuthenticationBody))
            {
                code = 200;
                message = "Success.";
            }
            else
            {
                code = 401;
                message = "The email or the password is incorrect.";
            }

            return (code, message);
        }

        private User? GetUserByEmail(string email)
        {
            return userList.Find(user => user.Email == email);
        }

        private bool IsEmailRegistered(string email)
        {
            
            return GetUserByEmail(email) != null;
        } 

        private bool IsAuthenticationCorrect(
            UserAuthenticationBody userAuthenticationBody)
        {
            var email = userAuthenticationBody.Email;
            var password = userAuthenticationBody.Password;

            var user = GetUserByEmail(email);
            if  (user != null)
            {
                return user.Password == password;
            }
            else
            {
                return false;
            }
        }
    }
}