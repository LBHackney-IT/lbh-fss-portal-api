using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class EntityFactory
    {
        public static UserDomain ToDomain(this User usersEntity)
        {
            return new UserDomain
            {
                Id = usersEntity.Id,
                Email = usersEntity.Email,
                Name = usersEntity.Name,
                Status = usersEntity.Status,
                CreatedAt = usersEntity.CreatedAt,
                SubId = usersEntity.SubId,
            };
        }

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

        public static List<UserDomain> ToDomain(this IEnumerable<User> users)
        {
            return users.Select(p => p.ToDomain()).ToList();
        }

        public static OrganizationsDomain ToDomain(this Organization orgEntity)
        {
            return new OrganizationsDomain
            {
                Id = orgEntity.Id,
                Name = orgEntity.Name,
                CreatedAt = orgEntity.CreatedAt,
            };
        }

        public static List<OrganizationsDomain> ToDomain(this IEnumerable<Organization> orgs)
        {
            return orgs.Select(o => o.ToDomain()).ToList();
        }
    }
}
