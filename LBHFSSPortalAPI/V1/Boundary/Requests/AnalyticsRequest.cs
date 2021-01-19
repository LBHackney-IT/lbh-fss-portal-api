using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class AnalyticsRequest
    {
        [FromQuery(Name = "organisationId")]
        public int OrganisationId { get; set; }
        [FromQuery(Name = "from_date")]
        public DateTime? StartDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? EndDate { get; set; }
    }
}
