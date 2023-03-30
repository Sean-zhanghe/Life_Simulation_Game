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
        private int m_BackgroundMusicId;

        // 在这边记录敌人状态机ID 防止ID重复
        private int ENEMY_FSM_SERIAL_ID = 0;
        private EntityLoader entityLoader;

        private DataScene dataScene;
        private DataNPC dataNPC;
        private DataEnemy dataEnemy;
        private Dictionary<int, EntityLogicNPC> dicEntityNPC;
        private Dictionary<int, EntityLogicEnemy> dicEntityEnemy;

        public SceneControl()
        {
            dicEntityNPC = new Dictionary<int, EntityLogicNPC>();
            dicEntityEnemy = new Dictionary<int, EntityLogicEnemy>();
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
            GameEntry.Event.Subscribe(ShowEntityInGameEventArgs.EventId, OnShowEntityInGame);
            GameEntry.Event.Subscribe(HideEntityInGameEventArgs.EventId, OnHideEntityInGame);

            entityLoader = EntityLoader.Create(this);

            dataScene = GameEntry.Data.GetData<DataScene>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            dataEnemy = GameEntry.Data.GetData<DataEnemy>();

            lastScene = "Character";
            currentScene = "MainGame";

            CreaterNPC();
            CreaterEnemy();
        }
        public void Clear()
        {
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadGameSceneEventArgs.EventId, OnLoadGameScene);
            GameEntry.Event.Unsubscribe(ShowEntityInGameEventArgs.EventId, OnShowEntityInGame);
            GameEntry.Event.Unsubscribe(HideEntityInGameEventArgs.EventId, OnHideEntityInGame);
        }

        public void Pause()
        {
            pause = true;

            foreach (var entity in entityLoader.GetAllEntities())
            {
                IPause iPause = entity.Logic as IPause;
                if (iPause != null)
                    iPause.Pause();
            }
        }

        public void Resume()
        {
            pause = false;

            foreach (var entity in entityLoader.GetAllEntities())
            {
                IPause iPause = entity.Logic as IPause;
                if (iPause != null)
                    iPause.Resume();
            }
        }

        public void Restart()
        {

        }

        public void CreatePlayer<T>() where T : EntityLogic
        {
            int characterId = GameEntry.Setting.GetInt(Constant.DataNode.CharacterId);
            string playerName = GameEntry.Setting.GetString(Constant.DataNode.PlayerName);

            int playerId = GameEntry.Data.GetData<DataCharacter>().GetCharacterData(characterId).PlayerId;
            Player player = GameEntry.Data.GetData<DataPlayer>().CreatePlayer(playerId, playerName);

            // TODO 设置玩家属性


            // 设置玩家位置
            int curSceneId = dataScene.GetSceneIdByName(currentScene);
            if (curSceneId == 0)
            {
                Log.Error("Can not get current scene {0} id.", curSceneId);
                return;
            }

            int lastSceneId = dataScene.GetSceneIdByName(lastScene);
            if (lastSceneId == 0)
            {
                Log.Error("Can not get last scene {0} id.", lastSceneId);
                return;
            }

            Vector3 pos = dataScene.GetPlayerPosition(curSceneId, lastSceneId);
            entityLoader.ShowEntity<T>(player.EntityId, null, EntityDataPlayer.Create(player, pos));
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

        public void CreaterEnemy()
        {
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag(Constant.Tag.Enemy);
            foreach (var enemy in Enemys)
            {
                int enemyId = int.Parse(enemy.name);
                EnemyData enemyData = dataEnemy.GetEnemyDataById(enemyId);
                if (enemyData == null) continue;

                // 创建Enemy
                entityLoader.ShowEntity(
                    enemyData.EntityId,
                    TypeUtility.GetEntityType(enemyData.EnemyType),
                    (entity) => { dicEntityEnemy.Add(entity.Id, (EntityLogicEnemy)entity.Logic); },
                    EntityDataEnemy.Create(enemyData, ENEMY_FSM_SERIAL_ID++, enemy.transform.position)
                );
            }
        }


        public void Transition(int to)
        {

            // 停止所有声音
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

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

            m_BackgroundMusicId = scene.BackgroundMusicId;
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

            if (m_BackgroundMusicId > 0)
            {
                GameEntry.Sound.PlayMusic(m_BackgroundMusicId);
            }

            GameEntry.Event.Fire(this, LoadSceneCompleteEventArgs.Create(lastScene, currentScene));

            CreaterNPC();
            CreaterEnemy();
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

        private void OnShowEntityInGame(object sender, GameEventArgs e)
        {
            ShowEntityInGameEventArgs ne = (ShowEntityInGameEventArgs)e;
            if (ne == null)
                return;

            entityLoader.ShowEntity(ne.EntityId, ne.Type, ne.ShowSuccess, ne.EntityData);
        }

        private void OnHideEntityInGame(object sender, GameEventArgs e)
        {
            HideEntityInGameEventArgs ne = (HideEntityInGameEventArgs)e;
            if (ne == null)
                return;

            entityLoader.HideEntity(ne.EntityId);
        }
    }

}
