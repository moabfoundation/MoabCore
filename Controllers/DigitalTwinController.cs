using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class DigitalTwinController : ApiController
    {
        private Services.DigitalTwinService digitalTwinService;
        private Services.IdentityAccessService IdentityAccessService;

        public DigitalTwinController()
        {
            this.digitalTwinService = new Services.DigitalTwinService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }

        // GET: api/DigitalTwin
        [Route("~/api/v1/DigitalTwin")]
        public IHttpActionResult Get()
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = digitalTwinService.GetAllDigitalTwins(organizationOut);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return Unauthorized();

            }
        }


        // GET: api/DigitalTwin/5
        [Route("~/api/v1/DigitalTwin/{id}")]
        public IHttpActionResult Get(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = digitalTwinService.GetDigitalTwin(id, organizationOut);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return Unauthorized();
            }

        }


        // POST: api/DigitalTwin
        [Route("~/api/v1/DigitalTwin")]
        public IHttpActionResult Post([FromBody] Models.DigitalTwinRequest value)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = digitalTwinService.CreateDigitalTwin(value, organizationOut);
                        if (result == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result);
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


        // DELETE: api/ApiWithActions/5
        [Route("~/api/v1/DigitalTwin/{id}")]
        public IHttpActionResult Delete(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = digitalTwinService.DeleteDigitalTwin(id, organizationOut);
                        if (result == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result);
                        }
                    case 2:
                        var result2 = digitalTwinService.DeleteDigitalTwin(id, organizationOut);
                        if (result2 == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result2);
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


        // PUT: api/Thing/5
        public void Put(int id, [FromBody]string value)
        {
        }

        **/

    }
}
