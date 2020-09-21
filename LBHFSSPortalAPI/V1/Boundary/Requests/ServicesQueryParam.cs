
namespace LBHFSSPortalAPI.V1.Boundary.Requests
{
    public class ServicesQueryParam
    {
        /// <summary>
        /// Search term to use (searches on [name] column for the MVP)
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// The column name by which to sort
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Sort order; asc, desc
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Maximum number of records to return
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Number of records to skip for pagination
        /// </summary>
        public int? Offset { get; set; }
    }
}

