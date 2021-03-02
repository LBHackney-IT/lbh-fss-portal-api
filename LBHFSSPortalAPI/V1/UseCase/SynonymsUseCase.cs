using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Boundary.Response;
using LBHFSSPortalAPI.V1.Enums;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;
using LBHFSSPortalAPI.V1.UseCase.Interfaces;

namespace LBHFSSPortalAPI.V1.UseCase
{
    public class SynonymsUseCase : ISynonymsUseCase
    {
        private readonly ISynonymGroupsGateway _synonymGroupsGateway;
        private readonly ISynonymWordsGateway _synonymWordsGateway;
        private readonly IGoogleClient _googleClient;

        public SynonymsUseCase(ISynonymGroupsGateway synonymGroupsGateway,
            ISynonymWordsGateway synonymWordsGateway,
            IGoogleClient googleClient)
        {
            _synonymGroupsGateway = synonymGroupsGateway;
            _synonymWordsGateway = synonymWordsGateway;
            _googleClient = googleClient;
        }

        public SynonymsResponse ExecuteUpdate(string accessToken, SynonymUpdateRequest requestParams)
        {
            SynonymsResponse response = new SynonymsResponse();
            UpdateSynonymChanges(accessToken, requestParams.GoogleFileId, requestParams.SheetName,
                requestParams.SheetRange, requestParams.GoogleApiKey).Wait();

            response.Success = true;
            return response;
        }

        /// <summary>
        /// Reads the given spreadsheet and updates database changes asynchronous.
        /// </summary>
        /// <param name="accessToken">Access token..</param>
        /// <param name="spreadSheetId">The spread sheet identifier.</param>
        /// <param name="sheetName">Name of the sheet to read.</param>
        /// <param name="sheetRange">The sheet range to read.</param>
        /// <param name="googleApiKey">The Google Api Key.</param>
        public async Task UpdateSynonymChanges(string accessToken, string spreadSheetId, string sheetName,
            string sheetRange, string googleApiKey)
        {
            IDictionary<string, string[]> synonymGroups =
                await ReadSpreadsheetAsync(spreadSheetId, sheetName, sheetRange, googleApiKey);
            if (synonymGroups == null || synonymGroups.Count == 0)
            {
                Console.WriteLine("There is no synonym data from the google spreadsheet.");
                return;
            }

            //compare groups with database results and delete anything that has been removed.
            SynonymGroupSearchRequest requestParams = new SynonymGroupSearchRequest()
            {
                Direction = "asc",
                Search = string.Empty,
                Sort = "name"
            };
            SynonymGroupResponseList synonymGroupsResponseList = await ExecuteGet(requestParams);
            try
            {
                await LoopDatabaseAndDeleteAnythingRemovedFromSpreadsheet(accessToken, synonymGroupsResponseList, synonymGroups);
                await LoopSpreadsheetAndUpdateDatabaseWithChanges(accessToken, synonymGroupsResponseList, synonymGroups);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Debugger.Break();
            }
        }

        private async Task LoopDatabaseAndDeleteAnythingRemovedFromSpreadsheet(string accessToken, SynonymGroupResponseList synonymGroupsResponseList, IDictionary<string, string[]> synonymGroups)
        {
            if (synonymGroupsResponseList != null && synonymGroupsResponseList.SynonymGroups != null)
            {
                foreach (var databaseSg in synonymGroupsResponseList.SynonymGroups)
                {
                    if (!synonymGroups.Any(s => s.Key.ToLower().Trim() == databaseSg.Name.ToLower()))
                    {
                        //Delete all words for that group
                        SynonymWordSearchRequest requestWordParams = new SynonymWordSearchRequest()
                        {
                            Direction = "asc",
                            GroupId = databaseSg.Id,
                            Sort = "word"
                        };
                        SynonymWordResponseList synonymWordResponseList = await ExecuteGetWord(requestWordParams);
                        foreach (var word in synonymWordResponseList.SynonymWords)
                        {
                            ExecuteDeleteWord(accessToken, word.Id);
                        }
                        //Delete the group
                        ExecuteDelete(accessToken, databaseSg.Id);
                    }
                }
            }
        }

        private async Task LoopSpreadsheetAndUpdateDatabaseWithChanges(string accessToken, SynonymGroupResponseList synonymGroupsResponseList, IDictionary<string, string[]> synonymGroups)
        {
            //Loop spreadsheet list and update changes if required
            DateTime today = DateTime.Now;
            if (synonymGroupsResponseList == null)
                synonymGroupsResponseList = new SynonymGroupResponseList() { SynonymGroups = new List<SynonymGroupResponse>() };
            foreach (var sg in synonymGroups)
            {
                if (sg.Value.Length > 0)
                {
                    int groupId = 0;
                    var synonymGroup =
                        synonymGroupsResponseList.SynonymGroups.Find(s => s.Name.ToLower() == sg.Key.ToLower());
                    if (synonymGroup == null) //Add
                    {

                        SynonymGroupRequest synonymGroupRequest =
                            new SynonymGroupRequest() { Name = sg.Key, CreatedAt = today };
                        var response = ExecuteCreate(accessToken, synonymGroupRequest);
                        groupId = response.Id;
                    }
                    else
                    {
                        groupId = synonymGroup.Id;
                    }

                    SynonymWordSearchRequest requestWordParams = new SynonymWordSearchRequest()
                    {
                        Direction = "asc",
                        GroupId = groupId,
                        Sort = "word"
                    };
                    SynonymWordResponseList synonymWordResponseList = await ExecuteGetWord(requestWordParams);
                    if (synonymWordResponseList == null)
                        synonymWordResponseList = new SynonymWordResponseList() { SynonymWords = new List<SynonymWordResponse>() };
                    List<SynonymWord> words = new List<SynonymWord>();
                    foreach (string word in sg.Value)
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            var synonymWord =
                                synonymWordResponseList.SynonymWords.Find(w => w.Word.ToLower() == word.ToLower());
                            if (synonymWord == null)//Add
                            {
                                SynonymWordRequest synonymWordRequest =
                                    new SynonymWordRequest() { Word = word, CreatedAt = today, GroupId = groupId };
                                var response = ExecuteCreateWord(accessToken, synonymWordRequest);
                            }
                        }
                    }
                }
            }
        }
        public SynonymGroupResponse ExecuteCreate(string accessToken, SynonymGroupRequest requestParams)
        {
            var gatewayResponse = _synonymGroupsGateway.CreateSynonymGroup(requestParams.ToEntity());
            return gatewayResponse == null ? new SynonymGroupResponse() : gatewayResponse.ToResponse();
        }

        public void ExecuteDeleteWord(string accessToken, int id)
        {
            _synonymWordsGateway.DeleteSynonymWord(id);
        }

        public void ExecuteDelete(string accessToken, int id)
        {
            _synonymGroupsGateway.DeleteSynonymGroup(id);
        }

        public SynonymWordResponse ExecuteCreateWord(string accessToken, SynonymWordRequest requestParams)
        {
            var gatewayResponse = _synonymWordsGateway.CreateSynonymWord(requestParams.ToEntity());
            return gatewayResponse == null ? new SynonymWordResponse() : gatewayResponse.ToResponse();
        }

        public SynonymGroupResponse ExecuteGetById(int id)
        {
            var gatewayResponse = _synonymGroupsGateway.GetSynonymGroup(id);
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }

        public SynonymWordResponse ExecuteGetWordById(int id)
        {
            var gatewayResponse = _synonymWordsGateway.GetSynonymWord(id);
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }

        public async Task<SynonymGroupResponseList> ExecuteGet(SynonymGroupSearchRequest requestParams)
        {
            var gatewayResponse = await _synonymGroupsGateway.SearchSynonymGroups(requestParams).ConfigureAwait(false);
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }

        public async Task<SynonymWordResponseList> ExecuteGetWord(SynonymWordSearchRequest requestParams)
        {
            var gatewayResponse = await _synonymWordsGateway.SearchSynonymWords(requestParams).ConfigureAwait(false);
            return gatewayResponse == null ? null : gatewayResponse.ToResponse();
        }

        public async Task<IDictionary<string, string[]>> ReadSpreadsheetAsync(string spreadSheetId, string sheetName, string sheetRange, string googleApiKey)
        {
            await _googleClient.InitialiseWithGoogleApiKey(googleApiKey);
            IList<IList<object>> values = await
                _googleClient.ReadSheetToObjectRowListAsync(spreadSheetId, sheetName, sheetRange);

            if (values == null || !values.Any())
            {
                Console.WriteLine("No data found.  Unresolved issue, so just return without making any updates.");
                return null;
            }

            //Start Synonyms
            IDictionary<string, string[]> synonymGroups = new Dictionary<string, string[]>();

            foreach (IList<object> row in values.Skip(2))
            {
                if (row.Count > 1)
                {
                    try
                    {
                        string synonymGroupName = row[0].ToString().Trim();
                        if (synonymGroupName.Length > 0)
                        {
                            List<string> words = new List<string>();
                            foreach (object cell in row) //Include group name.
                            {
                                if (cell != null)
                                {
                                    string word = cell.ToString().Trim();
                                    if (word.Length > 0)
                                        words.Add(word);
                                }
                            }

                            synonymGroups.Add(synonymGroupName, words.ToArray());
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e);
                        Debugger.Break();
                    }
                }
            }

            return synonymGroups;
        }
    }
}
