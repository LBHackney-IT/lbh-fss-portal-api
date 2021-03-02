using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    public class SynonymsResponse
    {
        [JsonIgnore]
        public int Id { get; set; }
        public bool Success { get; set; }
    }
}
