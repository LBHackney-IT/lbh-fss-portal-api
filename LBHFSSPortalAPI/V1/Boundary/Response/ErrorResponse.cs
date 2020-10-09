using System.Collections.Generic;
using System.Linq;

namespace LBHFSSPortalAPI.V1.Boundary.Response
{
    /// <summary>
    /// This is an Error Object that conforms to Hackney's API Playbook standards:
    ///
    ///     {
    ///        "status" : "fail",
    ///        "errors" : [ "A title is required" ]
    ///     }
    ///
    /// Defined at: "https://github.com/LBHackney-IT/API-Playbook-v2-beta/blob/master/api-guidelines/data-formats.md"
    /// </summary>
    public class ErrorResponse
    {
        public string Status { get; set; } = "fail";
        public List<string> Errors { get; set; }

        public ErrorResponse()
        {
            Errors = new List<string>();
        }

        public ErrorResponse(params string[] errorStrings)
        {
            Errors = errorStrings.ToList();
        }
    }
}
