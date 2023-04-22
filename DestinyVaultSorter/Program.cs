using DestinyVaultSorter;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Nodes;

WeaponDatabase weaponData = new WeaponDatabase();

//Pulling Settings from file or from user input
BungieAPISettings settings = new BungieAPISettings();
var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
string settingsPath = System.IO.Path.Join(path, "vaultSettings.txt");
if(File.Exists(settingsPath))
{
    settings = JsonConvert.DeserializeObject<BungieAPISettings>(File.ReadAllText(settingsPath));
}
else
{
    Console.WriteLine("Please enter your API key : ");
    settings.APIKey = Console.ReadLine();
    Console.WriteLine("Please enter your client_ID : ");
    settings.client_ID = Console.ReadLine();
    Console.WriteLine("Please enter your client_secret: ");
    settings.client_secret = Console.ReadLine();

    Console.WriteLine("Authorize in the opened web page then save the code at the end of url to enter as the AUTH CODE");
    //Opens OAuthpage then redirects to my github page with the code in the url
    Process.Start(new ProcessStartInfo("https://www.bungie.net/en/oauth/authorize?client_id=" + settings.client_ID + "&response_type=code") { UseShellExecute = true });
    Console.WriteLine("Please enter the auth code you got from the redirect : ");
    settings.authCode = Console.ReadLine();

    if (settings.APIKey == null || settings.client_ID == null || settings.client_secret == null || settings.authCode == null)
    {
        throw new Exception("These settings cannot be null");
    }
    else
    {
        FileStream settingsFile = File.Create(settingsPath);
        settingsFile.Close();
        string jsonString = JsonConvert.SerializeObject(settings);
        File.WriteAllText(settingsPath, jsonString);
    }
}

BungieAPIHandler bungieAPI = new BungieAPIHandler(settings);
bungieAPI.getAllOwnedWeapons();

/*
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
*/
