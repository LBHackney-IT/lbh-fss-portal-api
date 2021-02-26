using System;
using System.Text.Json.Serialization;

namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class SynonymUpdateRequest
    {
        public string GoogleFileId { get; set; }
        public string SheetName { get; set; }
        public string SheetRange { get; set; }
    }
}
