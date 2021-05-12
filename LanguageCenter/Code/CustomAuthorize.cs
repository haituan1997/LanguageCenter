using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
namespace LanguageCenter.Helper
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly string[] allowedroles;// hàm này mình cútom
        public CustomAuthorize(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            foreach (var role in allowedroles)
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                // Get the claims values
                try
                {
                    //nếu chỗ này mình có chức năng như bên mình xong quay đây mình sẽ khoe vs cái ten controller đó, còn chỗ này mình lưu lại cái type trong claim nên a lấy ra so sanh, typ3 thì full quyền
                    var type = identity.Claims.Where(c => c.Type == "TypeUser")
                   .Select(c => c.Value).SingleOrDefault(); ;
                    if (role.IndexOf(type)>=0|| string.IsNullOrEmpty(role))
                    {
                        authorize = true; /* return true if Entity has current user(active) with specific role */

                    }

                }
                catch (Exception e)
                {
                    return false;
                }

            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }


    }
}