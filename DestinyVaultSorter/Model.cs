using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DestinyVaultSorter
{
    public class WeaponContext : DbContext
    {
        public DbSet<Weapon> Weapons { get; set; }

        public string DbPath { get; }

        public WeaponContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "weapons.db");
        }

        public WeaponContext(string dbName)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, dbName+".db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
             => options.UseSqlite($"Data Source={DbPath}");
    }

    [PrimaryKey(nameof(weaponId))]
    public class Weapon
    {
        public int weaponId { get; set; }
        public string weaponName { get; set;}
        public string weaponType { get; set;}
        public string weaponElement { get; set; }
        public int weaponLevel { get; set; }
        public string weaponIconLink { get; set; } 

        public Weapon(int weaponId, string weaponName, string weaponType, string weaponElement, int weaponLevel, string weaponIcon)
        {
            this.weaponId = weaponId;
            this.weaponName = weaponName;
            this.weaponType = weaponType;
            this.weaponElement = weaponElement;
            this.weaponLevel = weaponLevel;
            this.weaponIconLink = weaponIcon;
        }

        public Weapon(int weaponId, BungieAPIHandler APIHandler)
        {
            this.weaponId = weaponId;
            this.weaponName = "";
            this.weaponType = "";
            this.weaponElement = "";
            this.weaponLevel = 1;
            this.weaponIconLink = "";
            populateUsingManifestData(APIHandler);
        }

        private void populateUsingManifestData(BungieAPIHandler APIHandler)
        {
            dynamic? weaponData = APIHandler.getWeaponManifest(weaponId.ToString());
            if (weaponData != null) 
            {
                weaponName = weaponData.Response.displayProperties.name;
                weaponIconLink = weaponData.Response.displayProperties.icon;

                //Splitting type from weapon_type.TYPE string
                string weaponTypeTrait = weaponData.Response.traitIds[1];
                weaponType = weaponTypeTrait.Split('.')[1];

                //Setting damage type based off hash provided
                string dmgTypeHash = weaponData.Response.defaultDamageTypeHash;
                switch (dmgTypeHash)
                {
                    case "3373582085":
                        weaponElement = "Kinetic";
                        break;
                    case "3949783978":
                        weaponElement = "Strand";
                        break;
                    case "3454344768":
                        weaponElement = "Void";
                        break;
                    case "2303181850":
                        weaponElement = "Arc";
                        break;
                    case "1847026933":
                        weaponElement = "Solar";
                        break;
                    case "151347233":
                        weaponElement = "Stasis";
                        break;
                }

                weaponLevel = weaponData.Response.investmentStats[1].value;
            }
        }
    }

}
