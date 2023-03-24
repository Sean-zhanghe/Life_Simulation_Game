using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class EnterGameEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(EnterGameEventArgs).GetHashCode();

        public EnterGameEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int SerialId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static EnterGameEventArgs Create(int serialId, EnumPriority priorityType, float lastPriority, float currentPriority, object userData = null)
        {
            EnterGameEventArgs enterGameEventArgs = ReferencePool.Acquire<EnterGameEventArgs>();
            enterGameEventArgs.SerialId = serialId;
            enterGameEventArgs.UserData = userData;
            return enterGameEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
