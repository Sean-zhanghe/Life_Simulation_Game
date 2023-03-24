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
        private TaskManager taskManager = null;
        private SceneControl sceneControl = null;
        private AchievementManager achievementManager = null;
        private CursorManager cursorManager = null;


        public GameManager()
        {

        }

        public void OnEnter()
        {
            // 创建游戏模块管理状态机
            gameModuleFsmMgr = GameModuleFsmMgr.Create(this);
            gameModuleFsmMgr.Initialize();

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

        }


        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            cursorManager.Update(elapseSeconds, realElapseSeconds);
        }

        public void Pause()
        {
            pause = true;

            sceneControl.Pause();
        }

        public void Resume()
        {
            pause = false;

            sceneControl.Resume();

        }

        public void Restart()
        {

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
