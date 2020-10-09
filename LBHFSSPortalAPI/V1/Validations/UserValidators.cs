using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.Validations
{
    public static class UserValidators
    {
        public static bool IsValid(this UserCreateRequest createRequest)
        {
            return !string.IsNullOrWhiteSpace(createRequest.Email);
        }

        public static bool IsValid(this UserConfirmRequest confirmRequest)
        {
            return (!string.IsNullOrWhiteSpace(confirmRequest.Email)) &&
                   (!string.IsNullOrWhiteSpace(confirmRequest.Code));
        }

        public static bool IsValid(this ConfirmationResendRequest confirmRequest)
        {
            return (!string.IsNullOrWhiteSpace(confirmRequest.Email));
        }

        public static bool IsValid(this AdminCreateUserRequest userCreateRequest)
        {
            return (!string.IsNullOrWhiteSpace(userCreateRequest.Email)) &&
                   (!string.IsNullOrWhiteSpace(userCreateRequest.Name));
        }
    }
}
