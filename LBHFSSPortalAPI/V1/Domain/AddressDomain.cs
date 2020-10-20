using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class AddressAPIResponse
    {
        public AddressDomain Data { get; set; }
     
        public int StatusCode { get; set; }
    }


    public class AddressDomain
    {
        [JsonProperty("address")]
        public List<Address> Addresses { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}
