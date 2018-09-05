using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EDScenicRouteCore.DataUpdates
{
    public class EDSMPOIDownloader
    {
        public const string EDSMBaseAddress = @"https://www.edsm.net/en/";
        public const string GalacticMappingJSONAddress = @"galactic-mapping/json";

        public async Task<string> DownloadPOIInfoAsJSON()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(EDSMBaseAddress);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent.UserAgentString);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                
                var json = await client.GetStringAsync(GalacticMappingJSONAddress);
                return json;
            }
        }
    }
}
