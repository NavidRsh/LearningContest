using LearningContest.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearningContest.Api.Controllers
{
    public class BaseController : ControllerBase
    {

        protected (int userId, List<byte> kinds, int customerId) GetUserDetail(ClaimsPrincipal principal)
        {
            var currentKindIds = new List<byte>();
            int currentUserId = 0;
            int currentCustomerId = 0;



            var kinds = principal.Claims.Where(q => q.Type == "kind").ToList();
            var user = User.Claims.Where(a => a.Type == Domain.Enums.User.ClaimType.UserIdClaim).FirstOrDefault();
            var customer = User.Claims.Where(a => a.Type == Domain.Enums.User.ClaimType.CustomerIdClaim).FirstOrDefault();


            if (customer != null)
                int.TryParse(customer.Value, out currentCustomerId);

            if (user != null)
                int.TryParse(user.Value, out currentUserId);

            foreach (var kind in kinds)
            {
                byte.TryParse(kind.Value, out var k);
                currentKindIds.Add(k);
            }

            return (userId: currentUserId, kinds: currentKindIds, customerId: currentCustomerId);


        }
        protected int GetUserId(ClaimsPrincipal principal)
        {
            
            int currentUserId = 0;
            var user = User.Claims.Where(a => a.Type == Domain.Enums.User.ClaimType.UserIdClaim).FirstOrDefault();
            
            if (user != null)
                int.TryParse(user.Value, out currentUserId);

            return currentUserId; 
        }

    }
}
