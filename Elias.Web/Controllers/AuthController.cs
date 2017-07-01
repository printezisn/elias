using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elias.DAL.Repository;
using Elias.Web.Code.Attributes;
using Elias.Web.Models.Auth;
using Elias.DAL.Entities;
using Elias.Shared.Helpers;
using System.Web.Security;
using Elias.Web.Models.Enums;
using Elias.Web.Code;

namespace Elias.Web.Controllers
{
    public class AuthController : BaseController
    {
        [SCIAuthorize(AllowAnonymous = true)]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Checks if the provided login credentials are valid
        /// </summary>
        /// <param name="model">The login model</param>
        /// <param name="user">The user entity object</param>
        /// <returns>True if the credentials are valid, otherwise false</returns>
        private bool CheckLoginCredentials(LoginModel model, User user)
        {
            // The user doesn't exist
            if (user == null)
            {
                return false;
            }

            return user.Password == PasswordHelper.HashPasswordWithSalt(model.Password, user.PasswordSalt);
        }

        /// <summary>
        /// Logs in the user
        /// </summary>
        /// <param name="model">The login model</param>
        /// <param name="returnUrl">The url to return</param>
        /// <returns>A redirect result if the authentication is successful, otherwise the login page view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SCIAuthorize(AllowAnonymous = true)]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = _db.GetUser(model.Username);

                // Check if credentials are correct
                if (!CheckLoginCredentials(model, user))
                {
                    ModelState.AddModelError(string.Empty, "The username or password that you entered are incorrect.");
                    return View(model);
                }

                // Authenticate the user by using both session and forms authentication in order
                // to prevent session fixation
                user.SessionId = Guid.NewGuid();
                _db.Save();

                this.CurrentUser = user;
                FormsAuthentication.SetAuthCookie(user.SessionId.ToString(), false);

                this.ShowMessage("You have successfully logged in", ToastrMessageTypeEnum.Success);

                return RedirectToReturnUrl("index", "home", null);
            }
            catch (Exception ex)
            {
                this.ShowMessage(AppGlobalMessages.UnexpectedErrorMessage, ToastrMessageTypeEnum.Error);

                return View(model);
            }
        }

        /// <summary>
        /// Logs the current user out
        /// </summary>
        /// <returns>A redirect result to the login page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            if (this.CurrentUser != null)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
            }

            return RedirectToAction("login");
        }
    }
}