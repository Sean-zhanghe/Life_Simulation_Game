using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class SceneControl : IReference
    {
        private bool pause = false;
        public string lastScene;
        public string currentScene;

        private EntityLoader entityLoader;

        private DataScene dataScene;
        private DataNPC dataNPC;
        private Dictionary<int, EntityLogicNPC> dicEntityNPC;

        public SceneControl()
        {
            dicEntityNPC = new Dictionary<int, EntityLogicNPC>();
        }

        public static SceneControl Create()
        {
            SceneControl sceneControl = ReferencePool.Acquire<SceneControl>();
            return sceneControl;
        }
        public void Initialize()
        {
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadGameSceneEventArgs.EventId, OnLoadGameScene);

            entityLoader = EntityLoader.Create(this);

            dataScene = GameEntry.Data.GetData<DataScene>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();

            lastScene = "Character";
            currentScene = "MainGame";

            // 创建玩家
            CreatePlayer();

            CreaterNPC();
        }
        public void Clear()
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadGameSceneEventArgs.EventId, OnLoadGameScene);
        }

        public void Pause()
        {
            pause = true;

            //foreach (var entity in entityLoader.GetAllEntities())
            //{
            //    IPause iPause = entity.Logic as IPause;
            //    if (iPause != null)
            //        iPause.Pause();
            //}
            entityLoader.HideAllEntity();
        }

        public void Resume()
        {
            pause = false;

            //foreach (var entity in entityLoader.GetAllEntities())
            //{
            //    IPause iPause = entity.Logic as IPause;
            //    if (iPause != null)
            //        iPause.Resume();
            //}
            CreatePlayer();
        }

        public void CreatePlayer()
        {
            int characterId = GameEntry.Setting.GetInt(Constant.DataNode.CharacterId);
            string playerName = GameEntry.Setting.GetString(Constant.DataNode.PlayerName);

            int playerId = GameEntry.Data.GetData<DataCharacter>().GetCharacterData(characterId).PlayerId;
            Player player = GameEntry.Data.GetData<DataPlayer>().CreatePlayer(playerId, playerName);

            // TODO 设置玩家属性


            // 设置玩家位置
            int curSceneId = GetSceneId(currentScene);
            if (curSceneId == 0)
            {
                Log.Error("Can not get current scene {0} id.", curSceneId);
                return;
            }

            int lastSceneId = GetSceneId(lastScene);
            if (lastSceneId == 0)
            {
                Log.Error("Can not get last scene {0} id.", lastSceneId);
                return;
            }

            Vector3 pos = dataScene.GetPlayerPosition(curSceneId, lastSceneId);
            entityLoader.ShowEntity<EntityLogicPlayer>(EnumEntity.Player, null, EntityDataPlayer.Create(player, pos));
        }

        public void CreaterNPC()
        {
            GameObject[] NPCs = GameObject.FindGameObjectsWithTag(Constant.Tag.NPC);
            foreach (var npc in NPCs)
            {
                int NPCId = int.Parse(npc.name);
                NPCData npcData = dataNPC.GetNPCDataById(NPCId);
                if (npcData == null) continue;

                // 创建NPC
                entityLoader.ShowEntity(
                    npcData.EntityId, 
                    typeof(EntityLogicNPC),
                    (entity) => { dicEntityNPC.Add(entity.Id, (EntityLogicNPC)entity.Logic); },
                    EntityDataNPC.Create(npcData, npc.transform.position)
                );
            }
        }

        private int GetSceneId(string sceneName)
        {
            SceneData[] scenes = dataScene.GetAllSceneData();
            foreach (var scene in scenes)
            {
                if (scene.AssetName == sceneName)
                {
                    return scene.Id;
                }
            }
            return 0;
        }

        public void Transition(int to)
        {

            // 停止所有声音
            //GameEntry.Sound.StopAllLoadingSounds();
            //GameEntry.Sound.StopAllLoadedSounds();

            // 隐藏所有实体
            entityLoader.HideAllEntity();
            //GameEntry.Entity.HideAllLoadingEntities();
            //GameEntry.Entity.HideAllLoadedEntities();

            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            SceneData scene = dataScene.GetSceneDataById(to);
            if (scene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", to.ToString());
                return;
            }

            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(scene.AssetName), Constant.AssetPriority.SceneAsset, this);
        }

        private void OnLoadGameScene(object sender, GameEventArgs e)
        {
            LoadGameSceneEventArgs ne = (LoadGameSceneEventArgs)e;
            if (ne == null)
            {
                return;
            }

            Transition(ne.To);
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            lastScene = currentScene;
            currentScene = ne.SceneAssetName.Substring(Constant.Path.Scenes.Length, ne.SceneAssetName.Length - Constant.Path.Scenes.Length - ".unity".Length);
            
            CreatePlayer();

            CreaterNPC();
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }
    }

}
