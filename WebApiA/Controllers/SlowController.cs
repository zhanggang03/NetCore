using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SlowController : ControllerBase
    {
        [HttpGet]
        public async Task<string> GetName()
        {
            await Task.Delay(6000);
            return "Jonathan";
        }
    }
}