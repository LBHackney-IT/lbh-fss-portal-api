using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken.Model;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Handlers;
using LBHFSSPortalAPI.V1.Infrastructure;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class RepositoryGateway : IRepositoryGateway
    {
        private static readonly Amazon.RegionEndpoint _region = Amazon.RegionEndpoint.EUWest2;
        private AmazonS3Client _s3Client;
        private AWSCredentials _credentials;
        private static string _repositoryBucket = Environment.GetEnvironmentVariable("REPOSITORY_BUCKET");
        private static string _imagesBaseUrl = Environment.GetEnvironmentVariable("IMAGES_BASE_URL");
        private static string _imagesFolder = Environment.GetEnvironmentVariable("IMAGES_FOLDER");

        public RepositoryGateway(ConnectionInfo connectionInfo)
        {
            _s3Client = new AmazonS3Client(_region);
        }

        public async Task<Infrastructure.File> UploadImage(ServiceImageRequest request)
        {
            var fileExtension = Path.GetExtension(request.Image.FileName);
            var putObjectRequest = new PutObjectRequest();
            var originalFileName = $"{request.ServiceId.ToString()}-original{fileExtension}";
            var resizedFileName = $"{request.ServiceId.ToString()}-medium{fileExtension}";
            putObjectRequest.BucketName = _repositoryBucket;
            putObjectRequest.Key = _imagesFolder + "/" + originalFileName;
            putObjectRequest.InputStream = request.Image.OpenReadStream();
            try
            {
                await _s3Client.PutObjectAsync(putObjectRequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }

            // Use ImageSharper to resize image and save resized version to S3
            // memory stream
            using (var memoryStream = new MemoryStream())

            // Open the file automatically detecting the file type to decode it.
            // Our image is now in an uncompressed, file format agnostic, structure in-memory as
            // a series of pixels.
            using (Image image = Image.Load(request.Image.OpenReadStream()))
            {
                // Resize the image in place and return it for chaining.
                // 'x' signifies the current image processing context.
                image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

                var encoder = new JpegEncoder()
                {
                    Quality = 30 //Use variable to set between 5-30 based on your requirements
                };

                // The library automatically picks an encoder based on the file extension then
                // encodes and write the data to disk.
                // You can optionally set the encoder to choose.
                image.Save(memoryStream, encoder);
                putObjectRequest.Key = _imagesFolder + "/" + resizedFileName;
                putObjectRequest.InputStream = memoryStream;
                try
                {
                    await _s3Client.PutObjectAsync(putObjectRequest).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    LoggingHandler.LogError(e.Message);
                    LoggingHandler.LogError(e.StackTrace);
                    throw;
                }
            } // Dispose - releasing memory into a memory pool ready for the next image you wish to process.
            var fileEntity = new Infrastructure.File();
            fileEntity.Url = $"{_imagesBaseUrl}/{_imagesFolder}/{originalFileName};{_imagesBaseUrl}/{_imagesFolder}/{resizedFileName}";
            fileEntity.CreatedAt = DateTime.Now;
            return fileEntity;
        }
    }
}
