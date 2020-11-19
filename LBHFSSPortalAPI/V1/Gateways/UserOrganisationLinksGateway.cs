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
    public class UserOrganisationLinksGateway : BaseGateway, IUserOrganisationLinksGateway
    {
        private readonly MappingHelper _mapper;

        public UserOrganisationLinksGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }        

        public void DeleteUserOrganisationLink(int userId)
        {
            try
            {
                var userOrganisationLink = Context.UserOrganisations
                    .FirstOrDefault(o => o.UserId == userId);
                if (userOrganisationLink == null)
                    throw new InvalidOperationException("User Organisation Link does not exist");
                Context.UserOrganisations.Remove(userOrganisationLink);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }
        
        public async Task<UserOrganisationDomain> LinkUserToOrganisationAsync(Organisation organisation, User user)
        {
            LoggingHandler.LogInfo($"Linking user {user.Name} to organisation {organisation.Name}");
            var userOrganisation = new UserOrganisation { UserId = user.Id, OrganisationId = organisation.Id, CreatedAt = DateTime.Now };
            try
            {
                Context.UserOrganisations.Add(userOrganisation);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                LoggingHandler.LogError("Error linking user to organisation");
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
            userOrganisation = await GetUserOrganisationById(userOrganisation.Id).ConfigureAwait(false);
            return _mapper.ToDomain(userOrganisation);
        }

        public async Task<UserOrganisation> GetUserOrganisationById(int id)
        {
            var userOrganisation = await Context.UserOrganisations
                .SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return userOrganisation;
        }
    }
}
