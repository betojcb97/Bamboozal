using Azure;
using Newtonsoft.Json.Linq;

namespace Bamboo.API
{
    public class GoogleImage
    {
        static readonly HttpClient client = new HttpClient();
        public async Task<string> Search(string query)
        {
            string apiKey = "AIzaSyCzVdplurQGJaqRb00Zh_ZZ-RAPH54cGNQ";
            string searchEngineId = "3216652fa405b4524";

            string url = "";
            url += "https://www.googleapis.com/customsearch/v1?key=";
            url += apiKey;
            url += "&cx=" + searchEngineId;
            url += "&q=" + query;
            url += "&searchType=image";

            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(content);

            string imageUrl = json["items"][0]["link"].ToString();

            return imageUrl;
        }
    }
}
