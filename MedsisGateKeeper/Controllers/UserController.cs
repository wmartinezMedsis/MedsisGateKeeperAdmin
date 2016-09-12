using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MedsisGateKeeper.Controllers
{
    public class UserController : ApiController
    {
        public HttpResponseMessage Get(String id = null)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "We are running!" });
        }

        public HttpResponseMessage Post()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "We are running!" });
        }

        public HttpResponseMessage Put()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "We are running!" });
        }

        public HttpResponseMessage Delete()
        {
            var test = new List<String> { "Lora", "Wilmeer" };
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "We are running!", jsonResponse = JsonConvert.SerializeObject(test) });
        }
    }
}
