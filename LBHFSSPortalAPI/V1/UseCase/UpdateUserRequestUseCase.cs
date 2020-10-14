using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using LBHFSSPortalAPI.V1.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class UpdateUserRequestUseCase : IUpdateUserRequestUseCase
    {
        private readonly IUsersGateway _usersGateway;
        private readonly ISessionsGateway _sessionsGateway;
        private readonly IAuthenticateGateway _authenticateGateway;

        public UpdateUserRequestUseCase(IUsersGateway usersGateway,
                                        ISessionsGateway sessionsGateway,
                                        IAuthenticateGateway authenticateGateway)
        {
            _usersGateway = usersGateway;
            _sessionsGateway = sessionsGateway;
            _authenticateGateway = authenticateGateway;
        }

        public UserResponse Execute(int userId, UserUpdateRequest updateRequest)
        {
            ValidateRequestParams(updateRequest);
            var userDomain = _usersGateway.GetUserById(userId);

            // We do not store the password in the API database so cannot tell here
            // if a different value was provided compared to the current value.
            // Hence need to call the Update Password method on the authentication
            // gateway every time
            if (updateRequest.Password != null)
            {
                _authenticateGateway.AdminChangePassword(new ResetPasswordQueryParams()
                {
                    Email = userDomain.Email,
                    Password = updateRequest.Password
                });
            }

            // if the user has passed a null value for any of the below we assume
            // they do not want that field to be updated (keep the current value)
            userDomain.CreatedAt = updateRequest.CreatedAt ?? userDomain.CreatedAt;
            userDomain.Name = updateRequest.Name ?? userDomain.Name;
            userDomain.Status = updateRequest.Status ?? userDomain.Status;

            // Set up the user roles provided
            userDomain = PopulateDomainWithUserRoles(userDomain, updateRequest.Roles);
            _usersGateway.UpdateUserAndRoles(userDomain);

            var associatedOrg = _usersGateway.GetAssociatedOrganisation(userId);

            // If the client has specified a non-null organisation id, create or update the association
            // (note: db schema and EF code allows many-to-many associations between users and organisations
            // but the MVP version of the front end does not allow or expect this behaviour at the moment)
            if (updateRequest.OrganisationId.HasValue)
            {
                if (associatedOrg == null)
                {
                    // create a new association
                    associatedOrg = _usersGateway.AssociateUserWithOrganisation(userId, updateRequest.OrganisationId.Value);

                    if (associatedOrg == null)
                    {
                        throw new UseCaseException()
                        {
                            UserErrorMessage = "Could not create an association from the user to the specified organisation"
                        };
                    }
                }
                else
                {
                    associatedOrg = _usersGateway.AssociateUserWithOrganisation(userDomain.Id, updateRequest.OrganisationId.Value);
                }
            }
            else
            {
                // null value indicates the caller would like to clear the users' organisation association
                if (associatedOrg != null)
                {
                    _usersGateway.RemoveUserOrganisationAssociation(userId);
                    associatedOrg = null;
                }
            }

            // refresh the user domain object after above changes
            userDomain = _usersGateway.GetUserById(userDomain.Id);

            return userDomain.ToResponse();
        }

        private static UserDomain PopulateDomainWithUserRoles(UserDomain userDomain, List<string> roles)
        {
            if (roles != null)
            {
                // clear any current user roles
                userDomain.UserRoles.Clear();

                foreach (var role in roles)
                {
                    // add the new roles from the request
                    userDomain.UserRoles.Add(new UserRoleDomain()
                    {
                        User = userDomain,
                        Role = new RoleDomain { Name = role }
                    });
                }
            }

            return userDomain;
        }

        private static void ValidateRequestParams(UserUpdateRequest updateRequest)
        {
            const string emptyFieldMessage =
                "Send a (null) value for this if no change is required on this field";

            // The rather strange logic below is to account for the fact clients can send
            // a (null) value for a field in the request intending it to be ignored (keep
            // current value) but can also send a value if they wish it to be updated.

            if (updateRequest.Name != null && string.IsNullOrWhiteSpace(updateRequest.Name))
                throw new UseCaseException()
                {
                    UserErrorMessage = "The provided user name was empty",
                    DevErrorMessage = emptyFieldMessage
                };

            if (updateRequest.Password != null && string.IsNullOrWhiteSpace(updateRequest.Password))
                throw new UseCaseException()
                {
                    UserErrorMessage = "The provided user password was empty",
                    DevErrorMessage = emptyFieldMessage
                };

            if (updateRequest.Status != null && string.IsNullOrWhiteSpace(updateRequest.Status))
                throw new UseCaseException()
                {
                    UserErrorMessage = "The provided user status was empty",
                    DevErrorMessage = emptyFieldMessage
                };
        }
    }
}
