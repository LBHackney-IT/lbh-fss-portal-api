using LBHFSSPortalAPI.V1.Controllers;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Moq;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Controllers
{
    [TestFixture]
    public class OrganizationsControllerTests
    {
        private OrganisationsController _classUnderTest;
        private Mock<IOrganisationsUseCase> _mockUseCase;

        [SetUp]
        public void SetUp()
        {
            _mockUseCase = new Mock<IOrganisationsUseCase>();
            _classUnderTest = new OrganisationsController(_mockUseCase.Object);
        }
    }
}
