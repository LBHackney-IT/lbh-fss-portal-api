using System;
using LBHFSSPortalAPI.V1.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class BaseGateway
    {
        public static void HandleDbUpdateException(DbUpdateException e)
        {
            var exception = new UseCaseException()
            {
                UserErrorMessage = "An API database error was detected",
                DevErrorMessage = e.Message
            };

            var npgSqlEx = e.InnerException as Npgsql.PostgresException;

            if (npgSqlEx != null)
            {
                exception.DevErrorMessage += Environment.NewLine + npgSqlEx.ConstraintName;
                exception.DevErrorMessage += Environment.NewLine + npgSqlEx.Detail;
                exception.DevErrorMessage += Environment.NewLine + npgSqlEx.MessageText;
            }

            throw exception;
        }
    }
}
