using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettingsContracts.ApiTransaction;
using SettingsProject.Attributes;
using SettingsProject.Managers;

namespace SettingsProject.Controllers
{
    [Route("misc")]
    [Produces("application/json")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        private readonly IAuthManager authManager;

        public MiscController(IAuthManager authManager)
        {
            this.authManager = authManager;
        }

        [HttpPost]
        [Route("authenticate")]
        [ValidateModelState]
        public async virtual Task<ActionResult> AuthenticateUser([FromBody][Required]AuthProperties body)
        {
            var response = await authManager.DoAuthenticationAsync(body);

            return StatusCode(response.Item1, response.Item2);
        }
    }
}