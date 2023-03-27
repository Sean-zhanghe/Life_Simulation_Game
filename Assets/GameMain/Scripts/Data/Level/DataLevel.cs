using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce.Data
{
    public sealed class DataLevel : DataBase
    {
        private IDataTable<DRLevel> dtLevel;
        private Dictionary<int, LevelData> dicLevelData;

        public int CurLevelId { get; private set; }
        public int MaxLevelId { get; private set; }

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            if (dtLevel == null)
                throw new System.Exception("Can not get data table Level");

            dicLevelData = new Dictionary<int, LevelData>();

            DRLevel[] dRLevels = dtLevel.GetAllDataRows();
            foreach (var dRLevel in dRLevels)
            {
                LevelData levelData = new LevelData(dRLevel);
                dicLevelData.Add(levelData.Id, levelData);
            }

            CurLevelId = 1;
            MaxLevelId = 1;
        }

        public LevelData GetLevelDataById(int levelId)
        {
            if (dicLevelData.ContainsKey(levelId))
            {
                return dicLevelData[levelId];
            }

            return null;
        }

        public LevelData[] GetAllLevelData()
        {
            int index = 0;
            LevelData[] results = new LevelData[dicLevelData.Count];
            foreach (var levelData in dicLevelData.Values)
            {
                results[index++] = levelData;
            }

            return results;
        }

        public void LoadLevel(int level)
        {
            if (level < 1)
            {
                Log.Error("Load level param invaild '{0}.'", level);
                return;
            }

            if (!dicLevelData.ContainsKey(level))
            {
                Log.Error("Can not found level '{0}.'", level);
                return;
            }
            
            LevelData levelData = dicLevelData[level];

            InternalLoadLevel(levelData);
        }

        private void InternalLoadLevel(LevelData levelData)
        {
            if (CurLevelId != levelData.SceneId)
            {
                CurLevelId = levelData.SceneId;
            }

            GameEntry.Data.GetData<DataPlayer>().Reset();

            GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create(CurLevelId));
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRNPC>();

            dtLevel = null;
            dicLevelData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}
