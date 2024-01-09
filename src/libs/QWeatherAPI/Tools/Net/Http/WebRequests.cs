using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Tools.Net.Http
{
    internal static class WebRequests
    {
        // Get 请求
        internal static async Task<HttpResponseMessage> GetRequestAsync(string Url, HttpMessageHandler handler = null)
        {
            Url = Url.Trim();
            if (handler == null)
            {
                handler = new HttpClientHandler()
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip
                };
            }
            using (HttpClient client = new HttpClient(handler))
            {
                return await client.GetAsync(Url);
            }
        }
    }
}
