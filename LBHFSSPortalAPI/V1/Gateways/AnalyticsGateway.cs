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
    public class AnalyticsGateway : BaseGateway, IAnalyticsGateway
    {
        private readonly MappingHelper _mapper;

        public AnalyticsGateway(DatabaseContext context) : base(context)
        {
            _mapper = new MappingHelper();
        }

        public List<AnalyticsEventDomain> GetAnalyticsEvents(AnalyticsEventQuery analyticsEventQuery)
        {
            try
            {
                var analyticsEvents = Context.AnalyticsEvents
                    .Where(x => x.Service.OrganisationId == analyticsEventQuery.OrganisationId)
                    .Where(a => analyticsEventQuery.StartDateTime == null || a.TimeStamp >= analyticsEventQuery.StartDateTime)
                    .Where(a => analyticsEventQuery.EndDateTime == null || a.TimeStamp <= analyticsEventQuery.EndDateTime)
                    .Include(s => s.Service);
                return _mapper.ToDomain(analyticsEvents);
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
