using System.Collections.Generic;
using System.Web.Http;
using App.Controllers;
using App.Models;

namespace App
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Game Game;
        public static int CurrentRound;
 
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
