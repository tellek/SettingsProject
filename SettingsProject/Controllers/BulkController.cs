using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettingsProject.Attributes;

namespace SettingsProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class BulkController : ControllerBase
    {
        #region GETs

        [HttpGet]
        [Route("/bulk/{gpid}")]
        [ValidateModelState]
        public virtual IActionResult GetGrandparents([FromRoute][Required]string gpid)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/bulk/{gpid}/{pid}")]
        [ValidateModelState]
        public virtual IActionResult GetParents([FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/bulk/{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public virtual IActionResult GetChildren([FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/bulk/{gpid}/{pid}/{cid}/{gcid}")]
        [ValidateModelState]
        public virtual IActionResult GetGrandchildren([FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, 
            [FromRoute][Required]string gcid)
        {
            return Ok();
        }
        #endregion

        #region POSTs

        [HttpPost]
        [Route("/bulk/{gpid}")]
        [ValidateModelState]
        public virtual IActionResult PostGrandparents([FromBody][Required]string body)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/bulk/{gpid}/{pid}")]
        [ValidateModelState]
        public virtual IActionResult PostParents([FromBody][Required]string body)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/bulk/{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public virtual IActionResult PostChildren([FromBody][Required]string body)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/bulk/{gpid}/{pid}/{cid}/{gcid}")]
        [ValidateModelState]
        public virtual IActionResult PostGrandchildren([FromBody][Required]string body)
        {
            return Ok();
        }
        #endregion
    }
}