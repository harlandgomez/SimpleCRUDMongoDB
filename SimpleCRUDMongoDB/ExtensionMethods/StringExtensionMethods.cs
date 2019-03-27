namespace SimpleCRUDMongoDB.ExtensionMethods
{

    public static class StringExtensionMethods
    {
        public static string ToBannerHtml(this string text)
        {
            return $"<!DOCTYPE html><html><head><title>Banner</title></head><body><div class='container'><p>{text}</p></div></body></html>";
        }
    }
}
