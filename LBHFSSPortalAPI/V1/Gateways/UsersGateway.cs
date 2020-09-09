using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var userEntity = new User()
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

        public UserDomain GetUser(int userId)
        {
            UserDomain userDomain = null;

            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
                userDomain = user.ToDomain();

            return userDomain;
        }

        public IEnumerable<OrganizationsDomain> GetAssociatedOrganisations(int userId)
        {
            var orgDomains = new List<OrganizationsDomain>();

            try
            {
                var userOrgs = _context.UserOrganizations.Where(u => u.UserId == userId);

                if (userOrgs.Any())
                {
                    var orgIds = userOrgs.Select(o => o.OrganizationId);
                    var orgs = _context.Organizations
                        .Where(o => orgIds.Contains(o.Id))
                        .ToList();

                    orgDomains = orgs.ToDomain();
                }
            }
            catch (ArgumentNullException)
            {
                // catch?
                throw;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }

            return orgDomains;
        }

        public OrganizationsDomain AssociateUserWithOrganisation(int userId, int organisationId)
        {
            OrganizationsDomain response = null;

            try
            {
                // check organisation actually exists before creating association in database
                var orgEntity = _context.Organizations.FirstOrDefault(o => o.Id == organisationId);

                if (orgEntity == null)
                {
                    throw new UseCaseException()
                    {
                        UserErrorMessage = $"The supplied organisation ID '{organisationId}' was not found",
                        DevErrorMessage = $"The [UserOrganizations] table does not contain an organisation with ID = {organisationId}"
                    };
                }

                // check if the association already exists and ignore the request if it does
                var userOrg = _context.UserOrganizations.FirstOrDefault(u =>
                    u.UserId == userId &&
                    u.OrganizationId == organisationId);

                if (userOrg == null)
                {
                    // create new organisation <-> user association
                    userOrg = new UserOrganizations()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UserId = userId,
                        OrganizationId = organisationId
                    };

                    _context.UserOrganizations.Add(userOrg);
                    _context.SaveChanges();

                    response = orgEntity.ToDomain();
                }
            }
            catch (DbUpdateException e)
            {
                HandleDbUpdateException(e);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }

            return response;
        }
    }
}
