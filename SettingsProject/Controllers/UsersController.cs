using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettingsProject.Attributes;

namespace SettingsProject.Controllers
{
    [Route("users/{accountId}")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Route("{uid}")]
        [ValidateModelState]
        public virtual IActionResult GetUser()
        {
            return Ok();
        }

        [HttpPatch]
        [Route("{uid}")]
        [ValidateModelState]
        public virtual IActionResult PatchUser()
        {
            return Ok();
        }

        [HttpPost]
        [Route("")]
        [ValidateModelState]
        public virtual IActionResult PostUser()
        {
            return Ok();
        }
    }
}