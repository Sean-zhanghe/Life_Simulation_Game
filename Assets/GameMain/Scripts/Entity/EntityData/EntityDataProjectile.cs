using GameFramework;
using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class EntityDataProjectile : EntityData
    {
        public ProjectileData projectileData { get; private set; }

        public Vector2 direction { get; private set; } 

        public EntityDataProjectile() : base()
        {
            projectileData = null;
        }

        public static EntityDataProjectile Create(ProjectileData projectileData, Vector2 direction, object userData = null)
        {
            EntityDataProjectile entityData = ReferencePool.Acquire<EntityDataProjectile>();
            entityData.projectileData = projectileData;
            entityData.direction = direction;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataProjectile Create(ProjectileData projectileData, Vector2 direction, Vector3 position, object userData = null)
        {
            EntityDataProjectile entityData = ReferencePool.Acquire<EntityDataProjectile>();
            entityData.projectileData = projectileData;
            entityData.direction = direction;
            entityData.Position = position;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityDataProjectile Create(ProjectileData projectileData, Vector2 direction, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataProjectile entityData = ReferencePool.Acquire<EntityDataProjectile>();
            entityData.projectileData = projectileData;
            entityData.direction = direction;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            projectileData = null;
        }
    }
}
