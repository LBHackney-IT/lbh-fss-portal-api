using System;
using System.Globalization;
using LBHFSSPortalAPI.V1.Domain;
using LBHFSSPortalAPI.V1.Exceptions;
using LBHFSSPortalAPI.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public abstract class BaseGateway
    {
        protected DatabaseContext Context { get; set; }

        public BaseGateway(DatabaseContext databaseContext)
        {
            Context = databaseContext;
        }

        protected static void HandleDbUpdateException(DbUpdateException e)
        {
            var uce = new UseCaseException()
            {
                UserErrorMessage = "An API database error was detected",
                DevErrorMessage = e.Message
            };

            if (e.InnerException is Npgsql.PostgresException npgSqlEx)
            {
                uce.DevErrorMessage += Environment.NewLine + npgSqlEx.ConstraintName;
                uce.DevErrorMessage += Environment.NewLine + npgSqlEx.Detail;
                uce.DevErrorMessage += Environment.NewLine + npgSqlEx.MessageText;
            }

            throw uce;
        }

        protected SortDirection ConvertToEnum(string directionString)
        {
            if (!string.IsNullOrWhiteSpace(directionString))
            {
                directionString = directionString.ToLower(CultureInfo.CurrentCulture).Trim();

                if (Enum.TryParse(directionString, true, out SortDirection direction))
                    return direction;
            }

            return SortDirection.None;
        }

        /// <summary>
        /// Searches for the given database column name (case-insensitive) and returns EF entity name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnToFind"></param>
        /// <returns></returns>
        protected string GetEntityPropertyForColumnName(Type type, string columnToFind)
        {
            if (columnToFind != null)
            {
                var entityType = Context.Model.FindEntityType(type.FullName);

                if (entityType != null)
                {
                    columnToFind = columnToFind.Trim().ToLower(CultureInfo.CurrentCulture);

                    foreach (var prop in entityType.GetProperties())
                    {
                        var columnName = prop.GetColumnName();

                        if (string.Compare(columnToFind, columnName, true, CultureInfo.CurrentCulture) == 0)
                        {
                            return prop.Name;
                        }
                    }
                }
            }

            return null;
        }
    }
}
