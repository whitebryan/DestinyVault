using DestinyVaultSorter;

namespace UnitTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void ClearDataBase()
        {
            WeaponDatabase weaponDatabase = new WeaponDatabase("unitTesting");
            weaponDatabase.clearDatabase();


            weaponDatabase.AddNewWeapon(200, "TestWeapon", "Shotgun", "Arc", 1000);
            weaponDatabase.AddNewWeapon(201, "TestWeapon1", "Shotgun", "Arc", 1000);

            weaponDatabase.clearDatabase();
            var results = weaponDatabase.databaseSearch();

            Assert.IsTrue(results.Count() == 0, "Didn't properly clear");
        }

        [TestMethod]
        public void AddWeaponTest()
        {
            WeaponDatabase weaponDatabase = new WeaponDatabase("unitTesting");
            weaponDatabase.clearDatabase();

            weaponDatabase.AddNewWeapon(200, "TestWeapon", "Shotgun", "Arc", 1000);

            Weapon testWeapon = new Weapon(200, "TestWeapon", "Shotgun", "Arc", 1000);
            var results = weaponDatabase.databaseSearch();

            Assert.IsTrue(results.Count() == 1, "More than one weapon");
            Assert.IsTrue(results[0].weaponName == testWeapon.weaponName && results[0].weaponElement == testWeapon.weaponElement && results[0].weaponType == testWeapon.weaponType && results[0].weaponLevel == testWeapon.weaponLevel, "Weapon not the same");
        }

        [TestMethod]
        public void RemoveWeaponTest()
        {
            WeaponDatabase weaponDatabase = new WeaponDatabase("unitTesting");
            weaponDatabase.clearDatabase();

            weaponDatabase.AddNewWeapon(200, "TestWeapon0", "Shotgun", "Arc", 1000);
            weaponDatabase.AddNewWeapon(201, "TestWeapon1", "SMG", "Arc", 1000);
            weaponDatabase.AddNewWeapon(202, "TestWeapon2", "Shotgun", "Stasis", 1000);

            weaponDatabase.RemoveWeapon(201);

            List<Weapon> results = weaponDatabase.databaseSearch();

            Weapon testWeapon1 = new Weapon(200, "TestWeapon0", "Shotgun", "Arc", 1000);
            Weapon testWeapon2 = new Weapon(202, "TestWeapon2", "Shotgun", "Stasis", 1000);

            bool wrongItemDeleted = results.Contains(testWeapon1) && results.Contains(testWeapon2);

            Assert.IsTrue(results.Count() == 2, "Didn't remove weapon");
            Assert.IsFalse(wrongItemDeleted, "Removed wrong weapon");
            Assert.IsFalse(results.Contains(new Weapon(201, "TestWeapon1", "SMG", "Arc", 1000)), "Weapon didn't remove");
        }

        [TestMethod]
        public void GetWeaponCount()
        {
            WeaponDatabase weaponDatabase = new WeaponDatabase("unitTesting");
            weaponDatabase.clearDatabase();

            weaponDatabase.AddNewWeapon(200, "TestWeapon0", "Shotgun", "Arc", 1000);
            weaponDatabase.AddNewWeapon(201, "TestWeapon1", "SMG", "Arc", 1500);
            weaponDatabase.AddNewWeapon(202, "TestWeapon2", "Sniper", "Void", 1100);
            weaponDatabase.AddNewWeapon(203, "TestWeapon3", "Sniper", "Stasis", 1200);
            weaponDatabase.AddNewWeapon(206, "TestWeapon6", "Sniper", "Stasis", 1300);
            weaponDatabase.AddNewWeapon(204, "TestWeapon4", "Bow", "Void", 1200);
            weaponDatabase.AddNewWeapon(205, "TestWeapon5", "Bow", "Solar", 1900);
            weaponDatabase.AddNewWeapon(207, "TestWeapon7", "Grenade Launcher", "Solar", 1000);

            var noParams = weaponDatabase.getWeaponCount();
            var oneParam = weaponDatabase.getWeaponCount("Stasis");
            var twoParamsInOrder = weaponDatabase.getWeaponCount("Void", "Sniper");
            var twoParamsOutOfOrder = weaponDatabase.getWeaponCount(null, "Sniper", 1000);
            var threeParams = weaponDatabase.getWeaponCount("Stasis", "Sniper", 1300);


            Assert.IsTrue(noParams == 8, "Didn't return all weapons");
            Assert.IsTrue(oneParam == 2, "Didn't return correct amount of weapons by element");
            Assert.IsTrue(twoParamsInOrder == 1, "Didn't return correct amount of weapons with two params");
            Assert.IsTrue(twoParamsOutOfOrder == 3, "Didn't return correct amount of weapons with two params out of order");
            Assert.IsTrue(threeParams == 1, "Didn't return correct amount of weapons with three params");
        }

        [TestMethod]
        public void WeaponSearch()
        {
            WeaponDatabase weaponDatabase = new WeaponDatabase("unitTesting");
            weaponDatabase.clearDatabase();

            weaponDatabase.AddNewWeapon(200, "TestWeapon0", "Shotgun", "Arc", 1000);
            weaponDatabase.AddNewWeapon(201, "TestWeapon1", "SMG", "Arc", 1500);
            weaponDatabase.AddNewWeapon(202, "TestWeapon2", "Sniper", "Void", 1100);
            weaponDatabase.AddNewWeapon(203, "TestWeapon3", "Sniper", "Stasis", 1200);
            weaponDatabase.AddNewWeapon(206, "TestWeapon6", "Sniper", "Stasis", 1300);
            weaponDatabase.AddNewWeapon(208, "TestWeapon8", "Sniper", "Arc", 1200);
            weaponDatabase.AddNewWeapon(204, "TestWeapon4", "Bow", "Void", 1200);
            weaponDatabase.AddNewWeapon(205, "TestWeapon5", "Bow", "Solar", 1900);
            weaponDatabase.AddNewWeapon(207, "TestWeapon7", "Grenade Launcher", "Solar", 1000);

            List<Weapon> noParams = weaponDatabase.databaseSearch();
            List<Weapon> oneParam = weaponDatabase.databaseSearch("Stasis");
            List<Weapon> twoParams = weaponDatabase.databaseSearch(null, "Sniper", 1200);
            List<Weapon> threeParams = weaponDatabase.databaseSearch("Stasis", "Sniper", 1300);


            Assert.IsTrue(noParams.Count() == 9, "Didn't return all weapons");
            Assert.IsTrue(oneParam.Count() == 2, "Didn't return correct amount of weapons by element");
            foreach(Weapon wep in oneParam)
            {
                if(wep.weaponElement != "Stasis")
                {
                    Assert.Fail("In correct element returned in search");
                }
            }

            Assert.IsTrue(twoParams.Count() == 3, "Didn't return correct amount of weapons with two params");
            foreach (Weapon wep in twoParams)
            {
                if (wep.weaponType != "Sniper")
                {
                    Assert.Fail("In correct type returned in search");
                }
                else if(wep.weaponLevel < 1200)
                {
                    Assert.Fail("Lower level weapon returned in search");
                }
            }

            Weapon testWeapon = new Weapon(206, "TestWeapon6", "Sniper", "Stasis", 1300);
            Assert.IsTrue(threeParams.Count() == 1, "Didn't return correct amount of weapons with three params");
            Assert.IsTrue(threeParams[0].weaponName == testWeapon.weaponName && threeParams[0].weaponElement == testWeapon.weaponElement && threeParams[0].weaponType == testWeapon.weaponType && threeParams[0].weaponLevel == testWeapon.weaponLevel, "Wrong weapon returned for three params");
        }
    }
}