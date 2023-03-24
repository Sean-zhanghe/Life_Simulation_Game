using System;
using UnityEngine;
using GameFramework;
using StarForce.Data;

namespace StarForce
{
    [Serializable]
    public class EntityDataPlayer : EntityData
    {
        public Player Player

        {
            get;
            private set;
        }

        public EntityDataPlayer() : base()
        {
            Player = null;
        }

        public static EntityDataPlayer Create(Player player, object userData = null)
        {
            EntityDataPlayer entityData = ReferencePool.Acquire<EntityDataPlayer>();
            entityData.Player = player;
            return entityData;
        }

        public static EntityDataPlayer Create(Player player, Vector3 position, object userData = null)
        {
            EntityDataPlayer entityData = ReferencePool.Acquire<EntityDataPlayer>();
            entityData.Player = player;
            entityData.Position = position;
            entityData.Rotation = Quaternion.identity;
            return entityData;
        }

        public static EntityDataPlayer Create(Player player, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataPlayer entityData = ReferencePool.Acquire<EntityDataPlayer>();
            entityData.Player = player;
            entityData.Position = position;
            entityData.Rotation = rotation;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();

            Player = null;
        }
    }
}


