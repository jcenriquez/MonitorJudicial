using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace MonitorJudicial
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            // Redirige a la página de inicio específica si la URL es solo el nombre del sitio
            if (HttpContext.Current.Request.Url.AbsolutePath == "/")
            {
                HttpContext.Current.Response.Redirect("~/Views/Login.aspx");
            }
        }
    }
}