using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using App.Models;
using Newtonsoft.Json.Linq;

namespace App.Controllers
{
    public class ExampleController : ApiController
    {
        private readonly Repo repository = WebApiApplication.repository;
        
        [Route("move"), HttpGet]
        public string AllMembers()
        {
            return "ROCK";
        }

        [Route("start"), HttpPost]
        public HttpStatusCode Start([FromBody]JObject json)
        {
            return HttpStatusCode.OK;
        }

        [Route("move"), HttpPost]
        public HttpStatusCode Move([FromBody]JObject json)
        {
            return HttpStatusCode.OK;
        }
    }
}
