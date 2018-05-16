using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace WebApi.Filter
{
    public class HttpBasicAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var authorizeation = actionContext.Request.Headers.Authorization;

            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Count != 0 ||
                actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true)
                    .Count != 0)
            {
                base.OnAuthorization(actionContext);
            }
            else if (authorizeation!=null&&authorizeation.Parameter!=null)
            {
                //用户验证
                if (ValidateTicket(authorizeation.Parameter))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
            {
                ReasonPhrase = "Unauthorized"
            };
        }

        private bool ValidateTicket(string encryptTicket)
        {
            var Ticket = FormsAuthentication.Decrypt(encryptTicket).UserData;
            return string.Equals(Ticket, string.Format("{0}&{1}", "admin", "123456"));
        }
    }
}