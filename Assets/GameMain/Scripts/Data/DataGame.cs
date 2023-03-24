using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce.Data
{
    public class DataGame : DataBase
    {
        // 暂停前游戏状态
        private EnumGameState stateBeforePause;

        public EnumGameState gameState { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            // 初始化游戏状态
            gameState = EnumGameState.Normal;
        }

        private void ChangeGameState(EnumGameState targetgameState)
        {
            if (gameState == targetgameState) return;

            stateBeforePause = gameState;
            gameState = targetgameState;
            GameEntry.Event.Fire(this, GameStateChangeEventArgs.Create(stateBeforePause, gameState));
        }

        public void GamePause()
        {
            if (gameState != EnumGameState.Normal)
            {
                Log.Error("Only can pause when game is in Normal, now is {0}", gameState.ToString());
                return;
            }
            ChangeGameState(EnumGameState.Pause);
        }

        public void GameResume()
        {
            if (gameState != EnumGameState.Pause)
            {
                Log.Error("Only can resume when game is in Pause state, now is {0}", gameState.ToString());
                return;
            }
            ChangeGameState(stateBeforePause);
        }
    }

}
