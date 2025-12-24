using System;
using System.Configuration;

namespace Vinaday.Web.Framework
{
    public static class ConfigManager
    {
        public static int CookieExpires
        {
            get
            {
                if (ConfigurationManager.AppSettings["CookieExpires"] == null)
                    return 24 * 365;
                var cookieExpires = Convert.ToInt32(ConfigurationManager.AppSettings["CookieExpires"]);
                return cookieExpires;
            }
        }
    }
}
