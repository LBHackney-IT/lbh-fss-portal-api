using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.Factories;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class UsersGateway : IUsersGateway
    {
        private readonly DatabaseContext _databaseContext;

        public UsersGateway(DatabaseContext usersDatabaseContext)
        {
            _databaseContext = usersDatabaseContext;
        }

        public List<UserDomain> GetAllUsers()
        {
            var users = _databaseContext.Users.ToDomain();

            return users;
        }

        public UserDomain GetUser(string emailAddress)
        {
            UserDomain userDomain = null;

            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                if (_databaseContext != null)
                {
                    try
                    {
                        // Perform case sensitive search for user based on unique email address (used as username)
                        var user = _databaseContext.Users.Single(u => u.Email == emailAddress);
                        userDomain = user.ToDomain();
                    }
                    catch (System.Exception e)
                    {
                        // TODO catch specific EF exceptions as well as catch all
                        throw;
                    }
                }
                else
                {
                    // throw database connection error exception
                }
            }
            else
            {
                // throw invalid email exception
            }

            return userDomain;
        }
    }
}
