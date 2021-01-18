using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHFSSPortalAPI.V1.Domain
{
    public class AddressXRefAPIResponse
    {
        public AddressXRefDomain Data { get; set; }

        public int StatusCode { get; set; }
    }

    public class AddressXRefDomain
    {
        [JsonProperty("addresscrossreferences")]
        public List<AddressXRef> AddressCrossReferences { get; set; }
    }
}
