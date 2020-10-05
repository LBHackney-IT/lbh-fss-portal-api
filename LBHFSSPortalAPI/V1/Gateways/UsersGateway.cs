using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.Validations;
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
                    .Include(u => u.UserOrganisations)
                    .ThenInclude(uo => uo.Organisation)
                    .Include(uo => uo.Organisations)
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
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

        public UserDomain GetUserByEmail(string emailAddress, string userStatus)
        {
            UserDomain userDomain = null;

            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new UseCaseException { UserErrorMessage = "Invalid user email address supplied" };

            // Perform search for user based on email address and status
            var user = Context.Users
                .Include(u => u.UserOrganisations)
                .ThenInclude(uo => uo.Organisation)
                .Include(uo => uo.Organisations)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
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
                .Include(u => u.UserOrganisations)
                .ThenInclude(uo => uo.Organisation)
                .Include(uo => uo.Organisations)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .SingleOrDefault(u => u.SubId == subId);

            if (user != null)
                userDomain = _mapper.ToDomain(user);

            return userDomain;
        }

        public UserDomain GetUserById(int userId)
        {
            var user = Context.Users
                .Include(u => u.UserOrganisations)
                .ThenInclude(uo => uo.Organisation)
                .Include(uo => uo.Organisations)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .SingleOrDefault(u => u.Id == userId);

            if (user != null)
                return _mapper.ToDomain(user);

            return null;
        }

        public async Task<UserDomain> GetUserByIdAsync(int userId)
        {
            var user = await Context.Users
                .Include(u => u.UserOrganisations)
                .ThenInclude(uo => uo.Organisation)
                .Include(uo => uo.Organisations)
                .Include(u => u.UserRoles)
                .ThenInclude(uo => uo.Role)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == userId)
                .ConfigureAwait(false);

            if (user != null)
                return _mapper.ToDomain(user);

            return null;
        }

        /// <summary>
        /// Returns the list of role names associated to the user with the given user ID
        /// </summary>
        public List<string> GetUserRoleList(int userId)
        {
            var roleNames = Context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(r => r.Role)
                .Where(r => r != null)
                .Select(r => r.Name)
                .ToList();

            return roleNames;
        }

        public UserDomain AddUser(AdminCreateUserRequest requestData, string subId)
        {
            // POST /users

            UserDomain userDomain = null;

            var userEntity = new User()
            {
                CreatedAt = requestData.CreatedAt.HasValue
                    ? requestData.CreatedAt
                    : DateTime.UtcNow,
                Email = requestData.Email,
                Name = requestData.Name,
                Status = requestData.Status,
                SubId = subId
            };

            try
            {
                // add the user
                Context.Users.Add(userEntity);
                Context.SaveChanges();

                if (requestData.OrganisationId.HasValue)
                {
                    AssociateUserWithOrganisation(userEntity.Id, requestData.OrganisationId.Value);
                }

                if (requestData.Roles != null)
                {
                    var validatedRoles = UserRoleValidator.ToValidList(requestData.Roles);
                    AddRolesToUser(userEntity.Id, validatedRoles);
                }

                // refresh the user domain object
                userDomain = GetUserById(userEntity.Id);
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

            return userDomain;
        }

        public UserDomain AddUser(UserDomain userDomain)
        {
            // POST /registration

            // Adds basic user record for registration process

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
            // PATCH /users

            try
            {
                var userEntity = Context.Users.FirstOrDefault(u => u.Id == userDomain.Id);

                if (userEntity == null)
                    throw new UseCaseException { UserErrorMessage = $"User with ID '{userDomain.Id}' could not be found" };

                userEntity.Email = userDomain.Email;
                userEntity.Name = userDomain.Name;
                userEntity.Status = userDomain.Status;
                userEntity.CreatedAt = userDomain.CreatedAt;
                userEntity.SubId = userDomain.SubId;
                Context.SaveChanges();

                // add any role associations this user does not already have
                if (userDomain.UserRoles != null)
                {
                    // Since we are updating the user, clear any current role associations even if the request
                    // contains an empty list of roles. The behaviour is to clear the associated roles if an empty
                    // list of roles is provided
                    ClearUserRoles(userEntity.Id);

                    var rolesInDomain = GetRoleNamesFromUserDomain(userDomain);
                    if (rolesInDomain != null)
                    {
                        // add any valid new roles
                        var validRoles = UserRoleValidator.ToValidList(rolesInDomain);

                        if (validRoles.Any())
                            AddRolesToUser(userEntity.Id, validRoles);
                    }
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

        public OrganisationDomain GetAssociatedOrganisation(int userId)
        {
            // Users and Organisations have a many to many relationship and use the UserOrganisation
            // link entity to resolve this. But for the MVP, callers will only ever associate
            // one organisation with one user

            var userOrg = Context.UserOrganisations
                .Include(uo => uo.Organisation)
                .FirstOrDefault(uo => uo.UserId == userId);

            if (userOrg?.Organisation != null)
                return _mapper.ToDomain(userOrg.Organisation);

            return null;
        }

        public OrganisationDomain AssociateUserWithOrganisation(int userId, int organisationId)
        {
            OrganisationDomain response = null;

            try
            {
                // check organisation actually exists before creating association in database
                var orgEntity = Context.Organisations.FirstOrDefault(o => o.Id == organisationId);

                if (orgEntity == null)
                {
                    throw new UseCaseException()
                    {
                        UserErrorMessage = $"The supplied organisation ID '{organisationId}' was not found",
                        DevErrorMessage = $"The [organisations] table does not contain an organisation with ID = {organisationId}"
                    };
                }

                var userOrg = Context.UserOrganisations.FirstOrDefault(u => u.UserId == userId);

                // check if an association already exists and modify this one if it does
                if (userOrg != null)
                {
                    userOrg.OrganisationId = organisationId;
                    Context.UserOrganisations.Update(userOrg);
                }
                else
                {
                    // create new organisation <-> user association
                    userOrg = new UserOrganisation()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UserId = userId,
                        OrganisationId = organisationId
                    };
                    Context.UserOrganisations.Add(userOrg);
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
            var userOrg = Context.UserOrganisations.FirstOrDefault(u => u.UserId == userId);

            if (userOrg != null)
            {
                Context.Remove(userOrg);
                Context.SaveChanges();
            }
        }

        private void ClearUserRoles(int userId)
        {
            try
            {
                var userRoles = Context.UserRoles.Where(ur => ur.UserId == userId);
                Context.UserRoles.RemoveRange(userRoles);
                Context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                HandleDbUpdateException(e);
            }
        }

        private static List<string> GetRoleNamesFromUserDomain(UserDomain userDomain)
        {
            var roleDomains = userDomain?.UserRoles
                .Select(ur => ur.Role)
                .Where(ur => ur != null)
                .Select(r => r.Name)
                .ToList();

            return roleDomains;
        }

        private void AddRolesToUser(int userId, List<string> roles)
        {
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    // check if the role exists and add if not create a new role entity
                    var roleEntity = Context.Roles.FirstOrDefault(r => r.Name == role);

                    if (roleEntity == null)
                        roleEntity = new Role { CreatedAt = DateTime.UtcNow, Name = role };

                    Context.UserRoles.Add(new UserRole()
                    {
                        CreatedAt = DateTime.UtcNow,
                        Role = roleEntity,
                        UserId = userId
                    });

                    try
                    {
                        Context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        HandleDbUpdateException(e);
                    }
                }
            }
        }
    }
}
