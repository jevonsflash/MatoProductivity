using MatoProductivity.Core.Location;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Amap
{
    public class AmapLocationResolveProvider : ILocationResolveProvider
    {
        protected AmapHttpRequestClient AmapHttpRequestClient { get; }

        public AmapLocationResolveProvider(AmapHttpRequestClient amapHttpRequestClient)
        {
            AmapHttpRequestClient = amapHttpRequestClient;
        }

        public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50)
        {
            var inverseRequestPramter = new AmapInverseHttpRequestParamter();
            inverseRequestPramter.Locations = new Location.Location[1]
            {
                new Location.Location(lat, lng)
            };
            return await AmapHttpRequestClient.InverseAsync(inverseRequestPramter);
        }

        public async Task<GecodeLocation> GeocodeAsync(string address, string city)
        {
            var positiceRequestParamter = new AmapPositiveHttpRequestParamter
            {
                Address = address
            };
            return await AmapHttpRequestClient.PositiveAsync(positiceRequestParamter);
        }
    }
}
