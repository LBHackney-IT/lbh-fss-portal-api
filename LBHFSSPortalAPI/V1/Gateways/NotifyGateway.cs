using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LBHFSSPortalAPI.V1.Enums;
using LBHFSSPortalAPI.V1.Gateways.Interfaces;
using LBHFSSPortalAPI.V1.Handlers;
using LBHFSSPortalAPI.V1.Infrastructure;
using Notify.Client;
using Notify.Exceptions;

namespace LBHFSSPortalAPI.V1.Gateways
{
    public class NotifyGateway : INotifyGateway
    {
        private readonly ConnectionInfo _connectionInfo;
        private readonly NotificationClient _client;
        private static string _reminderTemplate = Environment.GetEnvironmentVariable("REMINDER_TEMPLATE");
        private static string _statusTemplate = Environment.GetEnvironmentVariable("STATUS_TEMPLATE");
        private static string _adminNotificationTemplate = Environment.GetEnvironmentVariable("ADMIN_NOTIFICATION_TEMPLATE");
        private static string _notReverifiedTemplate = Environment.GetEnvironmentVariable("NOT_REVERIFIED_TEMPLATE");
        private static string _notApprovedTemplate = Environment.GetEnvironmentVariable("NOT_APPROVED_TEMPLATE");
        public NotifyGateway(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
            _client = new NotificationClient(connectionInfo.NotifyKey);
        }

        public async Task SendMessage(NotifyMessageTypes messageType, string[] addresses, string message)
        {
            if (messageType == null || addresses == null)
            {
                LoggingHandler.LogError("Notify request with invalid arguments");
                throw new ArgumentException("Notify request with invalid arguments");
            }
            var template = string.Empty;
            var personalisation = new Dictionary<string, dynamic>();
            switch (messageType)
            {
                case NotifyMessageTypes.Reminder:
                    template = _reminderTemplate;
                    break;
                case NotifyMessageTypes.AdminNotification:
                    template = _adminNotificationTemplate;
                    break;
                case NotifyMessageTypes.StatusUpdate:
                    template = _statusTemplate;
                    break;
                case NotifyMessageTypes.NotReverified:
                    template = _notReverifiedTemplate;
                    break;
                case NotifyMessageTypes.NotApproved:
                    template = _notApprovedTemplate;
                    personalisation.Add("status_message", message ?? "");
                    break;
            }

            try
            {
                for (int a = 0; a < addresses.Length; a++)
                {
                    await _client.SendEmailAsync(addresses[a], template, personalisation).ConfigureAwait(false);
                }
            }
            catch (NotifyClientException e)
            {
                LoggingHandler.LogError("Gov Notify send error");
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
            }
            catch (Exception e)
            {
                LoggingHandler.LogError(e.Message);
                LoggingHandler.LogError(e.StackTrace);
                throw;
            }
        }
    }
}
