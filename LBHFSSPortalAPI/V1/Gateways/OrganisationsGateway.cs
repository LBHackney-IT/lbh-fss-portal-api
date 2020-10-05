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
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class OrganisationsGateway : BaseGateway, IOrganisationsGateway
    {
        private readonly MappingHelper _mapper;

        public OrganisationsGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }
        public OrganisationDomain CreateOrganisation(Organisation request)
        {
            try
            {
                Context.Organisations.Add(request);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return _mapper.ToDomain(request);
        }

        public OrganisationDomain GetOrganisation(int id)
        {
            try
            {
                var organisation = Context.Organisations.Find(id);
                return _mapper.ToDomain(organisation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<OrganisationDomain>> SearchOrganisations(OrganisationSearchRequest requestParams)
        {
            // Search       search term to use (searches on [name] column for the MVP)
            // Sort         the column name by which to sort
            // Direction    sort order; asc, desc
            // Limit        maximum number of records to return
            // Offset       number of records to skip for pagination

            List<OrganisationDomain> response = new List<OrganisationDomain>();
            var direction = ConvertToEnum(requestParams.Direction);

            if (direction == SortDirection.None)
                throw new UseCaseException()
                {
                    UserErrorMessage = "The sort direction was not valid (must be one of asc, desc)"
                };

            var matchingOrganisations = Context.Organisations.AsQueryable();

            // handle search
            if (!string.IsNullOrWhiteSpace(requestParams.Search))
                matchingOrganisations = matchingOrganisations.Where(o => EF.Functions.Like(o.Name, $"%{requestParams.Search}%"));

            // handle sort by column name and sort direction
            var entityPropName = GetEntityPropertyForColumnName(typeof(Organisation), requestParams.Sort);

            if (entityPropName == null)
                throw new UseCaseException()
                {
                    UserErrorMessage = $"The 'Sort' parameter contained the value '{requestParams.Sort}' " +
                                        "which is not a valid column name"
                };

            matchingOrganisations = (direction == SortDirection.Asc) ?
                matchingOrganisations.OrderBy(u => EF.Property<Organisation>(u, entityPropName)) :
                matchingOrganisations.OrderByDescending(u => EF.Property<Organisation>(u, entityPropName));

            // handle pagination options
            if (requestParams.Limit.HasValue)
                matchingOrganisations = matchingOrganisations.Take(requestParams.Limit.Value);

            if (requestParams.Offset.HasValue)
                matchingOrganisations = matchingOrganisations.Skip(requestParams.Offset.Value);

            try
            {
                var organisationList = await matchingOrganisations
                    .Include(o => o.ReviewerU)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                response = _mapper.ToDomain(organisationList);
            }
            catch (InvalidOperationException e)
            {
                throw new UseCaseException()
                {
                    UserErrorMessage = "Could not run the user search query with the supplied input parameters",
                    DevErrorMessage = e.Message
                };
            }

            return response;
        }

        public void DeleteOrganisation(int id)
        {
            try
            {
                var organisation = Context.Organisations.Find(id);
                if (organisation == null)
                    throw new InvalidOperationException("Organisation does not exist");
                Context.Organisations.Remove(organisation);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public OrganisationDomain PatchOrganisation(OrganisationDomain organisationDomain)
        {
            try
            {
                var org = Context.Organisations.Find(organisationDomain.Id);
                org.Name = organisationDomain.Name;
                org.CreatedAt = organisationDomain.CreatedAt;
                org.UpdatedAt = organisationDomain.UpdatedAt;
                org.SubmittedAt = organisationDomain.SubmittedAt;
                org.ReviewedAt = organisationDomain.ReviewedAt;
                org.ReviewerMessage = organisationDomain.ReviewerMessage;
                org.Status = organisationDomain.Status;
                org.IsRegisteredCharity = organisationDomain.IsRegisteredCharity;
                org.CharityNumber = organisationDomain.CharityNumber;
                org.HasHcOrColGrant = organisationDomain.HasHcOrColGrant;
                org.HasHcvsOrHgOrAelGrant = organisationDomain.HasHcvsOrHgOrAelGrant;
                org.IsTraRegistered = organisationDomain.IsTraRegistered;
                org.RslOrHaAssociation = organisationDomain.RslOrHaAssociation;
                org.IsLotteryFunded = organisationDomain.IsLotteryFunded;
                org.LotteryFundedProject = organisationDomain.LotteryFundedProject;
                org.FundingOther = organisationDomain.FundingOther;
                org.HasChildSupport = organisationDomain.HasChildSupport;
                org.ChildSafeguardingLeadFirstName = organisationDomain.ChildSafeguardingLeadFirstName;
                org.ChildSafeguardingLeadLastName = organisationDomain.ChildSafeguardingLeadLastName;
                org.ChildSafeguardingLeadTrainingMonth = organisationDomain.ChildSafeguardingLeadTrainingMonth;
                org.ChildSafeguardingLeadTrainingYear = organisationDomain.ChildSafeguardingLeadTrainingYear;
                org.HasAdultSupport = organisationDomain.HasAdultSupport;
                org.HasAdultSafeguardingLead = organisationDomain.HasAdultSafeguardingLead;
                org.AdultSafeguardingLeadFirstName = organisationDomain.AdultSafeguardingLeadFirstName;
                org.AdultSafeguardingLeadLastName = organisationDomain.AdultSafeguardingLeadLastName;
                org.AdultSafeguardingLeadTrainingMonth = organisationDomain.AdultSafeguardingLeadTrainingMonth;
                org.AdultSafeguardingLeadTrainingYear = organisationDomain.AdultSafeguardingLeadTrainingYear;
                org.HasEnhancedSupport = organisationDomain.HasEnhancedSupport;
                org.IsLocalOfferListed = organisationDomain.IsLocalOfferListed;
                Context.SaveChanges();
                return organisationDomain;
            }
            catch (DbUpdateException dbe)
            {
                HandleDbUpdateException(dbe);
                throw;
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);
                throw;
            }
        }
    }
}
