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
    public class SynonymWordsGateway : BaseGateway, ISynonymWordsGateway
    {
        private readonly MappingHelper _mapper;

        public SynonymWordsGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }

        public SynonymWordDomain CreateSynonymWord(SynonymWord request)
        {
            try
            {
                Context.SynonymWords.Add(request);
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

        public void DeleteSynonymWord(int id)
        {
            try
            {
                var synonymWord = Context.SynonymWords
                    .FirstOrDefault(o => o.Id == id);
                if (synonymWord == null)
                    throw new InvalidOperationException("SynonymWord does not exist");
                Context.SynonymWords.Remove(synonymWord);
                SaveChanges();
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public SynonymWordDomain GetSynonymWord(int id)
        {
            try
            {
                var synonymWord = Context.SynonymWords
                    .FirstOrDefault(o => o.Id == id);
                return _mapper.ToDomain(synonymWord);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public SynonymWordDomain PatchSynonymWord(SynonymWordDomain synonymWordDomain)
        {
            try
            {
                var sw = Context.SynonymWords
                    .FirstOrDefault(o => o.Id == synonymWordDomain.Id);
                sw.Word = synonymWordDomain.Word;
                sw.GroupId = synonymWordDomain.GroupId;
                sw.CreatedAt = synonymWordDomain.CreatedAt;
                Context.SynonymWords.Attach(sw);
                SaveChanges();
                return _mapper.ToDomain(sw);
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

        public async Task<List<SynonymWordDomain>> SearchSynonymWords(SynonymWordSearchRequest requestParams)
        {
            // Search       search term to use (searches on [name] column for the MVP)
            // Sort         the column name by which to sort
            // Direction    sort order; asc, desc

            List<SynonymWordDomain> response = new List<SynonymWordDomain>();
            var direction = ConvertToEnum(requestParams.Direction);

            if (direction == SortDirection.None)
                throw new UseCaseException()
                {
                    UserErrorMessage = "The sort direction was not valid (must be one of asc, desc)"
                };

            var matchingSynonymWords = Context.SynonymWords.AsQueryable();

            if (requestParams.GroupId > 0)
                matchingSynonymWords = matchingSynonymWords.Where(w => w.GroupId == requestParams.GroupId);

            // handle search
            if (!string.IsNullOrWhiteSpace(requestParams.Search))
                matchingSynonymWords = matchingSynonymWords.Where(o => EF.Functions.ILike(o.Word, $"%{requestParams.Search}%"));

            // handle sort by column name and sort direction
            var entityPropName = GetEntityPropertyForColumnName(typeof(SynonymWord), requestParams.Sort);

            if (entityPropName == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"The 'Sort' parameter contained the value '{requestParams.Sort}' " +
                                        "which is not a valid column name"
                };

            matchingSynonymWords = (direction == SortDirection.Asc) ?
                matchingSynonymWords.OrderBy(u => EF.Property<SynonymWord>(u, entityPropName)) :
                matchingSynonymWords.OrderByDescending(u => EF.Property<SynonymWord>(u, entityPropName));

            try
            {
                var synonymGroupList = await matchingSynonymWords
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                response = _mapper.ToDomain(synonymGroupList);
            }
            catch (InvalidOperationException e)
            {
                throw new UseCaseException()
                {
                    UserErrorMessage = "Could not run the synonym word search query with the supplied input parameters",
                    DevErrorMessage = e.Message
                };
            }

            return response;
        }
    }
}
