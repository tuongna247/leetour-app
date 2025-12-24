using System.Text;
using System.Web.Mvc;

namespace Vinaday.Web.Framework.Extension{
    public class MetaTagExtension{
        private readonly string _description;
        private readonly string _keywords;
        public MetaTagExtension(string keywords, string description){
            _keywords = keywords;
            _description = description;
        }
        public MvcHtmlString MetaTag(){
            var stbdBuilder = new StringBuilder();
            stbdBuilder.AppendFormat("\n  <meta name=\"description\" content=\"{0}\" />", _description);
            //stbdBuilder.AppendFormat("\n  <meta name=\"keywords\" content=\"{0}\" />", _keywords);
            stbdBuilder.AppendFormat("\n  <meta name=\"robots\" content=\"index, follow\" />");
            stbdBuilder.AppendFormat("\n  <meta name=\"author\" content=\"vinaday.com\" />");
            stbdBuilder.AppendFormat("\n  <meta name=\"copyright\" content=\"vinaday.Com\" />");
            return MvcHtmlString.Create(stbdBuilder.ToString());
        }
    }
}
