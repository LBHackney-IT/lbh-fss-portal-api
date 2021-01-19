using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Factories
{
    public static class ServiceLocationRequestFactory
    {
        public static ServiceLocationDomain ToDomain(this ServiceLocationRequest request)
        {
            return new ServiceLocationDomain()
            {
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode,
                StateProvince = request.StateProvince
            };
        }

        public static ServiceLocation ToEntity(this ServiceLocationRequest request)
        {
            return new ServiceLocation()
            {
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode,
                StateProvince = request.StateProvince,
                Uprn = request.Uprn.ToString()
            };
        }

        public static List<ServiceLocationDomain> ToDomain(this List<ServiceLocationRequest> locations)
        {
            return locations.Select(s => ToDomain(s)).ToList();
        }

        public static List<ServiceLocation> ToEntity(this List<ServiceLocationRequest> locations)
        {
            return locations.Select(s => ToEntity(s)).ToList();
        }
    }
}
