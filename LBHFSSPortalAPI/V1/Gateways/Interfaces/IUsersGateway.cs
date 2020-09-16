using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public interface IUsersGateway
    {
        UserDomain GetUser(string emailAddress, string status);
        Task<List<UserDomain>> GetAllUsers(UserQueryParam userQueryParam);
        UserDomain AddUser(UserDomain user);
        UserDomain AddUser(AdminCreateUserRequest createRequestData);
        void UpdateUser(UserDomain user);
        UserDomain GetUserBySubId(string subId);
        UserDomain GetUser(int userId);
        IEnumerable<OrganizationsDomain> GetAssociatedOrganisations(int userId);
        OrganizationsDomain AssociateUserWithOrganisation(int userId, int organisationId);
    }
}
