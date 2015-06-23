using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using App.Models;
using Newtonsoft.Json.Linq;

namespace App.Controllers
{
    public class ExampleController : ApiController
    {
        private readonly Repo repository = WebApiApplication.repository;
        
        [Route("Members"), HttpGet]
        public object AllMembers()
        {
            return repository.AllMembers();
        }

        [Route("Member/{name}"), HttpGet]
        public object AllMembers(string name)
        {
            return repository.AllMembers().Where(e => e.Name == name);
        }

        [Route("Member"), HttpPost]
        public HttpStatusCode AddNewMember([FromBody]JObject json)
        {
            var members = repository.AllMembers();
            if (members.Any(e => e.Name == json["Name"].Value<string>()))
                return HttpStatusCode.Found;
            members.Add(new Member() {Name = json["Name"].Value<string>(), Age = json["Age"].Value<int>() });
            return HttpStatusCode.OK;
        }
    }
}
