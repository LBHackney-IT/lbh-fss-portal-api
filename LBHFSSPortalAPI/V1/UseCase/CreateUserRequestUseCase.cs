using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class CreateUserRequestUseCase : ICreateUserRequestUseCase
    {
        private IAuthenticateGateway _gateway;

        public CreateUserRequestUseCase(IAuthenticateGateway gateway)
        {
            _gateway = gateway;
        }
        public UserResponse Execute(UserCreateRequest createRequestData)
        {
            var createdUserId = _gateway.CreateUser(createRequestData);
            var userCreateResponse = new UserResponse
            {
                Email = createRequestData.Email,
                Name = createRequestData.Name,
                SubId = createdUserId
            };
            return userCreateResponse;
        }
    }
}
