﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public string GetSex(string name)
        {
            if(name == "Jonathan")
            {
                return "man";
            }
            return null;
        }

        [HttpGet]
        public int? GetAge(string name)
        {
            if (name == "Jonathan")
            {
                return 24;
            }
            return null;
        }
    }
}