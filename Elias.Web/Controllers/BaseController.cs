using Elias.DAL.Entities;
using Elias.DAL.Repository;
using Elias.Web.Code;
using Elias.Web.Models;
using Elias.Web.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elias.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IDataRepository _db;

        protected User CurrentUser
        {
            get
            {
                return (User)Session[AppConstants.SESSION_CURRENT_USER_KEY];
            }
            set
            {
                Session[AppConstants.SESSION_CURRENT_USER_KEY] = value;
            }
        }

        /// <summary>
        /// Displays a message in the front end. It uses the TempData object to store the list of messages
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="messageType">The type of the message</param>
        /// <param name="title">The title of the message</param>
        protected void ShowMessage(string message, ToastrMessageTypeEnum messageType, string title = null)
        {
            ToastrMessage toastrMessage = new ToastrMessage(title, message, messageType);
            List<ToastrMessage> allMessages = (List<ToastrMessage>)TempData[AppConstants.APP_MESSAGES_TEMPDATA_KEY];
            if (allMessages == null)
            {
                allMessages = new List<ToastrMessage>();
            }

            allMessages.Add(toastrMessage);
            TempData[AppConstants.APP_MESSAGES_TEMPDATA_KEY] = allMessages;
        }

        /// <summary>
        /// Displays an error message in the front end. It uses the TempData object to store the list of errors
        /// </summary>
        /// <param name="errorMessage">The error message to display</param>
        protected void ShowPageError(string errorMessage)
        {
            List<string> allPageErrors = (List<string>)TempData[AppConstants.PAGE_ERRORS_TEMPDATA_KEY];
            if (allPageErrors == null)
            {
                allPageErrors = new List<string>();
            }

            allPageErrors.Add(errorMessage);
            TempData[AppConstants.PAGE_ERRORS_TEMPDATA_KEY] = allPageErrors;
        }

        /// <summary>
        /// Adds a model property error in the model state
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model property</typeparam>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <param name="errorMessage">The error message to add</param>
        protected void AddModelError<TModel, TValue>(Expression<Func<TModel, TValue>> expression, string errorMessage)
        {
            string property = ExpressionHelper.GetExpressionText(expression);
            ModelState.AddModelError(property, errorMessage);
        }

        /// <summary>
        /// Adds model property errors in the model state
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model property</typeparam>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <param name="errorMessages">The error messages to add</param>
        protected void AddModelErrors<TModel, TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<string> errorMessages)
        {
            string property = ExpressionHelper.GetExpressionText(expression);
            foreach (string errorMessage in errorMessages)
            {
                ModelState.AddModelError(property, errorMessage);
            }
        }

        /// <summary>
        /// Adds model property errors in the model state
        /// </summary>
        /// <param name="property">The name of the property/param>
        /// <param name="errorMessages">The error messages to add</param>
        protected void AddModelErrors(string property, IEnumerable<string> errorMessages)
        {
            foreach (string errorMessage in errorMessages)
            {
                ModelState.AddModelError(property, errorMessage);
            }
        }

        /// <summary>
        /// Removes the errors of a model property from the model state
        /// </summary>
        /// <param name="property">The model property</param>
        protected void RemoveModelErrors(string property)
        {
            if (ModelState.ContainsKey(property))
            {
                ModelState[property].Errors.Clear();
            }
        }

        /// <summary>
        /// Removes the errors of a model property from the model state
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model property</typeparam>
        /// <param name="expression">The lambda expression to get the model property</param>
        protected void RemoveModelErrors<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            string property = ExpressionHelper.GetExpressionText(expression);
            RemoveModelErrors(property);
        }

        /// <summary>
        /// Returns all the model state errors
        /// </summary>
        /// <returns>The list with the model state errors</returns>
        protected IEnumerable<string> GetModelStateErrors()
        {
            List<string> errors = new List<string>();
            foreach (var property in ModelState)
            {
                errors.AddRange(property.Value.Errors.Select(s => s.ErrorMessage));
            }

            return errors;
        }

        /// <summary>
        /// Redirects the user to a return url, if a local one exists, otherwise to a default action
        /// </summary>
        /// <param name="actionName">The name of the default action</param>
        /// <param name="controllerName">The name of the default controller</param>
        /// <param name="routeValues">The default route values</param>
        /// <returns>A redirect result if a return url exists, otherwise a redirect to action result</returns>
        protected ActionResult RedirectToReturnUrl(string actionName, string controllerName, object routeValues)
        {
            if (Request.Params.AllKeys.Contains("returnUrl") && Url.IsLocalUrl(Request.Params["returnUrl"]))
            {
                return Redirect(Request.Params["returnUrl"]);
            }

            return RedirectToAction(actionName, controllerName, routeValues);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            base.Initialize(requestContext);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();

            base.Dispose(disposing);
        }

        protected override ViewResult View(IView view, object model)
        {
            ViewBag.User = CurrentUser;
            if (Request.Params.AllKeys.Contains("returnUrl") && Url.IsLocalUrl(Request.Params["returnUrl"]))
            {
                ViewBag.ReturnUrl = Request.Params["returnUrl"];
            }

            return base.View(view, model);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            ViewBag.User = CurrentUser;
            if (Request.Params.AllKeys.Contains("returnUrl") && Url.IsLocalUrl(Request.Params["returnUrl"]))
            {
                ViewBag.ReturnUrl = Request.Params["returnUrl"];
            }

            return base.View(viewName, masterName, model);
        }

        protected override PartialViewResult PartialView(string viewName, object model)
        {
            ViewBag.User = CurrentUser;

            return base.PartialView(viewName, model);
        }

        public BaseController()
        {
            _db = new DataRepository();
        }
    }
}