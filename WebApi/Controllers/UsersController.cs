using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Filter;
using WebApi.Models;

namespace WebApi.Controllers
{
    [HttpBasicAuthorize]
    public class UsersController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public string AccountLogin()
        {
            string account = HttpContext.Current.Request.Form["account"];
            string password= HttpContext.Current.Request.Form["password"];

            if (account.Equals("admin") && password.Equals("123456"))
            {
                FormsAuthenticationTicket ticketObj = new FormsAuthenticationTicket(
                    0, account, DateTime.Now, DateTime.Now.AddHours(1), true,
                    $"{account}&{password}",
                    FormsAuthentication.FormsCookiePath);
                var result = new
                {
                    Result = true,
                    Ticket = FormsAuthentication.Encrypt(ticketObj)
                };

                return JsonConvert.SerializeObject(result);
            }
            else
            {
                var result = new {Result = false};
                return JsonConvert.SerializeObject(result);
            }
        }
       

        [HttpPost]
        public Users Register(Users users)
        {
            Users _users = users;
            return _users;
        }


        public string RegisterObject(JObject Jinfo)
        {
            dynamic jdata = Jinfo;
            JObject jUser = jdata.User;
            string info = jdata.info;
            var user = jUser.ToObject<Users>();
            return JsonConvert.SerializeObject(user);
        }
    }
}
