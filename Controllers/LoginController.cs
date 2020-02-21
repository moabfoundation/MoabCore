using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class LoginController : ApiController
    {
        private Services.LoginService userLoginService;
        private Services.IdentityAccessService IdentityAccessService;

        public LoginController()
        {
            this.userLoginService = new Services.LoginService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }


        // POST: api/Login
        [Route("~/api/v1/Login")]
        public IHttpActionResult Post([FromBody] Models.LoginRequest value)
        {
            var result = userLoginService.Login(value.EmailAddress, value.Password);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }


        /**
        // GET: api/UserLogin
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UserLogin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserLogin
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserLogin/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserLogin/5
        public void Delete(int id)
        {
        }
        **/

    }
}
