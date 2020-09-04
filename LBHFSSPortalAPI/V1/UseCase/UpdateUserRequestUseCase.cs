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
            var userDomain = _usersGateway.GetUser(userId);

            userDomain.CreatedAt = updateRequest.CreatedAt;
            userDomain.Email = updateRequest.Email;
            userDomain.Name = updateRequest.Name;
            userDomain.Status = updateRequest.Status;

            _usersGateway.UpdateUser(userDomain);

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
                        // TODO (MJC) - better error message below
                        UserErrorMessage = "Could not create an association from the user to the specified organisation"
                    };
                }
            }

            var response = new UserResponse()
            {
                Organisation = new OrganisationResponse()
                {
                    Id = associatedOrg.Id,
                    Name = associatedOrg.Name
                },
                CreatedAt = userDomain.CreatedAt,
                Email = userDomain.Email,
                Id = userDomain.Id,
                Name = userDomain.Name,
                Status = userDomain.Status,
                SubId = userDomain.SubId
            };

            return response;
        }
    }
}
