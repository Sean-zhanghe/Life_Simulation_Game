using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityGameFramework.Runtime;

namespace StarForce
{

    public class ShowEntityInGameEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ShowEntityInGameEventArgs).GetHashCode();

        public ShowEntityInGameEventArgs()
        {
            EntityId = -1;
            Type = null;
            ShowSuccess = null;
            EntityData = null;
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

        public Type Type
        {
            get;
            private set;
        }

        public Action<Entity> ShowSuccess
        {
            get;
            private set;
        }

        public EntityData EntityData
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ShowEntityInGameEventArgs Create(int entityId, Type entityType, Action<Entity> showSuccess, EntityData entityData, object userData = null)
        {
            ShowEntityInGameEventArgs showEntityInGameEventArgs = ReferencePool.Acquire<ShowEntityInGameEventArgs>();
            showEntityInGameEventArgs.EntityId = entityId;
            showEntityInGameEventArgs.Type = entityType;
            showEntityInGameEventArgs.ShowSuccess = showSuccess;
            showEntityInGameEventArgs.EntityData = entityData;
            return showEntityInGameEventArgs;
        }

        public override void Clear()
        {
            EntityId = -1;
            Type = null;
            ShowSuccess = null;
            EntityData = null;
        }
    }
}
