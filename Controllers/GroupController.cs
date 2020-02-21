using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoabCore.Controllers
{
    public class GroupController : ApiController
    {
        private Services.GroupService thingGroupService;
        private Services.IdentityAccessService IdentityAccessService;

        public GroupController()
        {
            this.thingGroupService = new Services.GroupService();
            this.IdentityAccessService = new Services.IdentityAccessService();
        }

        // GET: api/Group
        [Route("~/api/v1/Group")]
        public IHttpActionResult Get()
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = thingGroupService.GetAllGroups(organizationOut);
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

        // GET: api/Group/5
        [Route("~/api/v1/Group/{id}")]
        public IHttpActionResult Get(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                var result = thingGroupService.GetGroup(id, organizationOut);
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


        // POST: api/Group
        [Route("~/api/v1/Group")]
        public IHttpActionResult Post([FromBody] Models.GroupRequest value)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result = thingGroupService.CreateGroup(value, organizationOut);
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
        [Route("~/api/v1/Group/{id}")]
        public IHttpActionResult Delete(long id)
        {
            if (IdentityAccessService.IsUserAuthorized(Request, out long organizationOut, out long identityOut, out long roleOut))
            {
                switch (roleOut)
                {
                    case 1:
                        var result1 = thingGroupService.DeleteGroup(id, organizationOut);
                        if (result1 == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(result1);
                        }
                    case 2:
                        var result2 = thingGroupService.DeleteGroup(id, organizationOut);
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


        // PUT: api/Group/5
        public void Put(int id, [FromBody]string value)
        {
        }

        **/

    }
}
