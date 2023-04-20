using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;

[assembly: InternalsVisibleTo("UnitTests")]
namespace DestinyVaultSorter
{
    [ApiController]
    public class DatabaseController
    {
        private WeaponDatabase myDatabase = new WeaponDatabase();

        [HttpGet]
        [Route("/weapons/search/{weaponElement?}/{weaponType?}/{weaponLevel:int?}")]
        public List<Weapon> weaponSearch([FromQuery] string? weaponElement = null, [FromQuery] string? weaponType = null, [FromQuery] int? weaponLevel = null)
        {
            return myDatabase.databaseSearch(weaponElement, weaponType, weaponLevel);  
        }

        [HttpGet]
        [Route("/weapons/count/{weaponElement?}/{weaponType?}/{weaponLevel:int?}")]
        public int weapoonCount([FromQuery] string? weaponElement = null, [FromQuery] string? weaponType = null, [FromQuery] int? weaponLevel = null)
        {
            return myDatabase.getWeaponCount(weaponElement, weaponType, weaponLevel);
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

        public void AddNewWeapon(int id, string weaponName, string weaponType, string element, int weaponLevel)
        {
            myDatabase.Add(new Weapon(id, weaponName, weaponType, element, weaponLevel));

            try
            {
                myDatabase.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void RemoveWeapon(int id) 
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


        public int getWeaponCount(string? weaponElement = null, string? weaponType = null, int? weaponLevel = null)
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

            return query.Count();
        }


        public List<Weapon> databaseSearch(string? weaponElement = null, string? weaponType = null, int? weaponLevel = null)
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

            return query.ToList<Weapon>();
            //return myDatabase.Weapons.Where(w => w.weaponId > 4).ToArray();
            //return myDatabase.Weapons.FromSqlRaw($"SELECT weaponId FROM Weapons WHERE {columnName} {condition0} {condition1}").ToArray();
            //return myDatabase.Weapons.Where(b => b.weaponId > 0).ToArray();
        }
    }
}

