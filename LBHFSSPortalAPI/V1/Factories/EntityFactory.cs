using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class EntityFactory
    {
        public static UserDomain ToDomain(this User user)
        {
            var userDomain = new UserDomain()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Status = user.Status,
                CreatedAt = user.CreatedAt,
                SubId = user.SubId,
            };

            // Only one association expected in the MVP version
            var org = user.Organizations.FirstOrDefault();

            if (org != null)
                userDomain.Organisation = org.ToDomain();

            return userDomain;
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
