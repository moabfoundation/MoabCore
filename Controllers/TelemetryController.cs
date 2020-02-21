using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class TelemetryController : ApiController
    {
        private Services.TelemetryService telemetryService;
        private Services.IdentityAccessService IdentityAccessService;

        public TelemetryController()
        {
            this.telemetryService = new Services.TelemetryService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }

        // POST: api/Telemetry
        [Route("~/api/v1/Telemetry")]
        public IHttpActionResult Post([FromBody] JObject value)
        {
            if (IdentityAccessService.IsThingAuthorized(Request, out long organizationOut, out long identityOut, out long digitalTwinModelOut))
            {
                var result = telemetryService.CreateTelemetry(value, identityOut, organizationOut, digitalTwinModelOut);
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


        // GET: api/Telemetry
        [Route("~/api/v1/Telemetry")]
        public IHttpActionResult Get()
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = telemetryService.GetAllTelemetry(organizationOut);
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



        // GET: api/Telemetry/5
        [Route("~/api/v1/Telemetry/{id}")]
        public IHttpActionResult Get(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = telemetryService.GetTelemetry(id, organizationOut);
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




        // DELETE: api/ApiWithActions/5
        [Route("~/api/v1/Telemetry/{id}")]
        public IHttpActionResult Delete(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = telemetryService.DeleteTelemetry(id, organizationOut);
                        if (result == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result);
                        }
                    case 2:
                        var result2 = telemetryService.DeleteTelemetry(id, organizationOut);
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

        // PUT: api/Telemetry/5
        public void Put(int id, [FromBody]string value)
        {
        }

        **/

    }
}
