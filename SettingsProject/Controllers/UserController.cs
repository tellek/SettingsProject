using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettingsProject.Attributes;

namespace SettingsProject.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("/user/{uid}")]
        [ValidateModelState]
        public virtual IActionResult GetUser()
        {
            return Ok();
        }

        [HttpPost]
        [Route("/user/{uid}")]
        [ValidateModelState]
        public virtual IActionResult PostUser()
        {
            return Ok();
        }
    }
}