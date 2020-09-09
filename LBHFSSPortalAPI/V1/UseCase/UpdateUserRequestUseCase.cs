using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
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


            var userDomain = _usersGateway.GetUser(userId);
            _authenticateGateway.UpdateUser(updateRequest);

            // +++
            // TODO (MJC): Do I need to call ChangePassword separately as below or will the above
            // _authenticateGateway.UpdateUser() do this once implemented?

            //if (!string.IsNullOrEmpty(updateRequest.Password))
            //    _authenticateGateway.ChangePassword(new ResetPasswordQueryParams()
            //    {
            //        Email = userDomain.Email,
            //        Password = updateRequest.Password
            //    });

            // +++

            // if the user has passed a null value for any of the below we assume
            // they do not want that field to be updated (keep the current value)
            userDomain.CreatedAt = updateRequest.CreatedAt ?? userDomain.CreatedAt;
            userDomain.Name = updateRequest.Name ?? userDomain.Name;
            userDomain.Status = updateRequest.Status ?? userDomain.Status;
            _usersGateway.UpdateUser(userDomain);

            var response = new UserResponse();

            // If the client has specified a non-zero organisation id value, add the organisation to
            // the list of users associated organisations if not already present
            if (updateRequest.OrganisationId != 0)
            {
                var orgs = _usersGateway.GetAssociatedOrganisations(userId);
                var associatedOrg = orgs.SingleOrDefault(o => o.Id == updateRequest.OrganisationId);

                if (associatedOrg == null)
                {
                    // add a link to this organisation Id
                    associatedOrg = _usersGateway.AssociateUserWithOrganisation(userDomain.Id, updateRequest.OrganisationId);

                    if (associatedOrg == null)
                    {
                        throw new UseCaseException()
                        {
                            UserErrorMessage = "Could not create an association from the user to the specified organisation"
                        };
                    }
                }

                response.Organisation = new OrganisationResponse()
                {
                    Id = associatedOrg.Id,
                    Name = associatedOrg.Name
                };
            }

            response.CreatedAt = userDomain.CreatedAt;
            response.Email = userDomain.Email;
            response.Id = userDomain.Id;
            response.Name = userDomain.Name;
            response.Status = userDomain.Status;
            response.SubId = userDomain.SubId;

            return response;
        }

        private static void ValidateRequestParams(UserUpdateRequest updateRequest)
        {
            const string EmptyFieldMessage =
                "Send a(null) value for this if no change is required on this field";

            // The rather strange logic below is to account for the fact clients can send
            // a (null) value for a field in the request intending it to be ignored (keep
            // current value) but can also send a value if they wish it to be updated.
            //
            // TODO (MJC): (what if they want to empty one of the fields below such as roles?)

            if (updateRequest.Name != null && string.IsNullOrWhiteSpace(updateRequest.Name))
                throw new UseCaseException()
                {
                    UserErrorMessage = "The provided user name was empty",
                    DevErrorMessage = EmptyFieldMessage
                };

            if (updateRequest.Password != null && string.IsNullOrWhiteSpace(updateRequest.Password))
                throw new UseCaseException()
                {
                    UserErrorMessage = "The provided user password was empty",
                    DevErrorMessage = EmptyFieldMessage
                };

            if (updateRequest.Status != null && string.IsNullOrWhiteSpace(updateRequest.Status))
                throw new UseCaseException()
                {
                    UserErrorMessage = "The provided user status was empty",
                    DevErrorMessage = EmptyFieldMessage
                };

            if (updateRequest.Roles != null && string.IsNullOrWhiteSpace(updateRequest.Roles))
                throw new UseCaseException()
                {
                    UserErrorMessage = "The provided user roles list was empty",
                    DevErrorMessage = EmptyFieldMessage
                };
        }
    }
}
