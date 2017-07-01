using Elias.DAL.Entities;
using Elias.DAL.Repository;
using Elias.Web.Code;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Elias.Web.Dialogs
{
    [LuisModel("b3fb3f2c-1f25-410f-91a4-7b84a15e0186", "76ea1f95318741e19f7632f86826578c")]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
       
        #region Intents
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None (IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"None");
        }

        [LuisIntent("Communication.Confirm")]
        public async Task Confirm(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Confirm");
        }

        [LuisIntent("Deny")]
        public async Task Deny(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Deny");
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"help");
        }

        [LuisIntent("Identification")]
        public async Task Identification(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Identification");
        }

        [LuisIntent("RequestLeave")]
        public async Task RequestLeave(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"request leave");
        }

        [LuisIntent("RequestRemaining")]
        public async Task RequestRemaining(IDialogContext context, LuisResult result)
        {
            using (var db = new DataRepository())
            {
                var employee = db.GetEmployees(false).FirstOrDefault(e => e.SkypeId == context.Activity.From.Id);
                if (employee != null)
                {
                    EmployeeHelper.SetReservedDays(db, employee);
                    int daysRemaining = employee.LeaveDays - employee.ReservedDays > 0 ? employee.LeaveDays - employee.ReservedDays : 0;
                    await context.PostAsync($"You have {daysRemaining}. Don't spend them all at once ;)");
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