using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SettingsContracts;
using SettingsContracts.ApiTransaction;
using SettingsContracts.DatabaseModels;
using SettingsProject.Attributes;
using SettingsProject.Helpers;
using SettingsProject.Managers;

namespace SettingsProject.Controllers
{
    [Route("settings/{accountId}")]
    [Produces("application/json")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsManager<Grandparent> gpManager;
        private readonly ISettingsManager<Parent> pManager;
        private readonly ISettingsManager<Child> cManager;
        private readonly ISettingsManager<Grandchild> gcManager;

        public SettingsController(ISettingsManager<Grandparent> gpManager, ISettingsManager<Parent> pManager,
            ISettingsManager<Child> cManager, ISettingsManager<Grandchild> gcManager)
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<Grandparent>> GetGrandparent([FromRoute][Required]string accountId, [FromRoute][Required]string gpid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, gpid, null, null, null);

            var result = await gpManager.GetSettingAsync(pData, Resource.Grandparent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<Parent>> GetParent([FromRoute][Required]string accountId, [FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, gpid, pid, null, null);

            var result = await pManager.GetSettingAsync(pData, Resource.Parent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<Child>> GetChild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, gpid, pid, cid, null);

            var result = await cManager.GetSettingAsync(pData, Resource.Child);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<Grandchild>> GetGrandchild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid, 
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, [FromRoute][Required]string gcid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, gpid, pid, cid, gcid);

            var result = await gcManager.GetSettingAsync(pData, Resource.Grandchild);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<List<Grandparent>>> GetGrandparents([FromRoute][Required]string accountId)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, null, null, null, null);

            var result = await gpManager.GetSettingsAsync(pData, Resource.Grandparent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<List<Parent>>> GetParents([FromRoute][Required]string accountId, 
            [FromRoute][Required]string gpid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, gpid, null, null, null);

            var result = await pManager.GetSettingsAsync(pData, Resource.Parent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<List<Child>>> GetChildren([FromRoute][Required]string accountId, 
            [FromRoute][Required]string gpid, [FromRoute][Required]string pid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, gpid, pid, null, null);
            var result = await cManager.GetSettingsAsync(pData, Resource.Child);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult<List<Grandchild>>> GetGrandchildren([FromRoute][Required]string accountId, 
            [FromRoute][Required]string gpid, [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"], 
                accountId, gpid, pid, cid, null);
            var result = await gcManager.GetSettingsAsync(pData, Resource.Grandchild);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PatchGrandparent([FromRoute][Required]string accountId, 
            [FromRoute][Required]string gpid, [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, null, null, null);
            var result = await gpManager.UpdateSettingAsync(pData, body, Resource.Grandparent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PatchParent([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, null, null);
            var result = await pManager.UpdateSettingAsync(pData, body, Resource.Parent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PatchChild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, cid, null);
            var result = await cManager.UpdateSettingAsync(pData, body, Resource.Child);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PatchGrandchild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, [FromRoute][Required]string gcid, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, cid, gcid);
            var result = await gcManager.UpdateSettingAsync(pData, body, Resource.Grandchild);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PostGrandparent([FromRoute][Required]string accountId, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, null, null, null, null);
            var result = await gpManager.CreateSettingAsync(pData, body, Resource.Grandparent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PostParent([FromRoute][Required]string accountId, [FromRoute][Required]string gpid, 
            [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, null, null, null);
            var result = await pManager.CreateSettingAsync(pData, body, Resource.Parent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PostChild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, null, null);
            var result = await cManager.CreateSettingAsync(pData, body, Resource.Child);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> PostGrandchild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, [FromBody][Required]SettingsOnly body)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, cid, null);
            var result = await gcManager.CreateSettingAsync(pData, body, Resource.Grandchild);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> DeleteGrandparent([FromRoute][Required]string accountId, [FromRoute][Required]string gpid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, null, null, null);
            var result = await gpManager.DeleteSettingAsync(pData, Resource.Grandparent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> DeleteParent([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, null, null);
            var result = await pManager.DeleteSettingAsync(pData, Resource.Parent);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> DeleteChild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, cid, null);
            var result = await cManager.DeleteSettingAsync(pData, Resource.Child);
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
        [TypeFilter(typeof(RequireTokenAttribute))]
        public async virtual Task<ActionResult> DeleteGrandchild([FromRoute][Required]string accountId, [FromRoute][Required]string gpid,
            [FromRoute][Required]string pid, [FromRoute][Required]string cid, [FromRoute][Required]string gcid)
        {
            var pData = ProcessDataHelpers.InitiateProcessData((Permissions)RouteData.Values["access"],
                accountId, gpid, pid, cid, gcid);
            var result = await gcManager.DeleteSettingAsync(pData, Resource.Grandchild);
            return StatusCode(result);
        }
        #endregion

    }

        
}
