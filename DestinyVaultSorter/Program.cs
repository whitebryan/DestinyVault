using DestinyVaultSorter;

//WeaponDatabase weaponData = new WeaponDatabase();

/*
weaponData.AddNewWeapon(2, "Weapon2", "Shotgun", "Void", 1210);
weaponData.AddNewWeapon(3, "Weapon3", "Shotgun", "Solar", 1001);
weaponData.AddNewWeapon(4, "Weapon4", "Shotgun", "Arc", 1050);
weaponData.AddNewWeapon(5, "Weapon5", "Shotgun", "Strand", 1300);
weaponData.AddNewWeapon(6, "Weapon6", "Shotgun", "Stasis", 1200);
weaponData.AddNewWeapon(7, "Weapon7", "Sniper", "Arc", 1210);
weaponData.AddNewWeapon(8, "Weapon8", "Sniper", "Arc", 1050);
weaponData.AddNewWeapon(9, "Weapon9", "Bow", "Arc", 1050);
weaponData.AddNewWeapon(10, "Weapon10", "Bow", "Strand", 1300);
weaponData.AddNewWeapon(11, "Weapon11", "SMG", "Stasis", 1200);
*/

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

