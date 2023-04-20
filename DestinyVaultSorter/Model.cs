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

        public Weapon(int weaponId, string weaponName, string weaponType, string weaponElement, int weaponLevel)
        {
            this.weaponId = weaponId;
            this.weaponName = weaponName;
            this.weaponType = weaponType;
            this.weaponElement = weaponElement;
            this.weaponLevel = weaponLevel;
        }
    }

}
