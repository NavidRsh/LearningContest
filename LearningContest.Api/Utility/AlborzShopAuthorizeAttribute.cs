//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LearningContest.Api.Utility
//{

//    public class LearningContestAuthorizeAttribute : TypeFilterAttribute
//    {
//        public LearningContestAuthorizeAttribute() : base(typeof(LearningContestAuthorizeFilter))
//        {
//            //Arguments = new object[] { values };
//        }
//    }
//    public class LearningContestAuthorizeFilter : IAsyncAuthorizationFilter
//    {
//        //readonly Claim _claim;
//        //readonly Access.AccessItemType[] _accessItems;

//        public LearningContestAuthorizeFilter()//Access.AccessItemType[] accessItems)
//        {
//            //_claim = claim;
//            //accessItems = accessItems;
//        }

//        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            try
//            {
//                if (context.HttpContext == null || context.HttpContext.User == null || context.HttpContext.User.Claims == null || context.HttpContext.User.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated)
//                {
//                    context.Result = new UnauthorizedResult();
//                    return;
//                }
//                var userInfo = context.HttpContext.User.Claims.Where(a => a.Type == "id").FirstOrDefault();
//                if (userInfo == null)
//                {
//                    context.Result = new UnauthorizedResult();
//                    return;
//                }
                
//            }
//            catch (Exception exc)
//            {
//                context.Result = new ForbidResult();
//            }

//        }
//    }

//}
