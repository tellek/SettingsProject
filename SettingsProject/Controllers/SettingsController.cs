using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
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
        private readonly IGrandchildManager gcManager;

        public SettingsController(IGrandparentManager gpManager, IParentManager pManager,
            IChildManager cManager, IGrandchildManager gcManager)
        {
            this.gpManager = gpManager;
            this.pManager = pManager;
            this.cManager = cManager;
            this.gcManager = gcManager;
        }

        #region GET

        [HttpGet]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Grandparent>> GetGrandparent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await gpManager.GetGrandparentAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Parent>> GetParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromRoute][Required]long pid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await pManager.GetParentAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }
        [HttpGet]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Child>> GetChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromRoute][Required]long pid, [FromRoute][Required]long cid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await cManager.GetChildAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Grandchild>> GetGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromRoute][Required]long gcid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, gcid);
            var result = await gcManager.GetGrandchildAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }
        #endregion

        #region GET SEARCH

        [HttpGet]
        [Route("")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Grandparent>>> GetGrandparents([FromRoute][Required]int accountId)
        {
            var pData = InitiateProcessData(accountId, 0, 0, 0, 0);
            var result = await gpManager.GetGrandparentsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Parent>>> GetParents([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromRoute][Required]long pid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await pManager.GetParentsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("{gpid}/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Child>>> GetChildren([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromRoute][Required]long pid, [FromRoute][Required]long cid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await cManager.GetChildsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Grandchild>>> GetGrandchildren([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromRoute][Required]long pid, [FromRoute][Required]long cid,
            [FromRoute][Required]long gcid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, gcid);
            var result = await gcManager.GetGrandchildsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }
        #endregion

        #region PATCH

        [HttpPatch]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchGrandparent([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await gpManager.UpdateGrandparentAsync(pData, body);
            return StatusCode(result);
        }

        [HttpPatch]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await pManager.UpdateParentAsync(pData, body);
            return StatusCode(result);
        }

        [HttpPatch]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await cManager.UpdateChildAsync(pData, body);
            return StatusCode(result);
        }

        [HttpPatch]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromRoute][Required]long gcid, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, gcid);
            var result = await gcManager.UpdateGrandchildAsync(pData, body);
            return StatusCode(result);
        }
        #endregion

        #region POST

        [HttpPost]
        [Route("")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostGrandparent([FromRoute][Required]int accountId, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, 0, 0, 0, 0);
            var result = await gpManager.CreateGrandparentAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }

        [HttpPost]
        [Route("{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await pManager.CreateParentAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }

        [HttpPost]
        [Route("{gpid}/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await cManager.CreateChildAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }

        [HttpPost]
        [Route("{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await gcManager.CreateGrandchildAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }
        #endregion

        #region DELETE

        [HttpDelete]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteGrandparent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await gpManager.DeleteGrandparentAsync(pData);
            return StatusCode(result);
        }

        [HttpDelete]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await pManager.DeleteParentAsync(pData);
            return StatusCode(result);
        }
        [HttpDelete]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await cManager.DeleteChildAsync(pData);
            return StatusCode(result);
        }

        [HttpDelete]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid,
            [FromRoute][Required]long gcid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, gcid);
            var result = await gcManager.DeleteGrandchildAsync(pData);
            return StatusCode(result);
        }
        #endregion

        private ProcessData InitiateProcessData(int aid, long gpid, long pid, long cid, long gcid)
        {
            return new ProcessData
            {
                AccountId = aid,
                Gpid = gpid,
                Pid = pid,
                Cid = cid,
                Gcid = gcid
            };
        }
    }
}
