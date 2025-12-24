using System;
using System.Web;
using System.Web.Security;
using Vinaday.Data.Models;
using Vinaday.Services;
using Vinaday.Web.Framework.Services;

namespace Vinaday.Web.Framework
{
    public class MySession
    {
        public static aspnet_Membership User
        {
            get
            {
                var userAuthenticated = GetAuthenticatedUser();
                if (userAuthenticated != null) return userAuthenticated;
                var custCookie = GetCustCookie();
                if (custCookie != null && !String.IsNullOrEmpty(custCookie.Value))
                {
                    Guid custGuid;
                    if (Guid.TryParse(custCookie.Value, out custGuid))
                    {
                        IAccountService account = new AccountService();

                        var customerByCookie = account.GetMembershipById(custGuid);
                        if (customerByCookie != null)
                            userAuthenticated = customerByCookie;
                    }
                }
                if (userAuthenticated != null)
                    SetUserCookie(userAuthenticated.UserId);

                return userAuthenticated;
            }
        }
        #region Utilities

        public static HttpCookie GetCustCookie(){
            return HttpContext.Current == null ? null : HttpContext.Current.Request.Cookies[Utilities.CookieName];
        }

        public static void SetUserCookie(Guid userGuid)
        {
            if (HttpContext.Current == null) return;
            var cookie = new HttpCookie(Utilities.CookieName)
            {
                HttpOnly = true,
                Value = userGuid.ToString()
            };
            if (userGuid == Guid.Empty)
            {
                cookie.Expires = DateTime.Now.AddMonths(-1);
            }
            else
            {
                var cookieExpires = ConfigManager.CookieExpires;
                cookie.Expires = DateTime.Now.AddHours(cookieExpires);
            }

            HttpContext.Current.Response.Cookies.Remove(Utilities.CookieName);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        #endregion

        #region method

        public static aspnet_Membership GetAuthenticatedUser()
        {
            if (HttpContext.Current == null ||
                !HttpContext.Current.Request.IsAuthenticated ||
                !(HttpContext.Current.User.Identity is FormsIdentity))
            {
                return null;
            }
            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
            return GetAuthenticatedUserFromTicket(formsIdentity.Ticket);

        }
        public static aspnet_Membership GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            var usernameOrEmail = ticket.Name;
            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            IAccountService account = new AccountService();
            var user = account.GetMembershipByName(usernameOrEmail);

            return user;

        }
        #endregion method
    }
}