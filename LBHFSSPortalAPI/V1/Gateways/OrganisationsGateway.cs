using System;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Factories;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Infrastructure;

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
            // add some exception handling
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
            // add some exception handling
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
    }
}
