namespace Application.Interfaces
{
    public interface ITokenEncoder
    {
        /// <summary>
        /// توکن را با قالب Base64Url کدگذاری میکند
        /// </summary>
        /// <param name="token">رشته حاوی توکن</param>
        /// <returns>رشته کدگذاری شده با Base64Url</returns>
        string Encode(string token);
        /// <summary>
        /// توکن کدگذاری شده با Base64Url را به مقدار اصلی تبدیل میکند
        /// </summary>
        /// <param name="token">رشته کدگذاری شده با Base64Url</param>
        /// <returns>رشته اصلی توکن</returns>
        string Decode(string token);
    }
}
