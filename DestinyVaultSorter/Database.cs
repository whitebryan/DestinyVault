using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using static System.Net.WebRequestMethods;

[assembly: InternalsVisibleTo("UnitTests")]
namespace DestinyVaultSorter
{
    [ApiController]
    public class DatabaseController
    {
        private WeaponDatabase myDatabase = new WeaponDatabase();

        [HttpGet]
        [Route("/weapons/search/{weaponElement?}/{weaponType?}/{weaponLevel:int?}/{weaponSlot?}")]
        public List<Weapon> weaponSearch([FromQuery] string? weaponElement = null, [FromQuery] string? weaponType = null, [FromQuery] int? weaponLevel = null, [FromQuery] string? weaponSlot = null)
        {
            return myDatabase.databaseSearch(weaponElement, weaponType, weaponLevel, weaponSlot);  
        }

        [HttpGet]
        [Route("/weapons/count/{weaponElement?}/{weaponType?}/{weaponLevel:int?}/{weaponSlot?}")]
        public int weapoonCount([FromQuery] string? weaponElement = null, [FromQuery] string? weaponType = null, [FromQuery] int? weaponLevel = null, [FromQuery] string? weaponSlot = null)
        {
            return myDatabase.getWeaponCount(weaponElement, weaponType, weaponLevel, weaponSlot);
        }

        [HttpGet]
        [Route("/weapons/icon/{weaponID}")]
        public string getWeaponIconLink(string weaponID)
        {
            Weapon? foundWeapon = myDatabase.getWeaponById(weaponID);
            if(foundWeapon != null)
            {
                return foundWeapon.weaponIconLink;
            }
            return "null";
        }
    }

    public class WeaponDatabase
    {
        public WeaponDatabase()
        {
            myDatabase = new WeaponContext();
        }

        public WeaponDatabase(string dbName)
        {
            myDatabase = new WeaponContext(dbName);
        }

        private WeaponContext myDatabase;
        public void AddNewWeapon(Weapon weaponToAdd)
        {
            myDatabase.Add(weaponToAdd);

            try
            {
                myDatabase.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void RemoveWeapon(string id) 
        {
            Weapon? wepToRemove = myDatabase.Find<Weapon>(id);

            if (wepToRemove != null)
            {
                myDatabase.Remove(wepToRemove);
                myDatabase.SaveChanges();
            }
        }

        //Only used for unit testing
        public void clearDatabase()
        {
            IQueryable<Weapon> query = myDatabase.Set<Weapon>();
            myDatabase.RemoveRange(query);
            myDatabase.SaveChanges();
        }


        public int getWeaponCount(string? weaponElement = null, string? weaponType = null, int? weaponLevel = null, string? weaponSlot = null)
        {
            IQueryable<Weapon> query = myDatabase.Set<Weapon>();

            if (weaponElement != null)
            {
                query = query.Where(w => w.weaponElement == weaponElement);
            }

            if (weaponType != null)
            {
                query = query.Where(w => w.weaponType == weaponType);
            }

            if (weaponLevel != null)
            {
                query = query.Where(w => w.weaponLevel >= weaponLevel);
            }

            if(weaponSlot != null)
            {
                query = query.Where(w => w.weaponSlot == weaponSlot);
            }

            return query.Count();
        }


        public List<Weapon> databaseSearch(string? weaponElement = null, string? weaponType = null, int? weaponLevel = null, string? weaponSlot = null)
        {
            IQueryable<Weapon> query = myDatabase.Set<Weapon>();

            if(weaponElement != null) 
            {
                query = query.Where(w => w.weaponElement == weaponElement);
            }

            if(weaponType != null)
            {
                query = query.Where(w => w.weaponType == weaponType);
            }

            if(weaponLevel != null)
            {
                query = query.Where(w => w.weaponLevel >= weaponLevel);
            }

            if (weaponSlot != null)
            {
                query = query.Where(w => w.weaponSlot == weaponSlot);
            }

            return query.ToList<Weapon>();
        }

        public Weapon? getWeaponById(string id) 
        {
            return myDatabase.Find<Weapon>(id);
        }
    }
}

