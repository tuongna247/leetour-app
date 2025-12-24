using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Vinaday.Web.Framework
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName);
        void SignOut();
        //string CreatePasswordHash(string pass, string fomat);
    }
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException(string.Format("value cannot be null or empty."), string.Format("userName"));
            FormsAuthentication.SetAuthCookie(userName, true);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        //public string CreatePasswordHash(string pass, string fomat)
        //{
        //    return FormsAuthentication.HashPasswordForStoringInConfigFile(pass, fomat);
        //}
    }
}
