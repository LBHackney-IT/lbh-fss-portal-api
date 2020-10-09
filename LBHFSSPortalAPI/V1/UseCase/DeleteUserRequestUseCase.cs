using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class DeleteUserRequestUseCase : IDeleteUserRequestUseCase
    {
        private readonly IUsersGateway _usersGateway;
        private readonly ISessionsGateway _sessionsGateway;
        private readonly IAuthenticateGateway _authenticateGateway;

        public DeleteUserRequestUseCase(IUsersGateway usersGateway,
                                        ISessionsGateway sessionsGateway,
                                        IAuthenticateGateway authenticateGateway)
        {
            _usersGateway = usersGateway;
            _sessionsGateway = sessionsGateway;
            _authenticateGateway = authenticateGateway;
        }

        public bool Execute(int userId)
        {
            bool success = false;

            var user = _usersGateway.GetUserById(userId);

            if (_authenticateGateway.DeleteUser(user.Email))
            {
                user.Status = UserStatus.Deleted;
                _sessionsGateway.RemoveSessions(user.Id);
                _usersGateway.UpdateUser(user);
                success = true;
            }
            return success;
        }
    }
}
