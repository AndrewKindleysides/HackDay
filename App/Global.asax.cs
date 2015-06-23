using System.Collections.Generic;
using System.Web.Http;
using App.Models;

namespace App
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Repo repository;
 
        protected void Application_Start()
        {
            repository= new Repo();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
