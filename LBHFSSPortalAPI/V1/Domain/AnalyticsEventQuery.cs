using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class AnalyticsEventQuery
    {
        public int OrganisationId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
