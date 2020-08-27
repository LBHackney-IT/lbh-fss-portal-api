using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.Factories;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using System;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class UsersGateway : IUsersGateway
    {
        private readonly DatabaseContext _context;

        public UsersGateway(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public List<UserDomain> GetAllUsers()
        {
            var users = _context.Users.ToDomain();

            return users;
        }

        public UserDomain GetUser(string emailAddress, string userStatus)
        {
            UserDomain userDomain = null;

            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                // Perform search for user based on email address and status
                var user = _context.Users.SingleOrDefault(u =>
                    u.Email == emailAddress &&
                    u.Status == userStatus );

                if (user != null)
                    userDomain = user.ToDomain();
            }
            else
            {
                // throw 'invalid email' gateway exception
            }

            return userDomain;
        }

        public UserDomain GetUserBySubId(string subId)
        {
            UserDomain userDomain = null;

            if (!string.IsNullOrWhiteSpace(subId))
            {
                // Perform search for user based on subscription ID
                var user = _context.Users.SingleOrDefault(u => u.SubId == subId);

                if (user != null)
                    userDomain = user.ToDomain();
            }
            else
            {
                // throw 'invalid email' gateway exception
            }

            return userDomain;
        }

        public void SaveUser(UserDomain user)
        {
            try
            {
                var userEntity = _context.Users.SingleOrDefault(u => u.Id == user.Id);

                if (userEntity == null)
                    userEntity = new Users();

                userEntity.SubId = user.SubId;
                userEntity.Email = user.Email;
                userEntity.Name = user.Name;
                userEntity.Status = user.Status;

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }
    }
}
