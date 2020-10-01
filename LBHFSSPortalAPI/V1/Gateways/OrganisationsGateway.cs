using System;
using Amazon.Lambda.Core;
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
        public OrganizationDomain CreateOrganisation(Organization request)
        {
            try
            {
                Context.Organizations.Add(request);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return _mapper.ToDomain(request);
        }

        public OrganizationDomain GetOrganisation(int id)
        {
            try
            {
                var organization = Context.Organizations.Find(id);
                return _mapper.ToDomain(organization);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DeleteOrganisation(int id)
        {
            try
            {
                var organization = Context.Organizations.Find(id);
                if (organization == null)
                    throw new InvalidOperationException("Organisation does not exist");
                Context.Organizations.Remove(organization);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public OrganizationDomain PatchOrganisation(OrganizationDomain organisationDomain)
        {
            try
            {
                var org = Context.Organizations.Find(organisationDomain.Id);
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
