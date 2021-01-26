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
    public class TaxonomyGateway : BaseGateway, ITaxonomyGateway
    {
        private readonly MappingHelper _mapper;

        public TaxonomyGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }

        public TaxonomyDomain CreateTaxonomy(Taxonomy request)
        {
            try
            {
                Context.Taxonomies.Add(request);
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

        public TaxonomyDomain GetTaxonomy(int id)
        {
            try
            {
                var taxonomy = Context.Taxonomies
                    .FirstOrDefault(o => o.Id == id);
                return _mapper.ToDomain(taxonomy);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public void DeleteTaxonomy(int id)
        {
            try
            {
                var taxonomy = Context.Taxonomies
                    .Include(o => o.ServiceTaxonomies)
                    .FirstOrDefault(o => o.Id == id);
                if (taxonomy == null)
                    throw new InvalidOperationException("Taxonomy does not exist");
                Context.Taxonomies.Remove(taxonomy);
                SaveChanges();
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public TaxonomyDomain PatchTaxonomy(int id, Taxonomy taxonomy)
        {
            try
            {
                var taxonomyEntity = Context.Taxonomies
                    .Include(o => o.ServiceTaxonomies)
                    .FirstOrDefault(o => o.Id == id);
                if (taxonomy == null)
                    throw new InvalidOperationException("Taxonomy does not exist");
                taxonomyEntity.Name = taxonomy.Name;
                taxonomyEntity.Description = taxonomy.Description;
                taxonomyEntity.Vocabulary = taxonomy.Vocabulary;
                taxonomyEntity.Weight = taxonomy.Weight;
                Context.SaveChanges();
                return _mapper.ToDomain(taxonomyEntity);
            }
            catch (DbUpdateException dbe)
            {
                HandleDbUpdateException(dbe);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
            return null;
        }

        public List<TaxonomyDomain> GetAllTaxonomies()
        {
            try
            {
                var taxonomies = Context.Taxonomies.AsQueryable();
                return _mapper.ToDomain(taxonomies);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public List<TaxonomyDomain> GetTaxonomiesByVocabulary(string vocabulary)
        {
            try
            {
                var taxonomies = Context.Taxonomies.Where(x => x.Vocabulary == vocabulary);
                return _mapper.ToDomain(taxonomies);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public List<ServiceTaxonomyDomain> GetServiceTaxonomies(int taxonomyId)
        {
            try
            {
                var serviceTaxonomies = Context.ServiceTaxonomies
                    .Where(x => x.TaxonomyId == taxonomyId)
                    .Include(x => x.Service);
                return _mapper.ToDomain(serviceTaxonomies);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }
    }
}
