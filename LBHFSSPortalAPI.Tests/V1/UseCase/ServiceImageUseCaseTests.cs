using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.UseCase
{
    [TestFixture]
    public class ServiceImageUseCaseTests
    {
        private ServiceImageUseCase _classUnderTest;
        private Mock<IServicesGateway> _mockServicesGateway;
        private Mock<IRepositoryGateway> _mockRepositoryGateway;

        public ServiceImageUseCaseTests()
        {
            _mockServicesGateway = new Mock<IServicesGateway>();
            _mockRepositoryGateway = new Mock<IRepositoryGateway>();
            _classUnderTest = new ServiceImageUseCase(_mockRepositoryGateway.Object, _mockServicesGateway.Object);
        }

        [TestCase(TestName =
            "When the service image executedelete action is called the services gateway deletefile gets called")]
        public void ImageDeleteActionCallsServicesGateway()
        {
            var serviceId = Randomm.Id();
            var imageId = Randomm.Id();
            var file = EntityHelpers.CreateFile();
            _mockServicesGateway.Setup(sg => sg.GetFile(It.IsAny<int>())).ReturnsAsync(file);
            _classUnderTest.ExecuteDelete(serviceId, imageId);
            _mockServicesGateway.Verify(sg => sg.DeleteFileInfo(It.IsAny<int>(), It.IsAny<File>()), Times.Once);
        }
    }
}
