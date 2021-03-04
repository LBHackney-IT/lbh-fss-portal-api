using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface ISynonymWordsGateway
    {
        SynonymWordDomain CreateSynonymWord(SynonymWord request);
        SynonymWordDomain GetSynonymWord(int id);
        List<SynonymWordDomain> SearchSynonymWords(SynonymWordSearchRequest requestParams);
        void DeleteSynonymWord(int id);
        SynonymWordDomain PatchSynonymWord(SynonymWordDomain synonymWord);
    }
}
