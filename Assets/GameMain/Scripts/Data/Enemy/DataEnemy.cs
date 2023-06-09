﻿using GameFramework;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce.Data
{
    public class DataEnemy : DataBase
    {
        private IDataTable<DREnemy> dtEnemy;
        private Dictionary<int, EnemyData> dicEnemyData;

        protected override void OnInit()
        {

        }

        protected override void OnLoad()
        {
            dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            if (dtEnemy == null)
                throw new System.Exception("Can not get data table Enemy");

            dicEnemyData = new Dictionary<int, EnemyData>();

            DREnemy[] dREnemies = dtEnemy.GetAllDataRows();
            foreach (var drEnemy in dREnemies)
            {
                EnemyData enemyData = new EnemyData(drEnemy);
                dicEnemyData.Add(drEnemy.Id, enemyData);
            }
        }

        public EnemyData GetEnemyDataById(int id)
        {
            if (dicEnemyData.ContainsKey(id))
            {
                return dicEnemyData[id];
            }

            return null;
        }

        public EnemyData[] GetAllEnemyData()
        {
            int index = 0;
            EnemyData[] results = new EnemyData[dicEnemyData.Count];
            foreach (var enemyData in dicEnemyData.Values)
            {
                results[index++] = enemyData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DREnemy>();

            dtEnemy = null;
            dicEnemyData = null;
        }

        protected override void OnShutdown()
        {

        }
    }
}
