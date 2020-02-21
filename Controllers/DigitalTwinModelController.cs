using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class DigitalTwinModelController : ApiController
    {
        private Services.DigitalTwinModelService digitalTwinModelService;
        private Services.IdentityAccessService IdentityAccessService;

        public DigitalTwinModelController()
        {
            this.digitalTwinModelService = new Services.DigitalTwinModelService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }

        // GET: api/DigitalTwinModel
        [Route("~/api/v1/DigitalTwinModel")]
        public IHttpActionResult Get()
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = digitalTwinModelService.GetAllDigitalTwinModels(organizationOut);
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

        // GET: api/DigitalTwinModel/5
        [Route("~/api/v1/DigitalTwinModel/{id}")]
        public IHttpActionResult Get(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = digitalTwinModelService.GetDigitalTwinModel(id, organizationOut);
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


        // POST: api/DigitalTwinModel
        [Route("~/api/v1/DigitalTwinModel")]
        public IHttpActionResult Post([FromBody] Models.DigitalTwinModelRequest value)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = digitalTwinModelService.CreateDigitalTwinModel(value, organizationOut);
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
        [Route("~/api/v1/DigitalTwinModel/{id}")]
        public IHttpActionResult Delete(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = digitalTwinModelService.DeleteDigitalTwinModel(id, organizationOut);
                        if (result == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result);
                        }
                    case 2:
                        var result2 = digitalTwinModelService.DeleteDigitalTwinModel(id, organizationOut);
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


        // PUT: api/DigitalTwinModel/5
        public void Put(int id, [FromBody]string value)
        {
        }

        **/

    }
}
