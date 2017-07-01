using Elias.DAL.Entities;
using Elias.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Elias.Web.Code.Attributes
{
    /// <summary>
    /// Attribute used for authorization of users
    /// </summary>
    public class SCIAuthorizeAttribute : AuthorizeAttribute
    {
        public bool AllowAnonymous { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            using (IDataRepository db = new DataRepository())
            {
                return RunAuthorizeCore(httpContext, db);
            }
        }

        /// <summary>
        /// Returns the Id of the user's last session
        /// </summary>
        /// <param name="httpContext">The HttpContext object</param>
        /// <param name="db">The data repository instance</param>
        /// <param name="currentUser">The current logged-in user</param>
        /// <returns>The Id of the session or null if it doesn't exist</returns>
        private Guid? GetUserSession(HttpContextBase httpContext, IDataRepository db, User currentUser)
        {
            if (httpContext.Items[AppConstants.HTTP_CONTEXT_ITEMS_USER_SESSION_KEY] != null)
            {
                return (Guid?)httpContext.Items[AppConstants.HTTP_CONTEXT_ITEMS_USER_SESSION_KEY];
            }

            var user = db.GetUser(currentUser.Id);
            if (user != null)
            {
                httpContext.Items[AppConstants.HTTP_CONTEXT_ITEMS_USER_SESSION_KEY] = user.SessionId;
                return user.SessionId;
            }

            return null;
        }

        /// <summary>
        /// Signs the current logged-in user out
        /// </summary>
        /// <param name="httpContext">The HttpContext object</param>
        /// <param name="currentUser">The current logged-in user</param>
        /// <param name="authService">The authentication service instance</param>
        private void SignOut(HttpContextBase httpContext, User currentUser)
        {
            if (currentUser != null)
            {
                httpContext.Session.Remove(AppConstants.SESSION_CURRENT_USER_KEY);
                httpContext.Session.Abandon();
            }
            if (httpContext.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
        }

        public bool RunAuthorizeCore(HttpContextBase httpContext, IDataRepository db)
        {
            User currentUser = (User)httpContext.Session[AppConstants.SESSION_CURRENT_USER_KEY];

            // Checks if credentials are valid
            if (currentUser == null && httpContext.User.Identity.IsAuthenticated)
            {
                SignOut(httpContext, currentUser);
                return false;
            }
            if (!httpContext.User.Identity.IsAuthenticated && currentUser != null)
            {
                SignOut(httpContext, currentUser);
                return false;
            }
            if (currentUser != null && httpContext.User.Identity.IsAuthenticated && currentUser.SessionId.ToString() != httpContext.User.Identity.Name)
            {
                SignOut(httpContext, currentUser);
                return false;
            }
            if (currentUser != null)
            {
                // Concurrent sessions are not allowed
                if (currentUser.SessionId != GetUserSession(httpContext, db, currentUser))
                {
                    SignOut(httpContext, currentUser);
                    return false;
                }
            }

            if (this.AllowAnonymous)
            {
                return true;
            }

            if (currentUser == null)
            {
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RunHandleUnauthorizedRequest(filterContext);
        }

        public void RunHandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                // Child actions cannot do redirects
                filterContext.Result = new ContentResult() { Content = string.Empty };
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // Returns 403 error in case of an ajax request
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "auth", action = "login", returnUrl = filterContext.HttpContext.Request.RawUrl }));
            }
        }
    }
}