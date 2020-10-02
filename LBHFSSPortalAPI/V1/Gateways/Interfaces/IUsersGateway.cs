using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IUsersGateway
    {
        UserDomain GetUserByEmail(string emailAddress, string status);
        Task<List<UserDomain>> GetAllUsers(UserQueryParam userQueryParam);
        UserDomain AddUser(UserDomain user);
        UserDomain AddUser(AdminCreateUserRequest createRequestData, string subId);
        void UpdateUser(UserDomain user);
        UserDomain GetUserBySubId(string subId);
        UserDomain GetUserById(int userId);
        Task<UserDomain> GetUserByIdAsync(int userId);
        OrganizationDomain GetAssociatedOrganisation(int userId);
        OrganizationDomain AssociateUserWithOrganisation(int userId, int organisationId);
        void RemoveUserOrganisationAssociation(int userId);
        List<string> GetUserRoleList(int userId);
    }
}
