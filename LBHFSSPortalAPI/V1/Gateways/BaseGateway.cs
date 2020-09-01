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
                ApiErrorMessage = "An API database error was detected",
                DeveloperErrorMessage = e.Message
            };

            var npgSqlEx = e.InnerException as Npgsql.PostgresException;

            if (npgSqlEx != null)
            {
                exception.DeveloperErrorMessage += Environment.NewLine + npgSqlEx.ConstraintName;
                exception.DeveloperErrorMessage += Environment.NewLine + npgSqlEx.Detail;
                exception.DeveloperErrorMessage += Environment.NewLine + npgSqlEx.MessageText;
            }

            throw exception;
        }
    }
}
