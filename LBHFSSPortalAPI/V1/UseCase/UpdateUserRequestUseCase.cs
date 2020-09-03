using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
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

        public UserResponse Execute(int currentUserId, UserUpdateRequest updateRequest)
        {
            var userDomain = _usersGateway.GetUser(currentUserId);

            userDomain.CreatedAt = updateRequest.CreatedAt;
            userDomain.Email = updateRequest.Email;
            userDomain.Id = updateRequest.Id;
            userDomain.Name = updateRequest.Name;
            userDomain.Status = updateRequest.Status;

            _usersGateway.UpdateUser(userDomain);

            var orgs = _usersGateway.GetAssociatedOrganisations(updateRequest.Id);
            var matchingOrg = orgs.SingleOrDefault(o => o.Id == updateRequest.OrganisationId);

            if (matchingOrg == null)
            {
                // add a link to this organisation Id
                _usersGateway.AssociateUserWithOrganisation(userDomain.Id, updateRequest.OrganisationId);
            }

            var response = new UserResponse()
            {
                CreatedAt = userDomain.CreatedAt,
                Email = userDomain.Email,
                Id = userDomain.Id,
                Name = userDomain.Name,
                Organisation = new OrganisationResponse()
                {
                    Id = matchingOrg.Id,
                    Name = matchingOrg.Name
                },
                Status = userDomain.Status
            };

            return response;
        }
    }
}
