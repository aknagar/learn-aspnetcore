using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Route.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/heartbeat")]
    public class HeartbeatController : Controller
    {
        public Task<string> Get()
        {
            return Task.FromResult("Hi");
        }
    }
}