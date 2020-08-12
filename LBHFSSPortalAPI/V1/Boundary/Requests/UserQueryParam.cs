using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserQueryParam
    {
        [FromQuery(Name = "category")]
        public string FirstName { get; set; }

        [FromQuery(Name = "sort")]
        public string LastName { get; set; }

        // TODO: add remaining (MJC)
    }
}
