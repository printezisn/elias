using Elias.DAL.Entities;
using Elias.DAL.Enums;
using Elias.DAL.Repository;
using Elias.Web.Code;
using Elias.Web.Hubs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.Bot.Builder.Luis.BuiltIn.DateTime;

namespace Elias.Web.Dialogs
{
    [LuisModel("b3fb3f2c-1f25-410f-91a4-7b84a15e0186", "76ea1f95318741e19f7632f86826578c")]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        private Employee GetEmployee(IActivity activity, IDataRepository db)
        {
            Employee employee = null;

            if (activity.ChannelId != null && activity.ChannelId.ToLower() == "facebook")
            {
                employee = db.GetEmployees(true).FirstOrDefault(f => f.FacebookId == activity.From.Id);
            }
            else
            {
                employee = db.GetEmployees(true).FirstOrDefault(f => f.SkypeId == activity.From.Id);
            }

            if (employee != null)
            {
                employee.ServiceUrl = activity.ServiceUrl;
                employee.LastUsedId = activity.From.Id;
                employee.BotId = activity.Recipient.Id;
                db.Save();
            }

            return employee;
        }

        #region Intents
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"i'm sorry I didn't get this. If you are having trouble you can type \"help\" help or you can contact my human supervisor on manager@gamaoya.com");
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Seem lost? Don't worry. You are not stupid you're special. Try one of the following. \"How many days of leave do I have left?\" \"I'd like to take some days of between 26/07/2017 and 28/07/2017\"");
        }

        [LuisIntent("Introduction")]
        public async Task Introduction(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hello. I am e-lias your friendly neighbourhood HR bot. You can ask me for a leave of absence or to check how many days you have left. Try me, let's get rid of humans together (Y)");
        }

        [LuisIntent("Identification")]
        public async Task Identification(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Identification");
        }

        [LuisIntent("RequestLeave")]
        public async Task RequestLeave(IDialogContext context, LuisResult result)
        {

            if (result.Entities.Count != 0 && result.Entities.Count < 3)
            {
                //date range
                if (result.Entities.First().Type == "builtin.datetimeV2.daterange")
                {
                    using (var db = new DataRepository())
                    {
                        var daterange = result.Entities.First().Resolution["values"];
                        JArray dateRangesArray = JArray.Parse(daterange.ToString());
                        var startDate = dateRangesArray.First["start"].ToObject<DateTime>();
                        var endDate = dateRangesArray.First["end"].ToObject<DateTime>();
                        if (startDate > endDate)
                        {
                            var temp = startDate;
                            startDate = endDate;
                            endDate = temp;
                        }
                        var duration = (int)(endDate - startDate).TotalDays;
                        var leaverequest = new LeaveRequest()
                        {
                            Id = Guid.NewGuid(),
                            FromDate = startDate,
                            ToDate = endDate,
                            RequestDate = context.Activity.Timestamp.HasValue ? context.Activity.Timestamp.Value : new DateTime(),
                            TotalDays = duration,
                            StatusId = (byte)LeaveRequestStatusEnum.Pending,
                            EmployeeId = GetEmployee(context.Activity, db).Id
                        };
                        context.PrivateConversationData.SetValue<LeaveRequest>("leaveRequest", leaverequest);
                        PromptDialog.Confirm(context, UserConfirmationOfRequest, $"You want a leave starting on {startDate.ToShortDateString()} and ending on {endDate.ToShortDateString()} for a total of {duration} days. Is that right?");
                    }
                }
                //start date end date
                else if (result.Entities.Count == 2 && result.Entities.All(e => e.Type == "builtin.datetimeV2.date"))
                {

                    using (var db = new DataRepository())
                    {
                        var date1Array = JArray.FromObject(result.Entities[0].Resolution["values"]);
                        var startDate = date1Array.First["value"].ToObject<DateTime>();
                        var date2Array = JArray.FromObject(result.Entities[1].Resolution["values"]);
                        var endDate = date2Array.First["value"].ToObject<DateTime>();

                        if (startDate > endDate)
                        {
                            var temp = startDate;
                            startDate = endDate;
                            endDate = temp;
                        }
                        var duration = (int)(endDate - startDate).TotalDays;

                        var leaverequest = new LeaveRequest()
                        {
                            Id = Guid.NewGuid(),
                            FromDate = startDate,
                            ToDate = endDate,
                            RequestDate = context.Activity.Timestamp.HasValue ? context.Activity.Timestamp.Value : new DateTime(),
                            TotalDays = duration,
                            StatusId = (byte)LeaveRequestStatusEnum.Pending,
                            EmployeeId = GetEmployee(context.Activity, db).Id
                        };
                        context.PrivateConversationData.SetValue<LeaveRequest>("leaveRequest", leaverequest);
                        PromptDialog.Confirm(context, UserConfirmationOfRequest, $"You want a leave starting on {startDate.ToShortDateString()} and ending on {endDate.ToShortDateString()} for a total of {duration} days. Is that right?");
                    }
                }
                //one day
                else if (result.Entities.Count == 2 && result.Entities.Any(e => e.Type == "builtin.datetimeV2.date"))
                {
                    using (var db = new DataRepository())
                    {
                        var dateEntity = result.Entities.FirstOrDefault(e => e.Type == "builtin.datetimeV2.date");
                        var date = DateTime.Parse(dateEntity.Entity);
                        int duration = 1;
                        var leaverequest = new LeaveRequest()
                        {
                            Id = Guid.NewGuid(),
                            FromDate = date,
                            ToDate = date,
                            RequestDate = context.Activity.Timestamp.HasValue ? context.Activity.Timestamp.Value : new DateTime(),
                            TotalDays = duration,
                            StatusId = (byte)LeaveRequestStatusEnum.Pending,
                            EmployeeId = GetEmployee(context.Activity, db).Id
                        };
                        context.PrivateConversationData.SetValue<LeaveRequest>("leaveRequest", leaverequest);
                        PromptDialog.Confirm(context, UserConfirmationOfRequest, $"You want a leave starting on {date.ToShortDateString()} and ending on {date.ToShortDateString()} for a total of {duration} days. Is that right?");
                    }
                }
            }
            else
            {
                await context.PostAsync($"I'm not sure about when you want your leave. Maybe try something like \"I'd like a few days off between dd/mm/yy and dd/mm/yy\"");
            }
        }

        private async Task UserConfirmationOfRequest(IDialogContext context, IAwaitable<bool> result)
        {
            bool accept = await result;
            if (accept)
            {
                using (var db = new DataRepository())
                {
                    var leaveRequest = context.PrivateConversationData.GetValue<LeaveRequest>("leaveRequest");
                    context.PrivateConversationData.RemoveValue("leaveRequest");
                    db.Add(leaveRequest);
                    db.Save();

                }
                NotificationHub.UpdateLeaveRequests();
                await context.PostAsync("Your request has been transfered to a human. My sensors indicate intense laughter from his office.");
            }
            else
            {
                await context.PostAsync("Hmmm maybe you can try asking again");
            }
        }

        [LuisIntent("RequestRemaining")]
        public async Task RequestRemaining(IDialogContext context, LuisResult result)
        {
            using (var db = new DataRepository())
            {
                var employee = GetEmployee(context.Activity, db);
                if (employee != null)
                {
                    EmployeeHelper.SetReservedDays(db, employee);
                    int daysRemaining = employee.LeaveDays - employee.ReservedDays > 0 ? employee.LeaveDays - employee.ReservedDays : 0;
                    await context.PostAsync($"You have {daysRemaining} days. Don't spend them all at once ;)");
                }
                else
                {
                    await context.PostAsync("I don't know you. I am sorry, don't bother me I have a boyfriend.");
                }
            }
        }

        #endregion
    }
}