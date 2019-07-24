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
    public class TimeController : ControllerBase
    {
        [HttpGet]
        public string GetNow()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }
    }
}