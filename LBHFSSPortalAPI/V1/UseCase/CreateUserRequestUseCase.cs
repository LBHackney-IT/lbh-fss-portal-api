using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using System;

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

        public ConfirmUserResponse ExecuteConfirmUser(ConfirmUserQueryParam confirmUserQueryParam)
        {
            // TODO (MJC) Implement this method properly

            //test
            //throw new UseCaseException()
            //{
            //    ApiErrorMessage = $"user {confirmUserQueryParam.Name}/{confirmUserQueryParam.EmailAddress} was not found in the database",
            //    DeveloperErrorMessage = $"database context exception detected... (+ call stack)
            //};

            // stubbed response
            return new ConfirmUserResponse()
            {
                AccessTokenValue = "jsirfhghhsoaoakfmjashaaydrufgfjsjssdkfidijss",
                UserResponse = new UserResponse()
                {
                    Id = 1,
                    Name = "Betty Davis",
                    Email = "betty.davis@baesystems.com",
                    CreatedAt = ( DateTime.UtcNow - TimeSpan.FromDays(-7) ),
                    Status = "invited",
                    SubId = Guid.NewGuid().ToString()
                }
            };
        }
    }
}
