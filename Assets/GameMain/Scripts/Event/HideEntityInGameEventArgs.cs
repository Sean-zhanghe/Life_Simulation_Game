using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class HideEntityInGameEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(HideEntityInGameEventArgs).GetHashCode();

        public HideEntityInGameEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int EntityId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static HideEntityInGameEventArgs Create(int entityId, object userData = null)
        {
            HideEntityInGameEventArgs hideEntityInGameEventArgs = ReferencePool.Acquire<HideEntityInGameEventArgs>();
            hideEntityInGameEventArgs.EntityId = entityId;
            return hideEntityInGameEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
