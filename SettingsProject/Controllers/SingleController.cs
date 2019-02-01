using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SettingsContracts.DatabaseModels;
using SettingsProject.Attributes;

namespace SettingsProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class SingleController : ControllerBase
    {
        #region GETs

        [HttpGet]
        [Route("/single/{gpid}")]
        [ValidateModelState]
        public virtual ActionResult<Grandparent> GetGrandparent([FromRoute][Required]string gpid)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/single/{gpid}/{pid}")]
        [ValidateModelState]
        public virtual ActionResult<Parent> GetParent([FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid)
        {
            return Ok();
        }
        [HttpGet]
        [Route("/single/{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public virtual ActionResult<Child> GetChild([FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            return Ok();
        }

        [HttpGet]
        [Route("/single/{gpid}/{pid}/{cid}/{gcid}")]
        [ValidateModelState]
        public virtual ActionResult<Grandchild> GetGrandchild([FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, 
            [FromRoute][Required]string gcid)
        {
            return Ok();
        }
        #endregion

        #region POSTs

        [HttpPost]
        [Route("/single/{gpid}")]
        [ValidateModelState]
        public virtual IActionResult PostGrandparent([FromBody][Required]Grandparent body)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/single/{gpid}/{pid}")]
        [ValidateModelState]
        public virtual IActionResult PostParent([FromBody][Required]Parent body)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/single/{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public virtual IActionResult PostChild([FromBody][Required]Child body)
        {
            return Ok();
        }

        [HttpPost]
        [Route("/single/{gpid}/{pid}/{cid}/{gcid}")]
        [ValidateModelState]
        public virtual IActionResult PostGrandchild([FromBody][Required]Grandchild body)
        {
            return Ok();
        }
        #endregion
    }
}
