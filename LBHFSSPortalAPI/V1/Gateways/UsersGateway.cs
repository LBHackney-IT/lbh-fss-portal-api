using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class UsersGateway : BaseGateway, IUsersGateway
    {
        private readonly MappingHelper _mapper;

        public UsersGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }

        public async Task<List<UserDomain>> GetAllUsers(UserQueryParam userQueryParam)
        {
            // Search       search term to use (searches on [name] column for the MVP)
            // Sort         the column name by which to sort
            // Direction    sort order; asc, desc
            // Limit        maximum number of records to return
            // Offset       number of records to skip for pagination

            List<UserDomain> response = new List<UserDomain>();
            var direction = ConvertToEnum(userQueryParam.Direction);

            if (direction == SortDirection.None)
                throw new UseCaseException()
                {
                    UserErrorMessage = "The sort direction was not valid (must be one of asc, desc)"
                };

            var matchingUsers = Context.Users.AsQueryable();

            // handle search 
            if (!string.IsNullOrWhiteSpace(userQueryParam.Search))
                matchingUsers = matchingUsers.Where(u => EF.Functions.Like(u.Name, $"%{userQueryParam.Search}%"));

            // handle sort by column name and sort direction
            var entityPropName = GetEntityPropertyForColumnName(typeof(User), userQueryParam.Sort);

            if (entityPropName == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"The 'Sort' parameter contained the value '{userQueryParam.Sort}' " +
                                        "which is not a valid column name"
                };

            matchingUsers = (direction == SortDirection.Asc) ?
                matchingUsers.OrderBy(u => EF.Property<User>(u, entityPropName)) :
                matchingUsers.OrderByDescending(u => EF.Property<User>(u, entityPropName));

            // handle pagination options
            if (userQueryParam.Limit.HasValue)
                matchingUsers = matchingUsers.Take(userQueryParam.Limit.Value);

            if (userQueryParam.Offset.HasValue)
                matchingUsers = matchingUsers.Skip(userQueryParam.Offset.Value);

            try
            {
                var userList = await matchingUsers
                    .Include(u => u.UserOrganizations)
                    .ThenInclude(uo => uo.Organization)
                    .Include(uo => uo.Organizations)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                response = _mapper.ToDomain(userList);
            }
            catch (InvalidOperationException e)
            {
                throw new UseCaseException()
                {
                    UserErrorMessage = "Could not run the user search query with the supplied input parameters",
                    DevErrorMessage = e.Message
                };
            }

            return response;
        }

        public UserDomain GetUser(string emailAddress, string userStatus)
        {
            UserDomain userDomain = null;

            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new UseCaseException { UserErrorMessage = "Invalid user email address supplied" };

            // Perform search for user based on email address and status
            var user = Context.Users
                .Include(u => u.UserOrganizations)
                .ThenInclude(uo => uo.Organization)
                .Include(uo => uo.Organizations)
                .AsNoTracking()
                .SingleOrDefault(u =>
                    u.Email == emailAddress &&
                    u.Status == userStatus);

            if (user != null)
                userDomain = _mapper.ToDomain(user);

            return userDomain;
        }

        public UserDomain GetUserBySubId(string subId)
        {
            UserDomain userDomain = null;

            if (string.IsNullOrWhiteSpace(subId))
                throw new UseCaseException { UserErrorMessage = "Invalid sub_id value supplied" };

            // Perform search for user based on subscription ID
            var user = Context.Users
                .Include(u => u.UserOrganizations)
                .ThenInclude(uo => uo.Organization)
                .Include(uo => uo.Organizations)
                .AsNoTracking()
                .SingleOrDefault(u => u.SubId == subId);

            if (user != null)
                userDomain = _mapper.ToDomain(user);

            return userDomain;
        }

        public UserDomain AddUser(UserDomain userDomain)
        {
            try
            {
                var userEntity = userDomain.ToEntity();
                Context.Users.Add(userEntity);
                Context.SaveChanges();
                userDomain = _mapper.ToDomain(userEntity);
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
                var userEntity = Context.Users.FirstOrDefault(u => u.Id == userDomain.Id);

                if (userEntity != null)
                {
                    userEntity.Email = userDomain.Email;
                    userEntity.Name = userDomain.Name;
                    userEntity.Status = userDomain.Status;
                    userEntity.CreatedAt = userDomain.CreatedAt;
                    userEntity.SubId = userDomain.SubId;
                    Context.SaveChanges();
                }
                else
                {
                    throw new UseCaseException { UserErrorMessage = $"User with ID '{userDomain.Id}' could not be found" };
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

        public UserDomain AddUser(AdminCreateUserRequest requestData, string subId)
        {
            UserDomain userDomain = null;

            var userEntity = new User()
            {
                CreatedAt = requestData.CreatedAt,
                Email = requestData.Email,
                Name = requestData.Name,
                Status = requestData.Status,
                SubId = subId
            };

            try
            {
                Context.Users.Add(userEntity);
                Context.SaveChanges();
                userDomain = _mapper.ToDomain(userEntity);
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

            if (requestData.OrganisationId.HasValue)
            {
                // Perform the association and refresh the user entity organisation details
                AssociateUserWithOrganisation(userEntity.Id, requestData.OrganisationId.Value);
                userDomain = GetUser(userEntity.Id);
            }

            return userDomain;
        }

        public UserDomain GetUser(int userId)
        {
            UserDomain userDomain = null;

            var user = Context.Users
                .Include(u => u.UserOrganizations)
                .ThenInclude(uo => uo.Organization)
                .Include(uo => uo.Organizations)
                .AsNoTracking()
                .SingleOrDefault(u => u.Id == userId);

            if (user != null)
                userDomain = _mapper.ToDomain(user);

            return userDomain;
        }

        public async Task<UserDomain> GetUserAsync(int userId)
        {
            var user = await Context.Users
                .Include(u => u.UserOrganizations)
                .ThenInclude(uo => uo.Organization)
                .Include(uo => uo.Organizations)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == userId)
                .ConfigureAwait(false);

            if (user != null)
                return _mapper.ToDomain(user);

            return null;
        }

        public OrganizationDomain GetAssociatedOrganisation(int userId)
        {
            // Users and Organisations have a many to many relationship and use the UserOrganization
            // link entity to resolve this. But for the MVP, callers will only ever associate
            // one organisation with one user

            var userOrg = Context.UserOrganizations
                .Include(uo => uo.Organization)
                .FirstOrDefault(uo => uo.UserId == userId);

            if (userOrg?.Organization != null)
                return _mapper.ToDomain(userOrg.Organization);

            return null;
        }

        public OrganizationDomain AssociateUserWithOrganisation(int userId, int organisationId)
        {
            OrganizationDomain response = null;

            try
            {
                // check organisation actually exists before creating association in database
                var orgEntity = Context.Organizations.FirstOrDefault(o => o.Id == organisationId);

                if (orgEntity == null)
                {
                    throw new UseCaseException()
                    {
                        UserErrorMessage = $"The supplied organisation ID '{organisationId}' was not found",
                        DevErrorMessage = $"The [Organizations] table does not contain an organisation with ID = {organisationId}"
                    };
                }

                var userOrg = Context.UserOrganizations.FirstOrDefault(u => u.UserId == userId);

                // check if an association already exists and modify this one if it does
                if (userOrg != null)
                {
                    userOrg.OrganizationId = organisationId;
                    Context.UserOrganizations.Update(userOrg);
                }
                else
                {
                    // create new organisation <-> user association
                    userOrg = new UserOrganization()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UserId = userId,
                        OrganizationId = organisationId
                    };
                    Context.UserOrganizations.Add(userOrg);
                }

                Context.SaveChanges();
                response = _mapper.ToDomain(orgEntity);
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

        public void RemoveUserOrganisationAssociation(int userId)
        {
            var userOrg = Context.UserOrganizations.FirstOrDefault(u => u.UserId == userId);

            if (userOrg != null)
            {
                Context.Remove(userOrg);
                Context.SaveChanges();
            }
        }
    }
}
