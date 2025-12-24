using System.Text;
using System.Web.Mvc;

namespace Vinaday.Web.Framework.Extension{
    public class SharingExtension{
        private readonly string _description;
        private readonly string _title;
        private readonly string _url;
        private readonly string _image;
        public SharingExtension(string title, string description, string url, string image){
            _title = title;
            _description = description;
            _url = url;
            _image = image;
        }
        public MvcHtmlString MetaTag(){
            var stbdBuilder = new StringBuilder();
            // Facebook
            stbdBuilder.AppendFormat(" <meta name=\"og:title\" content=\"{0}\" />", _title);
            stbdBuilder.AppendFormat("\n  <meta name=\"og:url\" content=\"{0}\"/>", _url);
            stbdBuilder.AppendFormat("\n  <meta name=\"og:description\" content=\"{0}\"/>", _description);
            stbdBuilder.AppendFormat("\n  <meta name=\"og:image\" content=\"{0}\" />", _image);
            // Twitter 
            stbdBuilder.AppendFormat("\n  <meta name=\"Twitter:card\" content=\"summary\" />");
            stbdBuilder.AppendFormat("\n  <meta name=\"Twitter:site\" content=\"@vinaday\" />");
            stbdBuilder.AppendFormat("\n  <meta name=\"Twitter:creator\" content=\"@vinaday\" />");
            stbdBuilder.AppendFormat("\n  <meta name=\"Twitter:title\" content=\"{0}\" />", _title);
            stbdBuilder.AppendFormat("\n  <meta name=\"Twitter:url\" content=\"{0}\" />", _url);
            stbdBuilder.AppendFormat("\n  <meta name=\"Twitter:description\" content=\"{0}\" />", _description);
            stbdBuilder.AppendFormat("\n  <meta name=\"Twitter:image\" content=\"{0}\"/>", _image);
            //Google+ 
            stbdBuilder.AppendFormat("\n  <meta itemprop=\"name\" content=\"{0}\" />", _title);
            stbdBuilder.AppendFormat("\n  <meta itemprop=\"description\" content=\"{0}\" />", _description);
            stbdBuilder.AppendFormat("\n  <meta itemprop=\"image\" content=\"{0}\" />", _image);

            return MvcHtmlString.Create(stbdBuilder.ToString());
        }
    }
}
