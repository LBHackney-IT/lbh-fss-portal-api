using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class ServiceErrorResponse
    {
        public string ErrorMessage { get; set; }
        public List<ServiceError> Services { get; set; }
    }

    public class ServiceError
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
    }
}
