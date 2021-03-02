using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using File = Google.Apis.Drive.v3.Data.File;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GoogleClientMocked : IGoogleClient
    {
        /// <summary>
        /// The service initializer
        /// </summary>
        private BaseClientService.Initializer _initializer;

        public async Task<IList<File>> GetFilesInDriveAsync(string driveId)
        {
            IList<File> fileList = new List<File>();
            //var fileMock = new Mock<IFile>();
            //fileList
            await Task.Run(() => { }).ConfigureAwait(false);
            return fileList;
        }

        public async Task<IList<IList<object>>> ReadSheetToObjectRowListAsync(string spreadSheetId, string sheetName, string sheetRange)
        {
            IList<IList<object>> mockedResult = new List<IList<object>>();
            Random rnd = new Random();
            int synonymGroupCount = rnd.Next(10, 100);
            for (int g = 0; g <= synonymGroupCount; g++)
            {
                List<object> synonymGroup = new List<object>();
                int synonymWordCount = rnd.Next(2, 13);
                for (int w = 0; w <= synonymWordCount; w++)
                {
                    int synonymWordLength = rnd.Next(4, 30);
                    string word = RandomString(synonymWordLength);
                    synonymGroup.Add(word);
                }
                mockedResult.Add(synonymGroup);
            }
            await Task.Run(() => { }).ConfigureAwait(false);
            return mockedResult;
        }

        private static readonly Random _random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public Task InitialiseWithGoogleApiKey(string googleApiKey)
        {
            _initializer = new BaseClientService.Initializer
            {
                ApplicationName = "applicationName",
                ApiKey = "apiKey"
            };
            return Task.CompletedTask;
        }
    }
}
