using System.IO;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface IRepositoryGateway
    {
        Task<Infrastructure.File> UploadImage(ServiceImageRequest request);
    }
}
