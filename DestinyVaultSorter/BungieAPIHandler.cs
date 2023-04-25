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
        private WeaponDatabase database;
        private BungieAPISettings mySettings;
        RestClient client;
        public BungieAPIHandler(BungieAPISettings settings, WeaponDatabase database)
        {
            client = new RestClient("https://www.bungie.net/");
            client.AddDefaultHeader("X-API-Key", settings.APIKey);
            mySettings = settings;
            this.database = database;

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
            if(database.getWeaponCount() > 0)
            {
                database.clearDatabase();
            }

            foreach(string charID in mySettings.characterIDs)
            {
                dynamic? curCharWeps = getCharacterWeapons(charID);

                //later do it multithreaded maybe
                sortInventory(curCharWeps.Response.inventory.data.items);
            }

            //Grab vault items
            dynamic? vaultWeps = getVaultWeapons();
            sortInventory(vaultWeps.Response.profileInventory.data.items);
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
                    }
                }

                saveSettings();
            }
        }

        public dynamic? getCharacterWeapons(string charID)
        {
            string requestString = String.Format($"/Platform/Destiny2/{mySettings.membershipType}/Profile/{mySettings.membershipID}/Character/{charID}/?components=201");
            RestRequest request = new RestRequest(requestString);
            RestResponse response = client.Execute(request);
            dynamic? inventory = JsonConvert.DeserializeObject(response.Content);
            return inventory;
        }

        public dynamic? getVaultWeapons()
        {
            RestRequest request = new RestRequest($"/Platform/Destiny2/{mySettings.membershipType}/Profile/{mySettings.membershipID}/?components=102,307");
            RestResponse response = client.Execute(request);   
            dynamic? vaultInv = JsonConvert.DeserializeObject(response.Content);
            return vaultInv;
        }

        public async void sortInventory(dynamic inventory)
        {
            if(inventory == null)
            {
                return;
            }
            else
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                int apiCallsCount = 0;
                foreach(var item in inventory)
                {
                    //Check to make sure its actually a weapon
                    string bucketHash = item.bucketHash;
                    string weaponHash = item.itemHash;
                    if (bucketTypeHashToString(bucketHash) == "Invalid")
                        continue;

                    TimeSpan curSpan = stopwatch.Elapsed;
                    apiCallsCount += 2;

                    //Getting name, icon, and element
                    dynamic? wepManifest = getWeaponManifest(weaponHash);

                    bucketHash = wepManifest.Response.inventory.bucketTypeHash;
                    if (wepManifest == null)
                    {
                        Console.WriteLine($"wepManifest : {wepManifest == null}");
                        continue;
                    }
                    else if (bucketTypeHashToString(bucketHash) == "Invalid")
                        continue;

                    //bucketHash for inventory bucket ADD LATER
                    //Getting stats on weapon
                    RestRequest request = new RestRequest($"Platform/Destiny2/{mySettings.membershipType}/Profile/{mySettings.membershipID}/Item/{item.itemInstanceId}/?components=300");
                    RestResponse response = client.Execute(request);
                    dynamic? stats = JsonConvert.DeserializeObject(response.Content);

                    Weapon curWeapon = new Weapon();
                    curWeapon.weaponId = item.itemInstanceId;
                    curWeapon.weaponName = wepManifest.Response.displayProperties.name;
                    curWeapon.weaponType = wepManifest.Response.itemTypeDisplayName;
                    curWeapon.weaponElement = damageHashToString((string)stats.Response.instance.data.damageTypeHash);
                    curWeapon.weaponLevel = stats.Response.instance.data.primaryStat.value;
                    curWeapon.weaponIconLink = wepManifest.Response.displayProperties.icon;
                    curWeapon.weaponSlot = bucketTypeHashToString((string)wepManifest.Response.inventory.bucketTypeHash);

                    database.AddNewWeapon(curWeapon);

                    //Limiting api calls to about 100 every 5 seconds
                    if(apiCallsCount >= 100 && stopwatch.Elapsed.TotalSeconds >= 5)
                    {
                        int waitTime = System.Math.Clamp((int)(10 - stopwatch.Elapsed.TotalSeconds), 0, 10);
                        await Task.Delay(waitTime * 1000);
                        apiCallsCount = 0;
                        stopwatch.Restart();
                    }
                }
            }
        }

        public string damageHashToString(string damageHash)
        {
            switch (damageHash)
            {
                case "3373582085":
                    return "Kinetic";
                case "3949783978":
                    return "Strand";
                case "3454344768":
                    return "Void";
                case "2303181850":
                    return "Arc";
                case "1847026933":
                    return "Solar";
                case "151347233":
                    return "Stasis";
            }

            return "Invalid";
        }

        public string bucketTypeHashToString(string bucketTypeHash)
        {
            switch (bucketTypeHash)
            {
                case "1498876634":
                    return "Kinetic";
                case "2465295065":
                    return "Elemental";
                case "953998645":
                    return "Heavy";
                case "138197802":
                    return "Vault";
            }

            return "Invalid";
        }

        public void saveSettings()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            string settingsPath = System.IO.Path.Join(path, "vaultSettings.txt");
            string jsonString = JsonConvert.SerializeObject(mySettings);
            File.WriteAllText(settingsPath, jsonString);
        }

        public void reset()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            string settingsPath = System.IO.Path.Join(path, "vaultSettings.txt");
            File.Delete(settingsPath);
        }
    }
}
