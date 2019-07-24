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
    public class CounterController : ControllerBase
    {
        private static int _count = 0;

        [HttpGet]
        public string Count()
        {
            return $"count {++_count} from WebApiA";
        }
    }
}