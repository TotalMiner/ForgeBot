using System.Net.Http;

namespace ForgeBot.Tools
{
    internal static class WebControl
    {
        public static HttpClient GetClient()
        {
            return _httpClient ??= CreateClient();
        }

        private static HttpClient _httpClient;

        private static HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ForgeBot/1.0");
            return client;
        }
    }
}
