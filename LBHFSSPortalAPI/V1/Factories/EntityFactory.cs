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

        public static List<UserDomain> ToDomain(this IEnumerable<Users> users)
        {
            return users.Select(p => p.ToDomain()).ToList();
        }
    }
}
