using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using SettingsProject.Attributes;
using SettingsProject.Managers;

namespace SettingsProject.Controllers
{
    [Route("settings/{accountId}")]
    [Produces("application/json")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IManager<Grandparent> gpManager;
        private readonly IManager<Parent> pManager;
        private readonly IManager<Child> cManager;
        private readonly IManager<Grandchild> gcManager;

        public SettingsController(IManager<Grandparent> gpManager, IManager<Parent> pManager,
            IManager<Child> cManager, IManager<Grandchild> gcManager)
        {
            this.gpManager = gpManager;
            this.pManager = pManager;
            this.cManager = cManager;
            this.gcManager = gcManager;
        }

        #region GET

        /// <summary>
        /// Get a Level 1 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Grandparent>> GetGrandparent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await gpManager.GetSettingAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        /// <summary>
        /// Get a Level 2 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Parent>> GetParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromRoute][Required]long pid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await pManager.GetSettingAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        /// <summary>
        /// Get a Level 3 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Child>> GetChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromRoute][Required]long pid, [FromRoute][Required]long cid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await cManager.GetSettingAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        /// <summary>
        /// Get a Level 4 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <param name="gcid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<Grandchild>> GetGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromRoute][Required]long gcid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, gcid);
            var result = await gcManager.GetSettingAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }
        #endregion

        #region GET SEARCH

        /// <summary>
        /// Get a list of Level 1 Settings.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Grandparent>>> GetGrandparents([FromRoute][Required]int accountId)
        {
            var pData = InitiateProcessData(accountId, 0, 0, 0, 0);
            var result = await gpManager.GetSettingsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        /// <summary>
        /// Get a list of Level 2 Settings.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Parent>>> GetParents([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await pManager.GetSettingsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        /// <summary>
        /// Get a list of Level 3 Settings.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gpid}/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Child>>> GetChildren([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromRoute][Required]long pid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await cManager.GetSettingsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }

        /// <summary>
        /// Get a list of Level 4 Settings.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult<List<Grandchild>>> GetGrandchildren([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromRoute][Required]long pid, [FromRoute][Required]long cid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await gcManager.GetSettingsAsync(pData);
            return StatusCode(result.Item1, result.Item2);
        }
        #endregion

        #region PATCH

        /// <summary>
        /// Update a Level 1 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchGrandparent([FromRoute][Required]int accountId, 
            [FromRoute][Required]long gpid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await gpManager.UpdateSettingAsync(pData, body);
            return StatusCode(result);
        }

        /// <summary>
        /// Update a Level 2 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await pManager.UpdateSettingAsync(pData, body);
            return StatusCode(result);
        }

        /// <summary>
        /// Update a Level 3 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await cManager.UpdateSettingAsync(pData, body);
            return StatusCode(result);
        }

        /// <summary>
        /// Update a Level 4 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <param name="gcid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PatchGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromRoute][Required]long gcid, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, gcid);
            var result = await gcManager.UpdateSettingAsync(pData, body);
            return StatusCode(result);
        }
        #endregion

        #region POST

        /// <summary>
        /// Update existing Level 1 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostGrandparent([FromRoute][Required]int accountId, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, 0, 0, 0, 0);
            var result = await gpManager.CreateSettingAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }

        /// <summary>
        /// Update existing Level 2 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await pManager.CreateSettingAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }

        /// <summary>
        /// Update existing Level 3 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gpid}/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await cManager.CreateSettingAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }

        /// <summary>
        /// Update existing Level 4 Setting.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gpid}/{pid}/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> PostGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid, [FromBody][Required]SettingsOnly body)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await gcManager.CreateSettingAsync(pData, body);
            return StatusCode(result.Item1, $"Created: {result.Item2}");
        }
        #endregion

        #region DELETE

        /// <summary>
        /// Delete a Level 1 Setting permanently.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("id/{gpid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteGrandparent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid)
        {
            var pData = InitiateProcessData(accountId, gpid, 0, 0, 0);
            var result = await gpManager.DeleteSettingAsync(pData);
            return StatusCode(result);
        }

        /// <summary>
        /// Delete a Level 2 Setting permanently.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{gpid}/id/{pid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteParent([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, 0, 0);
            var result = await pManager.DeleteSettingAsync(pData);
            return StatusCode(result);
        }

        /// <summary>
        /// Delete a Level 3 Setting permanently.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{gpid}/{pid}/id/{cid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteChild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, 0);
            var result = await cManager.DeleteSettingAsync(pData);
            return StatusCode(result);
        }

        /// <summary>
        /// Delete a Level 4 Setting permanently.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gpid"></param>
        /// <param name="pid"></param>
        /// <param name="cid"></param>
        /// <param name="gcid"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{gpid}/{pid}/{cid}/id/{gcid}")]
        [ValidateModelState]
        public async virtual Task<ActionResult> DeleteGrandchild([FromRoute][Required]int accountId, [FromRoute][Required]long gpid,
            [FromRoute][Required]long pid, [FromRoute][Required]long cid,
            [FromRoute][Required]long gcid)
        {
            var pData = InitiateProcessData(accountId, gpid, pid, cid, gcid);
            var result = await gcManager.DeleteSettingAsync(pData);
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

        private ProcessData InitiateProcessData(string aid, string gpid, string pid, string cid, string gcid)
        {
            var n_aid = TryToParseToLong(aid);
            var n_gpid = TryToParseToLong(gpid);
            var n_pid = TryToParseToLong(pid);
            var n_cid = TryToParseToLong(cid);
            var n_gcid = TryToParseToLong(gcid);

            return new ProcessData
            {
                // Only populate data that is numerical.
                AccountId = (n_aid.isNotNull && n_aid.isLong) ? n_aid.value : null,
                Gpid = (n_gpid.isNotNull && n_gpid.isLong) ? n_gpid.value : null,
                Pid = (n_pid.isNotNull && n_pid.isLong) ? n_pid.value : null,
                Cid = (n_cid.isNotNull && n_cid.isLong) ? n_cid.value : null,
                Gcid = (n_gcid.isNotNull && n_gcid.isLong) ? n_gcid.value : null,
                // Only populate data that is a string.
                AccountName = (n_aid.isNotNull && n_aid.isString) ? n_aid.value : null,
                Gpname = (n_gpid.isNotNull && n_gpid.isString) ? n_gpid.value : null,
                Pname = (n_pid.isNotNull && n_pid.isString) ? n_pid.value : null,
                Cname = (n_cid.isNotNull && n_cid.isString) ? n_cid.value : null,
                Gcname = (n_gcid.isNotNull && n_gcid.isString) ? n_gcid.value : null
            };
        }

        private static (bool isNotNull, bool isString, bool isLong, dynamic value) TryToParseToLong(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return (false, false, false, null);
            bool success = Int64.TryParse(value, out long number);
            if (success) return (true, false, true, number);
            else return (true, true, false, value);
        }
    }
}
