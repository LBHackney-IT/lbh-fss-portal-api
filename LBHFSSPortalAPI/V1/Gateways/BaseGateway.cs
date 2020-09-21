using System;
using LBHFSSPortalAPI.V1.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class BaseGateway
    {
        public static void HandleDbUpdateException(DbUpdateException e)
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
    }
}
