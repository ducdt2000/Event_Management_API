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
    public class UserAccountsController : ControllerBase
    {
        IUserAccountBL _userAccountBL;

        public UserAccountsController(IUserAccountBL userAccountBL)
        {
            _userAccountBL = userAccountBL;
        }


        [HttpPost]
        public IActionResult Post([FromBody] UserAccount userAccount)
        {
            var result = _userAccountBL.InsertUserAccount(userAccount);
            return switchAction(result);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login([FromQuery]string id, string password)
        {
            var result = _userAccountBL.Login(id, password);
            return switchAction(result);
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
