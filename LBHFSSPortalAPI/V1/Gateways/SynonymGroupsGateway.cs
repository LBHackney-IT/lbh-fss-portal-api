using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Boundary.Requests;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Handlers;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class SynonymGroupsGateway : BaseGateway, ISynonymGroupsGateway
    {
        private readonly MappingHelper _mapper;

        public SynonymGroupsGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }

        public SynonymGroupDomain CreateSynonymGroup(SynonymGroup request)
        {
            try
            {
                Context.SynonymGroups.Add(request);
                SaveChanges();
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
            return _mapper.ToDomain(request);
        }

        public void DeleteSynonymGroup(int id)
        {
            try
            {
                var synonymGroup = Context.SynonymGroups
                    .Include(o => o.SynonymWords)
                    .FirstOrDefault(o => o.Id == id);
                if (synonymGroup == null)
                    throw new InvalidOperationException("SynonymGroup does not exist");
                Context.SynonymGroups.Remove(synonymGroup);
                SaveChanges();
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public SynonymGroupDomain GetSynonymGroup(int id)
        {
            try
            {
                var synonymGroup = Context.SynonymGroups
                    .Include(o => o.SynonymWords)
                    .FirstOrDefault(o => o.Id == id);
                return _mapper.ToDomain(synonymGroup);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public SynonymGroupDomain PatchSynonymGroup(SynonymGroupDomain synonymGroupDomain)
        {
            try
            {
                var sg = Context.SynonymGroups
                    .Include(o => o.SynonymWords)
                    .FirstOrDefault(o => o.Id == synonymGroupDomain.Id);
                sg.Name = synonymGroupDomain.Name;
                sg.CreatedAt = synonymGroupDomain.CreatedAt;
                Context.SynonymGroups.Attach(sg);
                SaveChanges();
                return _mapper.ToDomain(sg);
            }
            catch (DbUpdateException dbe)
            {
                HandleDbUpdateException(dbe);
                throw;
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public List<SynonymGroupDomain> SearchSynonymGroups(SynonymGroupSearchRequest requestParams)
        {
            // Search       search term to use (searches on [name] column for the MVP)
            // Sort         the column name by which to sort
            // Direction    sort order; asc, desc

            List<SynonymGroupDomain> response = new List<SynonymGroupDomain>();
            var direction = ConvertToEnum(requestParams.Direction);

            if (direction == SortDirection.None)
                throw new UseCaseException()
                {
                    UserErrorMessage = "The sort direction was not valid (must be one of asc, desc)"
                };
            LoggingHandler.LogInfo("Getting existing synonyms from the db.");
            var matchingSynonymGroups = Context.SynonymGroups.AsQueryable();

            // handle search
            if (!string.IsNullOrWhiteSpace(requestParams.Search))
                matchingSynonymGroups = matchingSynonymGroups.Where(o => EF.Functions.ILike(o.Name, $"%{requestParams.Search}%"));

            // handle sort by column name and sort direction
            var entityPropName = GetEntityPropertyForColumnName(typeof(SynonymGroup), requestParams.Sort);

            if (entityPropName == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"The 'Sort' parameter contained the value '{requestParams.Sort}' " +
                                        "which is not a valid column name"
                };

            matchingSynonymGroups = (direction == SortDirection.Asc) ?
                matchingSynonymGroups.OrderBy(u => EF.Property<SynonymGroup>(u, entityPropName)) :
                matchingSynonymGroups.OrderByDescending(u => EF.Property<SynonymGroup>(u, entityPropName));

            try
            {
                var synonymGroupList = matchingSynonymGroups
                    .Include(o => o.SynonymWords)
                    .AsNoTracking()
                    .ToList();

                response = _mapper.ToDomain(synonymGroupList);
            }
            catch (InvalidOperationException e)
            {
                throw new UseCaseException()
                {
                    UserErrorMessage = "Could not run the synonym group search query with the supplied input parameters",
                    DevErrorMessage = e.Message
                };
            }
            return response;
        }
    }
}
