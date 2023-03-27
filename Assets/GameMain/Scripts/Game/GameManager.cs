using GameFramework;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StarForce
{
    public class GameManager : IReference
    {
        private bool pause = false;

        private GameModuleFsmMgr gameModuleFsmMgr = null;
        public TaskManager taskManager = null;
        public SceneControl sceneControl = null;
        public AchievementManager achievementManager = null;
        private CursorManager cursorManager = null;


        public GameManager()
        {

        }

        public void OnEnter()
        {
            // 创建任务管理模块
            taskManager = TaskManager.Create();
            taskManager.Initialize();

            // 创建场景管理模块
            sceneControl = SceneControl.Create();
            sceneControl.Initialize();

            // 成就管理模块
            achievementManager = AchievementManager.Create();
            achievementManager.Initialize();

            // 创建光标管理
            cursorManager = CursorManager.Create();

            // 创建游戏模块管理状态机
            gameModuleFsmMgr = GameModuleFsmMgr.Create(this);
            gameModuleFsmMgr.Initialize();

        }


        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            cursorManager.Update(elapseSeconds, realElapseSeconds);
        }

        public void Pause()
        {
            pause = true;

            gameModuleFsmMgr.Pause();
            sceneControl.Pause();
        }

        public void Resume()
        {
            pause = false;

            gameModuleFsmMgr.Resume();
            sceneControl.Resume();
        }

        public void Restart()
        {
            gameModuleFsmMgr.Restart();
            sceneControl.Restart();
        }

        public void Quick()
        {
            if (pause)
            {
                Resume();
                pause = false;
            }

            ReferencePool.Release(gameModuleFsmMgr);
            ReferencePool.Release(taskManager);
            ReferencePool.Release(sceneControl);
        }

        public static GameManager Create()
        {
            GameManager gameManager = ReferencePool.Acquire<GameManager>();
            return gameManager;
        }

        public void Clear()
        {
        }
    }
}
