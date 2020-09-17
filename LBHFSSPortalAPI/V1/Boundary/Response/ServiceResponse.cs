
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class ServiceResponse
    {
        public ServiceResponse()
        {
            Locations = new List<LocationResponse>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Keywords { get; set; }
        public string ReferralLink { get; set; }
        public string ReferralEmail { get; set; }

        public List<LocationResponse> Locations { get; set; }



    }
}
