using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class OrganizationController : ApiController
    {
        private Services.OrganizationService organizationService;
        private Services.IdentityAccessService IdentityAccessService;

        public OrganizationController()
        {
            this.organizationService = new Services.OrganizationService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }

        // POST: api/Organization
        [Route("~/api/v1/Organization")]
        public IHttpActionResult Post([FromBody] Models.OrganizationRequest value)
        {
            var result = organizationService.Initialize(value);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        // DELETE: api/ApiWithActions/5
        [Route("~/api/v1/Organization/{id}")]
        public IHttpActionResult Delete(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result1 = organizationService.DeleteOrganization(organizationOut);
                        if (result1 == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result1);
                        }
                    default:
                        return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }


        /**
        // GET: api/Organization
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Organization/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Organization
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Organization/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Organization/5
        public void Delete(int id)
        {
        }
        **/

    }
}
