using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Domain.Enums.User
{
    public static class ClaimType
    {
        public const string UserIdClaim = "id";

        public const string RoleClaim = System.Security.Claims.ClaimTypes.Role;

        public const string KindClaim = "kind";

        public const string CustomerIdClaim = "customerId"; 
    }
}
