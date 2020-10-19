using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ServiceImageRequest
    {
        [FromRoute]
        public int ServiceId { get; set; }
        public IFormFile Image { get; set; }
    }
}
