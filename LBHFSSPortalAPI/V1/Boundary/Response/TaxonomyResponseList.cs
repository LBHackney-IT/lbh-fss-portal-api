using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class TaxonomyResponseList
    {
        public List<TaxonomyResponse> Categories { get; set; }
        public List<TaxonomyResponse> Demographics { get; set; }
    }
}
