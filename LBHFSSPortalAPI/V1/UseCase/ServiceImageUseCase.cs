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
    }
}
