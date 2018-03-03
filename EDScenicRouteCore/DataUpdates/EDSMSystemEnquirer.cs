using EDScenicRouteCore.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EDScenicRouteCore.DataUpdates
{
    public class EDSMSystemEnquirer
    {
        private HttpClient client = new HttpClient();

        public EDSMSystemEnquirer()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://www.edsm.net/api-v1/system");
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
                JObject obj = await response.Content.ReadAsAsync<JObject>();
                system = new GalacticSystem();
                system.Name = (string) obj["name"];
                var coords = (JObject) obj["coords"];
                system.Coordinates = new System.Numerics.Vector3(
                    (float)coords["x"],
                    (float)coords["y"],
                    (float)coords["z"]);
            } else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            return system;
        }

    }
}
