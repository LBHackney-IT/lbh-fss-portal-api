
using System.Collections.Generic;

namespace LBHFSSPortalAPI.V1.Exceptions
{
    public class ServiceTaxonomyExistsException : BaseException
    {
        public Dictionary<int, string> Services { get; set; }
    }
}
