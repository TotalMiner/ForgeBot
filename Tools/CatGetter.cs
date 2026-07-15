using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForgeBot.Tools
{
    internal class CatGetter
    {
        public static async Task<string> GetRandomCatImageUrlAsync()
        {
            var client = WebControl.GetClient();

            try
            {
                string response = await client.GetStringAsync("https://api.thecatapi.com/v1/images/search");
                using JsonDocument data = JsonDocument.Parse(response);
                string imageUrl = data.RootElement[0].GetProperty("url").GetString();
                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! Error fetching cat image: {ex.Message}, using fallback image");
            }

            return "https://cdn2.thecatapi.com/images/1.jpg";
        }
    }
}
