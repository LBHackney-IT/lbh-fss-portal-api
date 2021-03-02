using System.Collections.Generic;
using System.Threading.Tasks;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Infrastructure;

namespace LBHFSSPortalAPI.V1.Gateways.Interfaces
{
    public interface ISynonymGroupsGateway
    {
        SynonymGroupDomain CreateSynonymGroup(SynonymGroup request);
        SynonymGroupDomain GetSynonymGroup(int id);
        Task<List<SynonymGroupDomain>> SearchSynonymGroups(SynonymGroupSearchRequest requestParams);
        void DeleteSynonymGroup(int id);
        SynonymGroupDomain PatchSynonymGroup(SynonymGroupDomain synonymGroup);
    }
}
