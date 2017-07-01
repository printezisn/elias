using Elias.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Elias.Web.Code.Extensions
{
    public static class HtmlExtensions
    {
        private const string ASSETS_FOLDER = "/Content/build/";

        #region Assets

        /// <summary>
        /// Reads and returns the names of the asset files
        /// </summary>
        /// <returns>A list with the asset file names</returns>
        private static IEnumerable<string> ReadAssets()
        {
            string path = HttpContext.Current.Server.MapPath($"~{ASSETS_FOLDER}");
            var directoryInfo = new DirectoryInfo(path);

            return directoryInfo.GetFiles()
                .Where(w => w.Name.EndsWith(".css") || w.Name.EndsWith(".js"))
                .Select(s => s.Name);
        }

        /// <summary>
        /// Returns the names of the already built asset files in the application
        /// </summary>
        /// <returns>A list with the names of the assets found</returns>
        private static IEnumerable<string> GetAssets()
        {
#if DEBUG
            return ReadAssets();
#else
            if (!SCICacheHelper.Exists(AppConstants.ASSETS_CACHE_KEY))
            {
                var files = ReadAssets();
                SCICacheHelper.Add(AppConstants.ASSETS_CACHE_KEY, files, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable);
            }

            return SCICacheHelper.Get<IEnumerable<string>>(AppConstants.ASSETS_CACHE_KEY);
#endif
        }

        /// <summary>
        /// Returns the link to an asset file, taking into account the hash
        /// </summary>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="name">The name of the asset file</param>
        /// <param name="extension">The extension of the asset file</param>
        /// <returns>The link to the asset file</returns>
        public static string AssetLink(this HtmlHelper htmlHelper, string name, string extension)
        {
            var assets = GetAssets();
            foreach (var asset in assets)
            {
                string regex = "^" + name + @"-\w+\." + extension + "$";
                if (Regex.IsMatch(asset, regex))
                {
                    return ASSETS_FOLDER + asset;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the link to a css file, taking into account the hash
        /// </summary>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="name">The name of the css file</param>
        /// <returns>The link to the css file</returns>
        public static string CssLink(this HtmlHelper htmlHelper, string name)
        {
            return AssetLink(htmlHelper, name, "min.css");
        }

        /// <summary>
        /// Returns the link to a js file, taking into account the hash
        /// </summary>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="name">The name of the js file</param>
        /// <returns>The link to the js file</returns>
        public static string JsLink(this HtmlHelper htmlHelper, string name)
        {
            return AssetLink(htmlHelper, name, "min.js");
        }

        #endregion

        /// <summary>
        /// Renders a table header that is used for sorting the entities of the table
        /// </summary>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="displayHtmlString">The string to display as the header value</param>
        /// <param name="sortingField">The field to sort by</param>
        /// <param name="currentSortingField">(Optional) The field that is currently used for sorting</param>
        /// <param name="isCurrentlyAsc">(Optional) Indicates if the current sorting process is ascending or descending</param>
        /// <returns>The generated table header string</returns>
        public static MvcHtmlString SCISortHeader(this HtmlHelper htmlHelper, string displayHtmlString, string sortingField, string currentSortingField = null, bool? isCurrentlyAsc = null)
        {
            sortingField = sortingField.ToLower();
            if (string.IsNullOrWhiteSpace(currentSortingField))
            {
                currentSortingField = (HttpContext.Current.Request.QueryString["sortBy"] ?? string.Empty).ToLower();
            }
            if (!isCurrentlyAsc.HasValue)
            {
                isCurrentlyAsc = (HttpContext.Current.Request.QueryString["isAsc"] ?? string.Empty).ToLower() == "true";
            }

            bool isAsc = (currentSortingField != sortingField || !isCurrentlyAsc.Value);

            var newQueryString = HttpUtility.ParseQueryString(HttpContext.Current.Request.QueryString.ToString());
            newQueryString.Remove("_");
            newQueryString.Set("sortBy", sortingField);
            newQueryString.Set("isAsc", isAsc.ToString());
            string newUrl = HttpContext.Current.Request.Url.AbsolutePath + "?" + newQueryString;

            TagBuilder tableHeader = new TagBuilder("th");
            if (currentSortingField == sortingField)
            {
                tableHeader.AddCssClass(isCurrentlyAsc.Value ? "sorting_asc" : "sorting_desc");
            }
            else
            {
                tableHeader.AddCssClass("sorting");
            }

            TagBuilder link = new TagBuilder("a");
            link.MergeAttribute("href", newUrl);
            link.InnerHtml = displayHtmlString;

            tableHeader.InnerHtml = link.ToString();

            return new MvcHtmlString(tableHeader.ToString());
        }

        /// <summary>
        /// Renders a table header that is used for sorting the entities of the table
        /// </summary>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="displayHtmlString">The html string to display as the header value</param>
        /// <param name="sortingField">The field to sort by</param>
        /// <param name="currentSortingField">(Optional) The field that is currently used for sorting</param>
        /// <param name="isCurrentlyAsc">(Optional) Indicates if the current sorting process is ascending or descending</param>
        /// <returns>The generated table header string</returns>
        public static MvcHtmlString SCISortHeader(this HtmlHelper htmlHelper, MvcHtmlString displayHtmlString, string sortingField, string currentSortingField = null, bool? isCurrentlyAsc = null)
        {
            return SCISortHeader(htmlHelper, displayHtmlString.ToHtmlString(), sortingField, currentSortingField, isCurrentlyAsc);
        }

        /// <summary>
        /// Renders a label that fulfils the requirements of the application's UI
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model's property</typeparam>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <param name="htmlAttributes">(Optional) HTML attributes for the label element</param>
        /// <param name="isReadOnly">(Optional) Indicates if the label is for a read-only property or not</param>
        /// <param name="isRequiredOverride">(Optional) Overrides the indication of the property is required or not</param>
        /// <returns>The generated label string</returns>
        public static MvcHtmlString SCILabelFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, bool isReadOnly = false, bool? isRequiredOverride = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return SCILabelFor(htmlHelper, expression, metadata.DisplayName ?? metadata.PropertyName, htmlAttributes, isReadOnly, isRequiredOverride);
        }

        /// <summary>
        /// Renders a label that fulfils the requirements of the application's UI
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model's property</typeparam>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <param name="labelText">The text to add in the label element</param>
        /// <param name="htmlAttributes">(Optional) HTML attributes for the label element</param>
        /// <param name="isReadOnly">(Optional) Indicates if the label is for a read-only property or not</param>
        /// <param name="isRequiredOverride">(Optional) Overrides the indication of the property is required or not</param>
        /// <returns>The generated label string</returns>
        public static MvcHtmlString SCILabelFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes = null, bool isReadOnly = false, bool? isRequiredOverride = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (string.IsNullOrWhiteSpace(labelText))
            {
                return MvcHtmlString.Empty;
            }

            RouteValueDictionary parameters = (htmlAttributes != null)
                ? new RouteValueDictionary(htmlAttributes)
                : new RouteValueDictionary();
            if (!parameters.ContainsKey("class"))
            {
                parameters["class"] = string.Empty;
            }

            bool isRequired = isRequiredOverride.HasValue ? isRequiredOverride.Value : metadata.IsRequired;

            if (isReadOnly)
            {
                labelText += ":";
            }
            else if (isRequired && metadata.ModelType != typeof(bool))
            {
                parameters["class"] = parameters["class"].ToString() + " required-field";
            }

            if (!parameters.ContainsKey("for"))
            {
                string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
                parameters["for"] = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);
            }

            TagBuilder labelTagBuilder = new TagBuilder("label");
            labelTagBuilder.MergeAttributes(parameters);
            labelTagBuilder.SetInnerText(labelText);

            return new MvcHtmlString(labelTagBuilder.ToString());
        }

        /// <summary>
        /// Renders a numeric text box
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model's property</typeparam>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <param name="htmlAttributes">(Optional) Additional html attributes for the text box element</param>
        /// <returns>The generated numeric text box string</returns>
        public static MvcHtmlString SCINumericTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            RouteValueDictionary parameters = (htmlAttributes != null)
                ? new RouteValueDictionary(htmlAttributes)
                : new RouteValueDictionary();
            parameters["data-val-number"] = AppGlobalMessages.FieldValueMustBeNumber;

            return InputExtensions.TextBoxFor(htmlHelper, expression, parameters);
        }

        /// <summary>
        /// Renders an integer text box
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model's property</typeparam>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <param name="htmlAttributes">(Optional) Additional html attributes for the text box element</param>
        /// <returns>The generated integer text box string</returns>
        public static MvcHtmlString SCIIntegerTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            RouteValueDictionary parameters = (htmlAttributes != null)
                ? new RouteValueDictionary(htmlAttributes)
                : new RouteValueDictionary();
            parameters["data-val-number"] = AppGlobalMessages.FieldValueMustBeInteger;
            parameters["type"] = "number";

            return InputExtensions.TextBoxFor(htmlHelper, expression, parameters);
        }

        /// <summary>
        /// Renders a date text box
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model's property</typeparam>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <param name="htmlAttributes">(Optional) Additional html attributes for the text box element</param>
        /// <returns>The generated date text box string</returns>
        public static MvcHtmlString SCIDateTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            DateTime? value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model as DateTime?;

            RouteValueDictionary parameters = (htmlAttributes != null)
                ? new RouteValueDictionary(htmlAttributes)
                : new RouteValueDictionary();
            parameters["type"] = "text";
            parameters["Value"] = value.HasValue ? value.Value.ToString("d") : string.Empty;

            string result = "<div class=\"input-group date sci-date-picker\">" +
                InputExtensions.TextBoxFor(htmlHelper, expression, parameters.ToDictionary(d => d.Key.Replace("_", "-"), d => d.Value)) +
                "<span class=\"input-group-btn\"><button class=\"btn default\" type=\"button\"><i class=\"fa fa-calendar\"></i></button></span></div>";

            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Returns the value of a model's property taking into account the model state
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model's property</typeparam>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <returns>The value of the model's property</returns>
        public static TValue GetModelValue<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            return (TValue)ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
        }

        /// <summary>
        /// Returns the validation attributes of a form control
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TValue">The type of the model's property</typeparam>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="expression">The lambda expression to get the property</param>
        /// <returns>The text with the validation attributes</returns>
        public static MvcHtmlString SCIValidationAttributes<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            var validationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(expressionText);

            return new MvcHtmlString(string.Join(" ", validationAttributes.Select(s => s.Key + "=\"" + s.Value + "\"")));
        }

        /// <summary>
        /// Returns the return url in a page or a default url if a return url does not exist
        /// </summary>
        /// <param name="htmlHelper">The html helper</param>
        /// <param name="defaultUrl">The default url</param>
        /// <returns>The return url or the default url if a return url does not exist</returns>
        public static string ReturnUrl(this HtmlHelper htmlHelper, string defaultUrl)
        {
            if (htmlHelper.ViewBag.ReturnUrl != null)
            {
                defaultUrl = htmlHelper.ViewBag.ReturnUrl.ToString();
            }

            return defaultUrl;
        }
    }
}