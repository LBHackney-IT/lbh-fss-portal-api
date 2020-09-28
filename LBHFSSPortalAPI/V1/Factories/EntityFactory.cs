using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class EntityFactory
    {
        public static User ToEntity(this UserDomain userDomain)
        {
            return new User
            {
                Id = userDomain.Id,
                Email = userDomain.Email,
                Name = userDomain.Name,
                Status = userDomain.Status,
                CreatedAt = userDomain.CreatedAt,
                SubId = userDomain.SubId
            };
        }
    }
}
