using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using RestSharp;
using Newtonsoft.Json;

namespace DestinyVaultSorter
{
    public class BungieAPIHandler
    {
        HttpClient client;
        private string APIKey = "";
        public BungieAPIHandler()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-Key", APIKey);
        }

        public void addAuthorization()
        {
            var authClient = new RestClient("https://www.bungie.net/platform/app/oauth/token/?");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            var postData = "client_id=CLIENT_ID&client_secret=CLIENT_SECRET&grant_type=authorization_code&code=CODE_HERE";
            request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
            RestResponse response = authClient.Execute(request);
            dynamic? item = JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(response.Content);

            if (item != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + item.access_code);
            }
        }

        public void removeAuthorization()
        {
            client.DefaultRequestHeaders.Remove("Authorization");
        }

        public dynamic? getWeaponManifest(string wepRefId)
        {
            var response = client.GetAsync("https://www.bungie.net//Platform/Destiny2/Manifest/DestinyInventoryItemDefinition/" + wepRefId + "/").Result;
            var content  = response.Content.ReadAsStringAsync().Result;
            dynamic? item = JsonConvert.DeserializeObject(content);
            return item;
        }

        public dynamic? getPlayerWeapons()
        {
            //addAuthorization();
            // /3/ = membership type
            // /4611686018470941199/ = destiny membership id
            // /2305843009301268998/ = character id
            var inventoryResponse = client.GetAsync("https://www.bungie.net/Platform/Destiny2/3/Profile/4611686018470941199/Character/2305843009301268998/?components=201").Result;
            var inventoryContent = inventoryResponse.Content.ReadAsStringAsync().Result;
            dynamic? inventory = JsonConvert.DeserializeObject(inventoryContent);

            Console.WriteLine(inventoryContent);
            Console.WriteLine(inventory);

            return inventory;

            //1498876634 kinetic weapons
            //2465295065 energy weapons
            //953998645 power weapons
        }
    }
}
