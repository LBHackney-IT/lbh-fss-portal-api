using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.Validations
{
    public static class UserCreateRequestValidator
    {
        public static bool IsValid(this UserCreateRequest createRequest)
        {
            return !string.IsNullOrWhiteSpace(createRequest.EmailAddress);
        }
    }
}
