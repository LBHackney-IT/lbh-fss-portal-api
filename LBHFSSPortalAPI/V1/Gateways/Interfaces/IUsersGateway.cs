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
        void UpdateUserAndRoles(UserDomain user);
        UserDomain GetUserBySubId(string subId);
        UserDomain GetUserById(int userId);
        Task<UserDomain> GetUserByIdAsync(int userId);
        OrganisationDomain GetAssociatedOrganisation(int userId);
        OrganisationDomain AssociateUserWithOrganisation(int userId, int organisationId);
        void RemoveUserOrganisationAssociation(int userId);
        UserDomain SetDefaultRole(UserDomain user);
        List<string> GetUserRoleList(int userId);
        void SetUserStatus(UserDomain user, string status);
        void ClearUserRoles(int userId);
    }
}
