using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using File = Google.Apis.Drive.v3.Data.File;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class GoogleClient : IGoogleClient
    {
        /// <summary>
        /// The service initializer
        /// </summary>
        private readonly BaseClientService.Initializer _initializer;

        /// <summary>
        /// The sheets service backing variable
        /// </summary>
        private SheetsService _mSheetsService;

        /// <summary>
        /// Gets the sheets service.
        /// </summary>
        private SheetsService _sheetsService => _mSheetsService ??= new SheetsService(_initializer);

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] _scopes = { DriveService.Scope.DriveReadonly };
        static string _applicationName = "SynonymsApi";
        static UserCredential _userCredential;

        public GoogleClient()
        {
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                _userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            // Create service initializer
            _initializer = new BaseClientService.Initializer
            {
                HttpClientInitializer = _userCredential,
                ApplicationName = _applicationName
            };
        }

        #region Drive
        private DriveService _mDriveService;
        private DriveService _driveService => _mDriveService ??= new DriveService(_initializer);

        public async Task<IList<File>> GetFilesInDriveAsync(string driveId)
        {
            FilesResource.ListRequest listRequest = _driveService.Files.List();
            listRequest.DriveId = driveId;
            listRequest.IncludeItemsFromAllDrives = true;
            listRequest.Corpora = "Drive";

            // Recursively get files from drive
            return await GetFilesInDrive(listRequest, null);
        }

        private static async Task<IList<File>> GetFilesInDrive(FilesResource.ListRequest listRequest, string nextPage)
        {
            List<File> results = new List<File>();

            if (!string.IsNullOrWhiteSpace(nextPage))
            {
                listRequest.PageToken = nextPage;
            }

            FileList requestResult = await listRequest.ExecuteAsync();

            if (requestResult.Files?.Any() ?? false)
            {
                results.AddRange(requestResult.Files);

                if (!string.IsNullOrWhiteSpace(requestResult.NextPageToken))
                {
                    results.AddRange(await GetFilesInDrive(listRequest, requestResult.NextPageToken));
                }
            }

            return results;
        }
        #endregion

        #region Sheets
        public async Task<IList<IList<object>>> ReadSheetToObjectRowListAsync(string spreadSheetId, string sheetName, string sheetRange)
        {
            SpreadsheetsResource.ValuesResource.GetRequest getter =
                _sheetsService.Spreadsheets.Values.Get(spreadSheetId, $"{sheetName}!{sheetRange}");
            ValueRange response = await getter.ExecuteAsync();
            IList<IList<object>> values = response.Values;
            return values;
        }
        #endregion
    }
}
