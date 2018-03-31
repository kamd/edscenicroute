using EDScenicRouteCore.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.DataUpdates
{
    public class EDSMSystemEnquirer
    {
        private HttpClient client = new HttpClient();
        private const string DefaultBaseUri = "https://www.edsm.net/api-v1/system";

        public EDSMSystemEnquirer(string baseUri = null)
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri(baseUri ?? DefaultBaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<GalacticSystem> GetSystemAsync(string name)
        {
            GalacticSystem system = null;
            HttpResponseMessage response = await client.GetAsync($"?systemName={name}&showCoordinates=1");
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
