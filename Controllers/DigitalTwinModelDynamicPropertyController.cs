using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class DigitalTwinModelDynamicPropertyController : ApiController
    {
        private Services.DigitalTwinModelDynamicPropertyService dynamicPropertyService;
        private Services.IdentityAccessService IdentityAccessService;

        public DigitalTwinModelDynamicPropertyController()
        {
            this.dynamicPropertyService = new Services.DigitalTwinModelDynamicPropertyService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }


        // GET: api/DigitalTwinModelDynamicProperty
        [Route("~/api/v1/DigitalTwinModelDynamicProperty/{DigitalTwinModel}")]
        public IHttpActionResult Get(long digitalTwinModel)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = dynamicPropertyService.GetAllDigitalTwinModelDynamicProperties(digitalTwinModel, organizationOut);
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


        // GET: api/DigitalTwinModelDynamicProperty/5/6
        [Route("~/api/v1/DigitalTwinModelDynamicProperty/{id}/{DigitalTwinModel}")]
        public IHttpActionResult Get(long id, long digitalTwinModel)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = dynamicPropertyService.GetDigitalTwinModelDynamicProperty(id, digitalTwinModel, organizationOut);
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


        // POST: api/DigitalTwinModelDynamicProperty
        [Route("~/api/v1/DigitalTwinModelDynamicProperty")]
        public IHttpActionResult Post([FromBody] Models.DigitalTwinModelDynamicPropertyRequest value)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = dynamicPropertyService.CreateDigitalTwinModelDynamicProperty(value, organizationOut);
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
        [Route("~/api/v1/DigitalTwinModelDynamicProperty/{id}/{DigitalTwinModel}")]
        public IHttpActionResult Delete(long id, long digitalTwinModel)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result1 = dynamicPropertyService.DeleteDigitalTwinModelDynamicProperty(id, digitalTwinModel, organizationOut);
                        if (result1 == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result1);
                        }
                    case 2:
                        var result2 = dynamicPropertyService.DeleteDigitalTwinModelDynamicProperty(id, digitalTwinModel, organizationOut);
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

        // PUT: api/DynamicProperty/5
        public void Put(int id, [FromBody]string value)
        {
        }

        **/

    }
}
