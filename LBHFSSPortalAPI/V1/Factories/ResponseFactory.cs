using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using Microsoft.AspNetCore.Http;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class ResponseFactory
    {
        //TODO: Map the fields in the domain object(s) to fields in the response object(s).
        // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings
        public static ResponseObject ToResponse(this Entity domain)
        {
            return new ResponseObject();
        }

        public static List<ResponseObject> ToResponse(this IEnumerable<Entity> domainList)
        {
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }

        public static UserResponse ToResponse(this UserDomain domain)
        {
            var response = new UserResponse()
            {
                Id = domain.Id,
                Name = domain.Name,
                Email = domain.Email,
                Status = domain.Status,
                CreatedAt = domain.CreatedAt,
                SubId = domain.SubId
            };

            if (domain.Organisation != null)
                response.Organisation = domain.Organisation.ToResponse();

            return response;
        }

        public static List<UserResponse> ToResponse(this IEnumerable<UserDomain> users)
        {
            return users.Select(p => p.ToResponse()).ToList();
        }

        public static OrganisationResponse ToResponse(this OrganizationDomain domain)
        {
            return new OrganisationResponse()
            {
                Id = domain.Id,
                Name = domain.Name
            };
        }

        public static List<OrganisationResponse> ToResponse(this IEnumerable<OrganizationDomain> orgs)
        {
            return orgs.Select(o => o.ToResponse()).ToList();
        }

        //public static ServiceResponse ToResponse(this ServiceDomain domain)
        //{
        //    return new ServiceResponse()
        //    {
        //    };
        //}

        //public static List<ServiceResponse> ToResponse(this IEnumerable<ServiceDomain> services)
        //{
        //    return services.Select(o => o.ToResponse()).ToList();
        //}
    }
}
