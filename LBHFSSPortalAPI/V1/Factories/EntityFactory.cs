using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class EntityFactory
    {
        public static Entity ToDomain(this DatabaseEntity databaseEntity)
        {
            //TODO: Map the rest of the fields in the domain object.
            // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings

            return new Entity
            {
                Id = databaseEntity.Id,
                CreatedAt = databaseEntity.CreatedAt,
            };
        }

        public static UserDomain ToDomain(this Users usersEntity)
        {
            return new UserDomain
            {
                Id = usersEntity.Id,
                Email = usersEntity.Email,
                Name = usersEntity.Name,
                Status = usersEntity.Status,
                CreatedAt = usersEntity.CreatedAt,
                SubId = usersEntity.SubId
            };
        }

        public static Users ToEntity(this UserDomain userDomain)
        {
            return new Users
            {
                Id = userDomain.Id,
                Email = userDomain.Email,
                Name = userDomain.Name,
                Status = userDomain.Status,
                CreatedAt = userDomain.CreatedAt,
                SubId = userDomain.SubId
            };
        }

        public static List<UserDomain> ToDomain(this IEnumerable<Users> users)
        {
            return users.Select(p => p.ToDomain()).ToList();
        }

        public static OrganizationsDomain ToDomain(this Organizations orgEntity)
        {
            return new OrganizationsDomain
            {
                Id = orgEntity.Id,
                Name = orgEntity.Name,
                CreatedAt = orgEntity.CreatedAt,
            };
        }

        public static List<OrganizationsDomain> ToDomain(this IEnumerable<Organizations> orgs)
        {
            return orgs.Select(o => o.ToDomain()).ToList();
        }
    }
}
