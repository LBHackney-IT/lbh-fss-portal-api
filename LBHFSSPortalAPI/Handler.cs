using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Gateways;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Handlers;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace LBHFSSPortalAPI
{
    public class Handler
    {
        private readonly ISynonymsUseCase _synonymsUseCase;
        private readonly ISynonymGroupsGateway _synonymGroupsGateway;
        private readonly ISynonymWordsGateway _synonymWordsGateway;
        private readonly IGoogleClient _googleClient;
        private readonly DatabaseContext _context;

        public Handler()
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(connectionString);
            _context = new DatabaseContext(optionsBuilder.Options);
            _googleClient = new GoogleClient();
            _synonymGroupsGateway = new SynonymGroupsGateway(_context);
            _synonymWordsGateway = new SynonymWordsGateway(_context);
            _synonymsUseCase = new SynonymsUseCase(_synonymGroupsGateway, _synonymWordsGateway, _googleClient);
        }

        public void UpdateSynonyms()
        {
            var updateRequest = new SynonymUpdateRequest
            {
                GoogleFileId = Environment.GetEnvironmentVariable("SYNONYMS_GOOGLE_FILE_ID"),
                SheetName = Environment.GetEnvironmentVariable("SYNONYMS_SHEET_NAME"),
                SheetRange = Environment.GetEnvironmentVariable("SYNONYMS_SHEET_RANGE"),
                GoogleApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY")
            };
            LoggingHandler.LogInfo("Starting process of updating synonyms.");
            _synonymsUseCase.ExecuteUpdate(null, updateRequest);
        }
    }
}
