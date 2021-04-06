using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LBHFSSPortalAPI.Tests.TestHelpers;
using LBHFSSPortalAPI.V1.Gateways;
using NUnit.Framework;

namespace LBHFSSPortalAPI.Tests.V1.Gateways
{
    [TestFixture]
    public class ServicesGatewayTests : DatabaseTests
    {
        private ServicesGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new ServicesGateway(DatabaseContext);
        }

        [TestCase(TestName = "Given a service image id an image with the matching id is deleted")]
        public async Task GivenValidImageIdImageGetsDeleted()
        {
            var file = EntityHelpers.CreateFile();
            var service = EntityHelpers.CreateService();
            service.Image = file;
            service.ImageId = file.Id;
            await DatabaseContext.Files.AddAsync(file).ConfigureAwait(true);
            await DatabaseContext.Services.AddAsync(service).ConfigureAwait(true);
            await DatabaseContext.SaveChangesAsync().ConfigureAwait(true);
            await _classUnderTest.DeleteFileInfo(service.Id, file).ConfigureAwait(true);
            var expectedResult = DatabaseContext.Files.Where(x => x.Id == file.Id).SingleOrDefault();
            expectedResult.Should().BeNull();
            var expectedService = DatabaseContext.Services.Where(x => x.Id == service.Id).SingleOrDefault();
            service.Image.Should().BeNull();
        }
    }
}
