using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml;

namespace DestinyVaultSorter
{
    public class WeaponDatabase
    {
        public WeaponDatabase() 
        {
            myDatabase = new WeaponContext();
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
            myDatabase.Remove(id);
            myDatabase.SaveChanges();
        }

        public int getWeaponCount()
        {
            return myDatabase.Weapons.Count();
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

