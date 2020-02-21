using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class DigitalTwinEventController : ApiController
    {
        private Services.DigitalTwinEventService digitalTwinEventService;
        private Services.IdentityAccessService IdentityAccessService;

        public DigitalTwinEventController()
        {
            this.digitalTwinEventService = new Services.DigitalTwinEventService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }

        // GET: api/DigitalTwinEvent
        [Route("~/api/v1/DigitalTwinEvent/{DigitalTwin}")]
        public IHttpActionResult Get(long digitalTwin)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = digitalTwinEventService.GetAllDigitalTwinEvents(digitalTwin, organizationOut);
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


        // GET: api/DigitalTwinEvent/5/6
        [Route("~/api/v1/DigitalTwinEvent/{id}/{DigitalTwin}")]
        public IHttpActionResult Get(long id, long digitalTwin)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = digitalTwinEventService.GetDigitalTwinEvent(id, digitalTwin, organizationOut);
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


        // POST: api/DigitalTwinEvent
        [Route("~/api/v1/DigitalTwinEvent")]
        public IHttpActionResult Post([FromBody] Models.DigitalTwinEventRequest value)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = digitalTwinEventService.CreateDigitalTwinEvent(value, organizationOut);
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
        [Route("~/api/v1/DigitalTwinEvent/{id}")]
        public IHttpActionResult Delete(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result1 = digitalTwinEventService.DeleteDigitalTwinEvent(id, organizationOut);
                        if (result1 == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result1);
                        }
                    case 2:
                        var result2 = digitalTwinEventService.DeleteDigitalTwinEvent(id, organizationOut);
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
        // GET: api/DigitalTwinEvent
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DigitalTwinEvent/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DigitalTwinEvent
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DigitalTwinEvent/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DigitalTwinEvent/5
        public void Delete(int id)
        {
        }
        **/

    }
}
