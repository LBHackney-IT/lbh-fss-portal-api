using LBHFSSPortalAPI.V1.Exceptions;
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

        public void Execute(int userId)
        {
            var user = _usersGateway.GetUserById(userId);

            if (user == null)
                throw new UseCaseException() { UserErrorMessage = $"A user with the provided ID={userId} does not exist" };

            if (_authenticateGateway.DeleteUser(user.Email))
            {
                _sessionsGateway.RemoveSessions(user.Id);
                _usersGateway.SetUserStatus(user, UserStatus.Deleted);
            }
        }
    }
}
