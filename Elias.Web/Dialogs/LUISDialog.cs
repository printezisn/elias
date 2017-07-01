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
            await context.PostAsync($"request remaining");
        }
       
        #endregion
    }
}