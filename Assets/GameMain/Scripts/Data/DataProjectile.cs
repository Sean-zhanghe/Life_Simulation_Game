using GameFramework.DataTable;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public sealed class ProjectileData
    {
        private DRProjectile dRProjectile;

        public int Id { get { return dRProjectile.Id; } }

        public int ProjectileEntityId { get { return dRProjectile.ProjectileEntityId; } }

        public float Speed { get { return dRProjectile.Speed; } }

        public int VFXEntityId { get { return dRProjectile.VFXEntityId; } }

        public float Damage { get { return dRProjectile.Damage; } }

        public ProjectileData(DRProjectile dRProjectile)
        {
            this.dRProjectile = dRProjectile;
        }
    }


    public sealed class DataProjectile : DataBase
    {
        private IDataTable<DRProjectile> dtProjectile;
        private Dictionary<int, ProjectileData> dicProjectileData;

        protected override void OnInit()
        {
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtProjectile = GameEntry.DataTable.GetDataTable<DRProjectile>();
            if (dtProjectile == null)
                throw new System.Exception("Can not get data table Sound");

            dicProjectileData = new Dictionary<int, ProjectileData>();

            DRProjectile[] dRProjectiles = dtProjectile.GetAllDataRows();
            foreach (var dRProjectile in dRProjectiles)
            {
                ProjectileData projectileData = new ProjectileData(dRProjectile);
                dicProjectileData.Add(projectileData.Id, projectileData);
            }
        }

        public ProjectileData GetProjectileDataById(int projectileId)
        {
            if (dicProjectileData.ContainsKey(projectileId))
            {
                return dicProjectileData[projectileId];
            }

            return null;
        }

        public ProjectileData[] GetAllProjectileData()
        {
            int index = 0;
            ProjectileData[] results = new ProjectileData[dicProjectileData.Count];
            foreach (var projectileData in dicProjectileData.Values)
            {
                results[index++] = projectileData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRProjectile>();

            dtProjectile = null;
            dicProjectileData = null;
        }

        protected override void OnShutdown()
        {
        }
    }
}