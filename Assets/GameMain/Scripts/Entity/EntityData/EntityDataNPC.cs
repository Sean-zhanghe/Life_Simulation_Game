using System;
using UnityEngine;
using GameFramework;
using StarForce.Data;

namespace StarForce
{
    [Serializable]
    public class EntityDataNPC : EntityData
    {
        public NPCData data

        {
            get;
            private set;
        }

        public EntityDataNPC() : base()
        {
            data = null;
        }

        public static EntityDataNPC Create(NPCData data, object userData = null)
        {
            EntityDataNPC entityData = ReferencePool.Acquire<EntityDataNPC>();
            entityData.data = data;
            return entityData;
        }

        public static EntityDataNPC Create(NPCData data, Vector3 position, object userData = null)
        {
            EntityDataNPC entityData = ReferencePool.Acquire<EntityDataNPC>();
            entityData.data = data;
            entityData.Position = position;
            entityData.Rotation = Quaternion.identity;
            return entityData;
        }

        public static EntityDataNPC Create(NPCData data, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataNPC entityData = ReferencePool.Acquire<EntityDataNPC>();
            entityData.data = data;
            entityData.Position = position;
            entityData.Rotation = rotation;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();

            data = null;
        }
    }
}


