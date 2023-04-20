using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace DestinyVaultSorter
{
    public class BungieAPIHandler
    {
        HttpClient client;

        public BungieAPIHandler(string APIKey)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-Key", APIKey);
        }

        public dynamic? getWeaponManifest(string wepRefId)
        {
            var response = client.GetAsync("https://www.bungie.net//Platform/Destiny2/Manifest/DestinyInventoryItemDefinition/" + wepRefId + "/").Result;
            var content  = response.Content.ReadAsStringAsync().Result;
            dynamic? item = item = JsonConvert.DeserializeObject(content);
            return item;
        }

    }
}
