using Event_Manager_API.BL.Interface;
using Event_Manager_API.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Event_Manager_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        IJobBL _jobBL;

        public JobsController(IJobBL jobBL)
        {
            _jobBL = jobBL;
        }


        [HttpGet]
        [Route("jobs")]
        public IActionResult GetJobs([FromQuery]string userId)
        {
            var res = _jobBL.GetJobs(userId);
            return switchAction(res);
        }

        [HttpGet]
        [Route("job")]
        public IActionResult GetJob([FromQuery]string jobId)
        {
            var res = _jobBL.GetJob(jobId);
            return switchAction(res);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Job job)
        {
            var res = _jobBL.InsertJob(job);
            return switchAction(res);
        }

        // PUT api/<JobsController>/5
        [HttpPut]
        public IActionResult Put(Job job)
        {
            var res = _jobBL.UpdateJob(job);
            return switchAction(res);
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery]string jobId)
        {
            var res = _jobBL.DeleteJob(jobId);
            return switchAction(res);
        }


        //method private
        private IActionResult switchAction(ServiceResult serviceResult)
        {
            switch (serviceResult.Code)
            {
                case ServiceCode.BadRequest:
                    return BadRequest(serviceResult);
                case ServiceCode.Success:
                    return Ok(serviceResult);
                case ServiceCode.Excaption:
                    return StatusCode(500);
                default:
                    return Ok();
            }
        }
    }
}
