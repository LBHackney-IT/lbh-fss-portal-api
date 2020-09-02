using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.Factories;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using System;
using Microsoft.EntityFrameworkCore;
using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class UsersGateway : BaseGateway, IUsersGateway
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
                    u.Status == userStatus);

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

        public UserDomain AddUser(UserDomain userDomain)
        {
            try
            {
                var userEntity = userDomain.ToEntity();
                _context.Users.Add(userEntity);
                _context.SaveChanges();
                userDomain = userEntity.ToDomain();
                return userDomain;
            }
            catch (DbUpdateException dbe)
            {
                HandleDbUpdateException(dbe);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }

            return null;
        }

        public void UpdateUser(UserDomain userDomain)
        {
            try
            {
                var userEntity = _context.Users.FirstOrDefault(u => u.Id == userDomain.Id);

                if (userEntity != null)
                {
                    userEntity.Email = userDomain.Email;
                    userEntity.Name = userDomain.Name;
                    userEntity.Status = userDomain.Status;
                    userEntity.CreatedAt = userDomain.CreatedAt;
                    userEntity.SubId = userDomain.SubId;
                    _context.SaveChanges();
                }
                else
                {
                    // user was not found
                }
            }
            catch (DbUpdateException dbe)
            {
                HandleDbUpdateException(dbe);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }

        public UserDomain AddUser(AdminCreateUserRequest requestData)
        {
            var userEntity = new Users()
            {
                Id = requestData.Id,
                CreatedAt = requestData.CreatedAt,
                Email = requestData.Email,
                Name = requestData.Name,
                Status = requestData.Status,
                SubId = requestData.Status,
            };

            try
            {
                _context.Users.Add(userEntity);
                _context.SaveChanges();
                var userDomain = userEntity.ToDomain();
                return userDomain;
            }
            catch (DbUpdateException dbe)
            {
                HandleDbUpdateException(dbe);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }

            return null;
        }
    }
}
