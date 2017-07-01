using Elias.DAL.Entities;
using Elias.DAL.Enums;
using Elias.DAL.Repository;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Elias.Web.Code
{
    public static class EmployeeHelper
    {
        public static void SetReservedDays(IDataRepository db, Employee employee)
        {
            var date = new DateTime(DateTime.UtcNow.Year, 1, 1);

            employee.ReservedDays = db.GetLeaveRequests()
                    .Where(w =>
                        w.EmployeeId == employee.Id &&
                        w.StatusId == (byte)LeaveRequestStatusEnum.Accepted &&
                        w.FromDate >= date && date <= w.ToDate
                    )
                    .Sum(sum => (int?)sum.TotalDays) ?? 0;
        }

        public static async Task SendMessage(Employee employee, string text)
        {
            if (string.IsNullOrEmpty(employee.ServiceUrl) || string.IsNullOrEmpty(employee.BotId) || string.IsNullOrEmpty(employee.LastUsedId))
            {
                return;
            }

            var userAccount = new ChannelAccount(name: employee.FirstName, id: employee.LastUsedId);
            var connector = new ConnectorClient(new Uri(employee.ServiceUrl));
            var botAccount = new ChannelAccount(employee.BotId);
            var conversationId = await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount);

            IMessageActivity message = Activity.CreateMessageActivity();
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId.Id);
            message.Text = text;
            message.Locale = "en-Us";

            await connector.Conversations.SendToConversationAsync((Activity)message);
        }
    }
}