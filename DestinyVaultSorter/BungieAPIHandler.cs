using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Numerics;

namespace DestinyVaultSorter
{
    public class BungieAPISettings
    {
        public string APIKey { get; set; } 
        public string client_ID { get; set; }
        public string client_secret { get; set;}
        public string authCode { get; set; }
        public string? authKey { get; set; }
        public string? membershipID { get; set; }
        public string? membershipType { get; set; }
        public List<string> characterIDs = new List<string>();
    }


    public class BungieAPIHandler
    {
        private BungieAPISettings mySettings;
        RestClient client;
        public BungieAPIHandler(BungieAPISettings settings)
        {
            client = new RestClient("https://www.bungie.net/");
            client.AddDefaultHeader("X-API-Key", settings.APIKey);
            mySettings = settings;

            if (mySettings.authKey != null)
            {
                client.AddDefaultHeader("Authorization", "Bearer " + settings.authKey);
            }
            else
            {
                addAuthorization();
            }
        }
        
        public void getAllOwnedWeapons()
        {
            foreach(string charID in mySettings.characterIDs)
            {
                dynamic? curCharWeps = getCharacterWeapons(charID);

                //later do this multi threaded
                sortInventory(curCharWeps.Response.inventory.data.items);
                return;
            }
        }

        public void addAuthorization()
        {
            var authClient = new RestClient("https://www.bungie.net/platform/app/oauth/token/?");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            var postData = String.Format($"client_id={mySettings.client_ID}&client_secret={mySettings.client_secret}&grant_type=authorization_code&code={mySettings.authCode}");
            request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
            RestResponse response = authClient.Execute(request);
            dynamic? item = JsonConvert.DeserializeObject(response.Content);

            if (item != null)
            {
                //Adding auth header
                string access_code = item.access_token;
                client.AddDefaultHeader("Authorization", "Bearer " + access_code);
                mySettings.authKey = access_code;
                saveSettings();
            }

            getAccountIDs();
        }

        public dynamic? getWeaponManifest(string wepRefId)
        {
            string requestString = "Platform/Destiny2/Manifest/DestinyInventoryItemDefinition/" + wepRefId + "/";
            RestRequest request = new RestRequest(requestString); 
            RestResponse response = client.Execute(request);
            dynamic? item = JsonConvert.DeserializeObject(response.Content);
            return item;
        }

        public void getAccountIDs()
        {
            if (mySettings.characterIDs.Count != 0)
                return;

            RestRequest request = new RestRequest("/Platform/User/GetMembershipsForCurrentUser/");
            RestResponse response = client.Execute(request);
            dynamic? responseJson = JsonConvert.DeserializeObject(response.Content);

            if(responseJson != null)
            {
                //Getting memebershipID and type which can then be used to get character IDs
                mySettings.membershipID = responseJson.Response.primaryMembershipId;
                mySettings.membershipType = responseJson.Response.destinyMemberships[0].membershipType;

                RestRequest characterIDRequest = new RestRequest("/Platform/Destiny2/3/Profile/4611686018470941199/?components=200");
                RestResponse characterResponse = client.Execute(characterIDRequest);
                dynamic? characterIDs = JsonConvert.DeserializeObject(characterResponse.Content);

                if(characterIDs != null)
                {
                    foreach(var charID in characterIDs.Response.characters.data)
                    {
                        mySettings.characterIDs.Add(charID.Name);
                        Console.WriteLine(charID.Name);
                    }
                }

                saveSettings();
            }
        }

        public dynamic? getCharacterWeapons(string charID)
        {
            //1498876634 kinetic weapons
            //2465295065 energy weapons
            //953998645 power weapons
            string requestString = String.Format($"/Platform/Destiny2/{mySettings.membershipType}/Profile/{mySettings.membershipID}/Character/{charID}/?components=201");
            RestRequest request = new RestRequest(requestString);
            RestResponse response = client.Execute(request);
            dynamic? inventory = JsonConvert.DeserializeObject(response.Content);

            Console.WriteLine(requestString);
            return inventory;
        }

        public void sortInventory(dynamic invetory)
        {
            if(invetory == null)
            {
                return;
            }
            else
            {
                foreach(var item in invetory)
                {
                    //item.itemHash
                    //item.itemInstanceId                  memID                  instanceID
                    ///Platform/Destiny2/3/Profile/4611686018470941199/Item/6917529892396675415/?components=300
                    //bucketHash for inventory bucket
                    //instanceID for wepID
                    //damage type hash for element
                    //primary stat for level

                    //getMAnifest for wepName, element, icon
                    Console.WriteLine(item);
                    //Console.WriteLine(item.itemHash);
                    //request data on this 
                    return;
                }
            }
        }

        public void saveSettings()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            string settingsPath = System.IO.Path.Join(path, "vaultSettings.txt");
            string jsonString = JsonConvert.SerializeObject(mySettings);
            File.WriteAllText(settingsPath, jsonString);
        }
    }
}
