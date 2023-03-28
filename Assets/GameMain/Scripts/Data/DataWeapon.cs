using GameFramework.DataTable;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public sealed class WeaponData
    {
        private DRWeapon dRWeapon;

        public int Id { get { return dRWeapon.Id; } }

        public string Name { get { return dRWeapon.Name; } }

        public int WeaponType { get { return dRWeapon.WeaponType; } }

        public int ProjectileId { get { return dRWeapon.ProjectileId; } }

        public string ProjectileType { get { return dRWeapon.ProjectileType; } }

        public float Damage { get { return dRWeapon.Damage; } }

        public string Parameter { get { return dRWeapon.Paramater; } }

        public WeaponData(DRWeapon dRWeapon)
        {
            this.dRWeapon = dRWeapon;
        }
    }


    public sealed class DataWeapon : DataBase
    {
        private IDataTable<DRWeapon> dtWeapon;
        private Dictionary<int, WeaponData> dicWeaponData;

        protected override void OnInit()
        {
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtWeapon = GameEntry.DataTable.GetDataTable<DRWeapon>();
            if (dtWeapon == null)
                throw new System.Exception("Can not get data table Sound");

            dicWeaponData = new Dictionary<int, WeaponData>();

            DRWeapon[] dRWeapons = dtWeapon.GetAllDataRows();
            foreach (var dRWeapon in dRWeapons)
            {
                WeaponData weaponData = new WeaponData(dRWeapon);
                dicWeaponData.Add(weaponData.Id, weaponData);
            }
        }

        public WeaponData GetWeaponDataById(int weaponId)
        {
            if (dicWeaponData.ContainsKey(weaponId))
            {
                return dicWeaponData[weaponId];
            }

            return null;
        }

        public WeaponData[] GetAllWeaponData()
        {
            int index = 0;
            WeaponData[] results = new WeaponData[dicWeaponData.Count];
            foreach (var weaponData in dicWeaponData.Values)
            {
                results[index++] = weaponData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRProjectile>();

            dtWeapon = null;
            dicWeaponData = null;
        }

        protected override void OnShutdown()
        {
        }
    }
}