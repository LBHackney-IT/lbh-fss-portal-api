using System.IO;
using Microsoft.AspNetCore.Http;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ServiceImageRequest
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}
