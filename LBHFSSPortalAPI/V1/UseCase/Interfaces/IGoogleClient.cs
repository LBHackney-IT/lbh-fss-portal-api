using System.Collections.Generic;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;

namespace LBHFSSPortalAPI.V1.UseCase.Interfaces
{
    /// <summary>
    /// The google client interface.
    /// </summary>
    public interface IGoogleClient
    {
        /// <summary>
        /// Gets the files in drive asynchronous.
        /// </summary>
        /// <param name="driveId">The drive identifier.</param>
        /// <returns>The list of files for the given drive.</returns>
        public Task<IList<File>> GetFilesInDriveAsync(string driveId);

        /// <summary>
        /// Reads the given spreadsheet to a List of rows asynchronous.
        /// </summary>
        /// <param name="spreadSheetId">The spread sheet identifier.</param>
        /// <param name="sheetName">Name of the sheet to read.</param>
        /// <param name="sheetRange">The sheet range to read.</param>
        /// <returns>
        /// An async task.containing a list to represent the worksheet content.
        /// </returns>
        public Task<IList<IList<object>>> ReadSheetToObjectRowListAsync(string spreadSheetId, string sheetName, string sheetRange);
    }
}
