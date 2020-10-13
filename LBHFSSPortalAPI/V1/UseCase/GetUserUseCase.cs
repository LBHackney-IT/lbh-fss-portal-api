using System;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly IUsersGateway _usersGateway;
        private readonly IAuthenticateGateway _authenticateGateway;
        private readonly ISessionsGateway _sessionsGateway;
        private readonly MappingHelper _mapper;

        public GetUserUseCase(IUsersGateway usersGateway, IAuthenticateGateway authenticateGateway, ISessionsGateway sessionsGateway)
        {
            _usersGateway = usersGateway;
            _authenticateGateway = authenticateGateway;
            _sessionsGateway = sessionsGateway;
            _mapper = new MappingHelper();
        }

        public async Task<UserResponse> Execute(int userId)
        {
            var userDomain = await _usersGateway.GetUserByIdAsync(userId).ConfigureAwait(false);

            if (userDomain == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"Could not retrieve a user with an ID of '{userId}'",
                };
            var userStatus = _authenticateGateway.GetUserStatus(userDomain.Email);
            var userResponse = userDomain.ToResponse();
            userResponse.Status = userStatus;
            userResponse.SetPasswordRequired = userStatus == null || userStatus == "FORCE_CHANGE_PASSWORD";
            return userResponse;
        }

        public UserResponse Execute(string accessKey)
        {
            var session = _sessionsGateway.GetSessionByToken(accessKey);
            if (session != null)
                return session.User == null ? null :  _mapper.ToDomain(session.User).ToResponse();
            return null;
        }
    }
}
