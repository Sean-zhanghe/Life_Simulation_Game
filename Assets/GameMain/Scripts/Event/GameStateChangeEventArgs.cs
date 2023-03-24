using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class GameStateChangeEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(GameStateChangeEventArgs).GetHashCode();

        public GameStateChangeEventArgs()
        {
            LastState = EnumGameState.None;
            CurrentState = EnumGameState.None;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public EnumGameState LastState
        {
            get;
            private set;
        }

        public EnumGameState CurrentState
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static GameStateChangeEventArgs Create(EnumGameState lastState, EnumGameState currentState, object userData = null)
        {
            GameStateChangeEventArgs gameStateChangeEventArgs = ReferencePool.Acquire<GameStateChangeEventArgs>();
            gameStateChangeEventArgs.LastState = lastState;
            gameStateChangeEventArgs.CurrentState= currentState;
            gameStateChangeEventArgs.UserData = userData;
            return gameStateChangeEventArgs;
        }

        public override void Clear()
        {
            LastState = EnumGameState.None;
            CurrentState = EnumGameState.None;
        }
    }
}
