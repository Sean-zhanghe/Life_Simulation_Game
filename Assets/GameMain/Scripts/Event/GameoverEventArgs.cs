using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class GameoverEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(GameoverEventArgs).GetHashCode();

        public GameoverEventArgs()
        {
            EnumGameOverType = EnumGameOverType.Fail;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public EnumGameOverType EnumGameOverType
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static GameoverEventArgs Create(EnumGameOverType enumGameOverType, object userData = null)
        {
            GameoverEventArgs gameoverEventArgs = ReferencePool.Acquire<GameoverEventArgs>();
            gameoverEventArgs.EnumGameOverType = enumGameOverType;
            return gameoverEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
