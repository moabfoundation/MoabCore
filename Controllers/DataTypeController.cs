using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class DataTypeController : ApiController
    {
        private Services.DataTypeService dataTypeService;
        private Services.IdentityAccessService IdentityAccessService;

        public DataTypeController()
        {
            this.dataTypeService = new Services.DataTypeService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }


        // GET: api/DataType
        [Route("~/api/v1/DataType")]
        public IHttpActionResult Get()
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = dataTypeService.GetAllDataTypes();
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

        // GET: api/DataType/5
        [Route("~/api/v1/DataType/{id}")]
        public IHttpActionResult Get(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = dataTypeService.GetDataType(id);
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

        /**
        // GET: api/DataType
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DataType/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DataType
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DataType/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DataType/5
        public void Delete(int id)
        {
        }
        **/

    }
}
