using GameFramework.DataTable;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public sealed class SceneData
    {
        private DRScene dRScene;

        public int Id { get { return dRScene.Id; } }

        public string AssetName { get { return dRScene.AssetName; } }

        public int BackgroundMusicId { get { return dRScene.BackgroundMusicId; } }

        public string Procedure { get { return dRScene.Procedure; } }

        public string PlayerPosition { get { return dRScene.PlayerPosition; } }

        public SceneData(DRScene dRScene)
        {
            this.dRScene = dRScene;
        }
    }


    public sealed class DataScene : DataBase
    {
        private IDataTable<DRScene> dtScene;
        private Dictionary<int, SceneData> dicSceneData;
        public string scene;

        protected override void OnInit()
        {
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            if (dtScene == null)
                throw new System.Exception("Can not get data table Sound");

            dicSceneData = new Dictionary<int, SceneData>();

            DRScene[] dRScenes = dtScene.GetAllDataRows();
            foreach (var dRScene in dRScenes)
            {
                SceneData sceneData = new SceneData(dRScene);
                dicSceneData.Add(sceneData.Id, sceneData);
            }
        }

        public SceneData GetSceneDataById(int sceneId)
        {
            if (dicSceneData.ContainsKey(sceneId))
            {
                return dicSceneData[sceneId];
            }

            return null;
        }

        public SceneData[] GetAllSceneData()
        {
            int index = 0;
            SceneData[] results = new SceneData[dicSceneData.Count];
            foreach (var sceneData in dicSceneData.Values)
            {
                results[index++] = sceneData;
            }

            return results;
        }

        public int GetSceneIdByName(string sceneName)
        {
            SceneData[] scenes = this.GetAllSceneData();
            foreach (var scene in scenes)
            {
                if (scene.AssetName == sceneName)
                {
                    return scene.Id;
                }
            }
            return 0;
        }

        public Vector3 GetPlayerPosition(int curSceneId, int lastSceneId)
        {
            Vector3 playerPos = Vector3.zero;
            SceneData sceneData = GetSceneDataById(curSceneId);
            string[] positions = sceneData.PlayerPosition.Split('|');
            foreach (var pos in positions)
            {
                string[] detail = pos.Split(':');
                int scene = int.Parse(detail[0]);

                string[] posDetail = detail[1].Split(',');
                Vector3 temp = new Vector3(float.Parse(posDetail[0]), float.Parse(posDetail[1]), float.Parse(posDetail[2]));

                // 场景玩家默认位置
                if (scene == 0)
                {
                    playerPos = temp;
                }

                if (scene == lastSceneId)
                {
                    playerPos = temp;
                    return playerPos;
                }
            }
            return playerPos;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRScene>();

            dtScene = null;
            dicSceneData = null;
        }

        protected override void OnShutdown()
        {
        }
    }
}