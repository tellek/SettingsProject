using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SettingsContracts.DatabaseModels;
using SettingsProject.Attributes;
using SettingsProject.Managers;
using SettingsProject.Managers.Interfaces;
using SettingsResources.DatabaseRepositories;

namespace SettingsProject.Controllers
{
    [Route("settings/{accountId}")]
    [Produces("application/json")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IGrandparentManager gpManager;
        private readonly IParentManager pManager;
        private readonly IChildManager cManager;
        private readonly IGrandchildManager cpManager;

        public SettingsController(IGrandparentManager gpManager, IParentManager pManager,
            IChildManager cManager, IGrandchildManager cpManager)
        {
            this.gpManager = gpManager;
            this.pManager = pManager;
            this.cManager = cManager;
            this.cpManager = cpManager;
        }

        #region GET

        [HttpGet]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Grandparent>> GetGrandparent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid)
        {
            var result = await gpManager.GetGrandparentAsync(accountId, gpid);
            return StatusCode(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public virtual ActionResult<Parent> GetParent([FromRoute][Required]int accountId, [FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid)
        {
            // TODO Impliment this
            return Ok();
        }
        [HttpGet]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public virtual ActionResult<Child> GetChild([FromRoute][Required]int accountId, [FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpGet]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public virtual ActionResult<Grandchild> GetGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, 
            [FromRoute][Required]string gcid)
        {
            // TODO Impliment this
            return Ok();
        }
        #endregion

        #region GET SEARCH

        [HttpGet]
        [Route("")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Grandparent>>> GetGrandparents([FromRoute][Required]int accountId)
        {
            var result = await gpManager.GetGrandparentsAsync(accountId);
            return StatusCode(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("{gpid}")]
        [ValidateModelState]
        public virtual IActionResult GetParents([FromRoute][Required]string gpid,
            [FromRoute][Required]string pid)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpGet]
        [Route("{gpid}/{pid}")]
        [ValidateModelState]
        public virtual IActionResult GetChildren([FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpGet]
        [Route("{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public virtual IActionResult GetGrandchildren([FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid,
            [FromRoute][Required]string gcid)
        {
            // TODO Impliment this
            return Ok();
        }
        #endregion

        #region PATCH

        [HttpPatch]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchGrandparent([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromBody][Required]Grandparent body)
        {
            var result = await gpManager.UpdateGrandparentAsync(accountId, gpid, body);
            return StatusCode(result);
        }

        [HttpPatch]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public virtual IActionResult PatchParent([FromRoute][Required]int accountId, [FromBody][Required]Parent body)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpPatch]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public virtual IActionResult PatchChild([FromRoute][Required]int accountId, [FromBody][Required]Child body)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpPatch]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public virtual IActionResult PatchGrandchild([FromRoute][Required]int accountId, [FromBody][Required]Grandchild body)
        {
            // TODO Impliment this
            return Ok();
        }
        #endregion

        #region POST

        [HttpPost]
        [Route("")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostGrandparent([FromRoute][Required]int accountId, [FromBody][Required]Grandparent body)
        {
            var result = await gpManager.CreateGrandparentAsync(accountId, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }

        [HttpPost]
        [Route("{gpid}")]
        [ValidateModelState]
        public virtual IActionResult PostParent([FromRoute][Required]int accountId, [FromBody][Required]Parent body)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpPost]
        [Route("{gpid}/{pid}")]
        [ValidateModelState]
        public virtual IActionResult PostChild([FromRoute][Required]int accountId, [FromBody][Required]Child body)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpPost]
        [Route("{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public virtual IActionResult PostGrandchild([FromRoute][Required]int accountId, [FromBody][Required]Grandchild body)
        {
            // TODO Impliment this
            return Ok();
        }
        #endregion

        #region DELETE

        [HttpDelete]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteGrandparent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid)
        {
            var result = await gpManager.DeleteGrandparentAsync(accountId, gpid);
            return StatusCode(result);
        }

        [HttpDelete]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public virtual ActionResult<Parent> DeleteParent([FromRoute][Required]int accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid)
        {
            // TODO Impliment this
            return Ok();
        }
        [HttpDelete]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public virtual ActionResult<Child> DeleteChild([FromRoute][Required]int accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            // TODO Impliment this
            return Ok();
        }

        [HttpDelete]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public virtual ActionResult<Grandchild> DeleteGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid,
            [FromRoute][Required]string gcid)
        {
            // TODO Impliment this
            return Ok();
        }
        #endregion
    }
}
