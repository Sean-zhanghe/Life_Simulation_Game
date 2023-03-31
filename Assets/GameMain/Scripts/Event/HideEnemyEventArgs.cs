using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class HideEnemyEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(HideEnemyEventArgs).GetHashCode();

        public HideEnemyEventArgs()
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

        public static HideEnemyEventArgs Create(int entityId, object userData = null)
        {
            HideEnemyEventArgs hideEnemyEventArgs = ReferencePool.Acquire<HideEnemyEventArgs>();
            hideEnemyEventArgs.EntityId = entityId;
            return hideEnemyEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
