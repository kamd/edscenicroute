using EDScenicRouteCore.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.DataUpdates
{
    public class EDSMSystemEnquirer
    {
        private readonly HttpClient client = new HttpClient();
        private const string DefaultBaseUri = "https://www.edsm.net/api-v1/system";

        public EDSMSystemEnquirer(string baseUri = null)
        {
            client.BaseAddress = new Uri(baseUri ?? DefaultBaseUri);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent.UserAgentString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<GalacticSystem> GetSystemAsync(string name)
        {
            GalacticSystem system = null;
            string urlName = HttpUtility.UrlEncode(name.Trim());
            var cancellationTokenSource = new CancellationTokenSource(5000);
            HttpResponseMessage response = await client.GetAsync($"?systemName={urlName}&showCoordinates=1",
                cancellationTokenSource.Token);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    JObject obj = await response.Content.ReadAsAsync<JObject>();
                    system = new GalacticSystem();
                    system.Name = (string) obj["name"];
                    var coords = (JObject) obj["coords"];
                    system.Coordinates = new Vector3(
                        (float) coords["x"],
                        (float) coords["y"],
                        (float) coords["z"]);
                }
                catch (Exception)
                {
                    throw new SystemNotFoundException() {SystemName = name};
                }
                
            } else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            return system;
        }

    }
}
