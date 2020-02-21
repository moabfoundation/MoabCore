using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoabCore.Models
{
    public class UserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserDescription { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPassword { get; set; }
        public long Role { get; set; }
        public long UserGroup { get; set; }
        public long AgreeToTermsAndPolicies { get; set; }
        public long Enabled { get; set; }
    }
}
