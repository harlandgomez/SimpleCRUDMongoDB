using System.Web;

namespace SimpleCRUDMongoDB.ExtensionMethods
{

    public static class StringExtensionMethods
    {
        public static string ToBannerHtml(this string nonHtmlText)
        {
            return HttpUtility.HtmlEncode(nonHtmlText);
        }
    }
}
