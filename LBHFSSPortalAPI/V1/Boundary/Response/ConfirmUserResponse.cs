using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class ConfirmUserResponse
    {
        public const string AccessTokenName = "access_token";

        public UserResponse UserResponse { get; set; }

        public string AccessTokenValue { get; set; }
    }
}
