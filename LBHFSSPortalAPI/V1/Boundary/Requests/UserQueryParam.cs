using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class UserQueryParam
    {
        [FromQuery(Name = "firstname")]
        public string FirstName { get; set; }

        [FromQuery(Name = "lastname")]
        public string LastName { get; set; }

        // TODO: add remaining (MJC)
    }
}
