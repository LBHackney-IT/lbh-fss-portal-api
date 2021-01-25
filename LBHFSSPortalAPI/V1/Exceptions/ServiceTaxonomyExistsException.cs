
using LBHFSSPortalAPI.V1.Boundary.Response;
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Exceptions
{
    public class ServiceTaxonomyExistsException : BaseException
    {
        public List<ServiceError> Services { get; set; }
    }
}
