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
    public class UserOrganisationGateway : BaseGateway, IUserOrganisationGateway
    {
        private readonly MappingHelper _mapper;

        public UserOrganisationGateway(DatabaseContext context) : base(context)
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
                {
                    LoggingHandler.LogWarning("User Organisation Link does not exist");
                }
                else
                {
                    Context.UserOrganisations.Remove(userOrganisationLink);
                    Context.SaveChanges();
                    LoggingHandler.LogInfo("User organisation link removed successfully");
                }

            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }

        public UserOrganisationDomain LinkUserToOrganisation(int organisationId, int userId)
        {
            LoggingHandler.LogInfo($"Linking userId {userId} to organisation {organisationId}");
            var userOrganisation = new UserOrganisation { UserId = userId, OrganisationId = organisationId, CreatedAt = DateTime.Now };
            try
            {
                Context.UserOrganisations.Add(userOrganisation);
                Context.SaveChanges();
            }
            catch (DbUpdateException dbex)
            {
                LoggingHandler.LogError("Error linking user to organisation");
                LoggingHandler.LogError(dbex.Message);
                LoggingHandler.LogError(dbex.StackTrace);
                var deverror = dbex.Message;
                if (dbex.InnerException != null)
                {
                    deverror += "-" + dbex.InnerException.Message;
                }
                throw new UseCaseException()
                {
                    UserErrorMessage = "Error linking user to organisation",
                    DevErrorMessage = deverror
                };
            }
            catch (Exception e)
            {
                LoggingHandler.LogError("Error linking user to organisation");
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
            userOrganisation = GetUserOrganisationById(userOrganisation.Id);
            return _mapper.ToDomain(userOrganisation);
        }

        public UserOrganisationDomain GetUserOrganisationByUserAndOrgId(int userId, int organisationId)
        {
            var userOrganisation = Context.UserOrganisations
                .SingleOrDefault(x => x.UserId == userId && x.OrganisationId == organisationId);
            return _mapper.ToDomain(userOrganisation);
        }

        public UserOrganisation GetUserOrganisationById(int id)
        {
            var userOrganisation = Context.UserOrganisations
                .SingleOrDefault(x => x.Id == id);
            return userOrganisation;
        }
    }
}
