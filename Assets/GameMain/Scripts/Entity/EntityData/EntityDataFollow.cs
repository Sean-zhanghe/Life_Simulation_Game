using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class EntityDataFollow : EntityData
    {
        public Transform Follow
        {
            get;
            private set;
        }

        public Vector3 Offset
        {
            get;
            private set;
        }

        public Vector3 Scale
        {
            get;
            private set;
        }

        public EnumSound ShowSound
        {
            get;
            private set;
        }

        public EntityDataFollow() : base()
        {
            Follow = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
            ShowSound = EnumSound.None;
        }

        public static EntityDataFollow Create(Transform follow, object userData = null)
        {
            EntityDataFollow entityData = ReferencePool.Acquire<EntityDataFollow>();
            entityData.Follow = follow;
            entityData.UserData = userData;
            return entityData;
        }
        public static EntityDataFollow Create(Transform follow, Vector3 offset, object userData = null)
        {
            EntityDataFollow entityData = ReferencePool.Acquire<EntityDataFollow>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataFollow Create(Transform follow, Vector3 offset, EnumSound enumSound, object userData = null)
        {
            EntityDataFollow entityData = ReferencePool.Acquire<EntityDataFollow>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.ShowSound = enumSound;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataFollow Create(Transform follow, Vector3 offset, Vector3 scale, object userData = null)
        {
            EntityDataFollow entityData = ReferencePool.Acquire<EntityDataFollow>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataFollow Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, object userData = null)
        {
            EntityDataFollow entityData = ReferencePool.Acquire<EntityDataFollow>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.ShowSound = enumSound;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataFollow Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataFollow entityData = ReferencePool.Acquire<EntityDataFollow>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.ShowSound = enumSound;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataFollow Create(EnumSound enumSound, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataFollow entityData = ReferencePool.Acquire<EntityDataFollow>();
            entityData.ShowSound = enumSound;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            Follow = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
            ShowSound = EnumSound.None;
        }
    }
}
