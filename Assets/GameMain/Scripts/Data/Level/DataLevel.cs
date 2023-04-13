using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Cinemachine.DocumentationSortingAttribute;

namespace StarForce.Data
{
    public sealed class DataLevel : DataBase
    {
        private IDataTable<DRLevel> dtLevel;
        private Dictionary<int, LevelData> dicLevelData;
        public List<int> listLevelId;

        private readonly static int NONE_LEVEL_INDEX = -1;
        private readonly static int NONE_LEVEL_ID = -1;
        public int CurLevelIndex { get; private set; }
        public int CurLevelId { get; private set; }
        public int MaxLevelIndex { get; private set; }
        public int MaxLevelId { get; private set; }

        private EnumGameState levelState;

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
            listLevelId = new List<int>();

            DRLevel[] dRLevels = dtLevel.GetAllDataRows();
            foreach (var dRLevel in dRLevels)
            {
                LevelData levelData = new LevelData(dRLevel);
                dicLevelData.Add(levelData.Id, levelData);
                listLevelId.Add(levelData.Id);
            }

            listLevelId.Sort();

            CurLevelIndex = NONE_LEVEL_INDEX;
            MaxLevelIndex = 0;
            CurLevelId = NONE_LEVEL_ID;
            MaxLevelId = listLevelId[MaxLevelIndex];
            levelState = EnumGameState.None;
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

        public void ChangeLevelState(EnumGameState state)
        {
            if (levelState == state) return;

            levelState = state;
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
            bool isReload = true;

            if (CurLevelId != levelData.Id)
            {
                for (int i = 0; i < listLevelId.Count; i++)
                {
                    if (listLevelId[i] == levelData.Id)
                    {
                        CurLevelIndex = i;
                        CurLevelId = listLevelId[i];
                        break;
                    }
                }
                isReload = false;
            }

            ChangeLevelState(EnumGameState.GameNormal);

            GameEntry.Data.GetData<DataPlayer>().Reset();

            if (isReload)
            {
                GameEntry.Event.Fire(this, ReloadLevelEventArgs.Create(levelData));
            }
            else
            {
                GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create(levelData.SceneId));
            }
        }

        public void LevelGameSuccess()
        {
            if (levelState != EnumGameState.GameNormal) return;

            // TODO 发放通关奖励
            LevelData levelData = GetLevelDataById(CurLevelId);
            DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataPlayer.AddRewardByConfiger(levelData.Reward);

            if (CurLevelId == MaxLevelId)
            {
                MaxLevelIndex++;
                if (MaxLevelIndex >= listLevelId.Count)
                {
                    MaxLevelIndex = listLevelId.Count - 1;
                }
                MaxLevelId = listLevelId[MaxLevelIndex];
            }

            ChangeLevelState(EnumGameState.Gameover);
            GameEntry.Event.Fire(this, GameoverEventArgs.Create(EnumGameOverType.Success));
        }

        public void LevelGameFail()
        {
            if (levelState != EnumGameState.GameNormal) return;

            ChangeLevelState(EnumGameState.Gameover);
            GameEntry.Event.Fire(this, GameoverEventArgs.Create(EnumGameOverType.Fail));
        }

        public void LoadNextLevel()
        {
            // 最后一关
            int nextLevelIndex = CurLevelIndex + 1;
            if (nextLevelIndex >= listLevelId.Count)
            {
                nextLevelIndex = listLevelId.Count - 1;
                return;
            }

            int nextLevelId = listLevelId[nextLevelIndex];

            LoadLevel(nextLevelId);
        }

        public void ExitLevel()
        {
            if (CurLevelIndex == NONE_LEVEL_INDEX) return;

            CurLevelIndex = NONE_LEVEL_INDEX;
            CurLevelId = NONE_LEVEL_ID;
            ChangeLevelState(EnumGameState.None);

            GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create((int)EnumScene.LevelMenu));
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRNPC>();

            dtLevel = null;
            dicLevelData = null;
            levelState = EnumGameState.None;
        }

        protected override void OnShutdown()
        {
        }
    }

}
