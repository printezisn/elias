using Elias.Web.App_Start;
using Elias.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Elias.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Removes unused view engines
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new SCIViewEngine());

            // Adds application model binders
            ModelBinders.Binders.Add(typeof(int), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(int?), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(byte), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(byte?), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(float), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(float?), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(double), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime), new SCIModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new SCIModelBinder());
        }
    }
}
