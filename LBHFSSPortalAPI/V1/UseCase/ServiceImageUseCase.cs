using System;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Handlers;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class ServiceImageUseCase : IServiceImageUseCase
    {
        private readonly IRepositoryGateway _repositoryGateway;
        private readonly IServicesGateway _servicesGateway;

        public ServiceImageUseCase(IRepositoryGateway repositoryGateway, IServicesGateway servicesGateway)
        {
            _repositoryGateway = repositoryGateway;
            _servicesGateway = servicesGateway;
        }
        public async Task ExecuteCreate(ServiceImageRequest request)
        {
            var fileInfo = await _repositoryGateway.UploadImage(request).ConfigureAwait(false);
            if (fileInfo != null)
            {
                try
                {
                    _servicesGateway.AddFileInfo(request.ServiceId, fileInfo);
                }
                catch (Exception e)
                {
                    LoggingHandler.LogError(e.Message);
                    LoggingHandler.LogError(e.StackTrace);
                    throw;
                }
            }
        }

        public async Task ExecuteDelete(int serviceId, int imageId)
        {
            var file = await _servicesGateway.GetFile(imageId).ConfigureAwait(false);
            if (file == null)
                return;
            var images = file?.Url == null ? Array.Empty<string>() : file.Url.Split(';');
            var resizedFileName = images.Length > 1 ? images[1] : null;
            var originalFileName = images.Length > 0 ? images[0] : null;
            try
            {
                if (originalFileName != null)
                {
                    await _repositoryGateway.DeleteImage(originalFileName).ConfigureAwait(false);
                }
                if (resizedFileName != null)
                {
                    await _repositoryGateway.DeleteImage(resizedFileName).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                LoggingHandler.LogError("Error deleting object from S3.");
            }
            try
            {
                await _servicesGateway.DeleteFileInfo(serviceId, file).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError("Error deleting image.");
            }
        }
    }
}
