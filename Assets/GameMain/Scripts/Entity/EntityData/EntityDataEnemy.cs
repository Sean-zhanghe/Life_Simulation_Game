using GameFramework;
using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class EntityDataEnemy : EntityData
    {
        public EnemyData enemyData { get; private set; }

        public int FSM_SERIAL_ID { get; private set; }

        public EntityDataEnemy() : base()
        {
            enemyData = null;
        }

        public static EntityDataEnemy Create(EnemyData enemyData, int id, object userData = null)
        {
            EntityDataEnemy entityDataEnemy = ReferencePool.Acquire<EntityDataEnemy>();
            entityDataEnemy.enemyData = enemyData;
            entityDataEnemy.FSM_SERIAL_ID = id;
            entityDataEnemy.UserData = userData;
            return entityDataEnemy;
        }

        public static EntityDataEnemy Create(EnemyData enemyData, int id, Vector3 position, object userData = null)
        {
            EntityDataEnemy entityDataEnemy = ReferencePool.Acquire<EntityDataEnemy>();
            entityDataEnemy.enemyData = enemyData;
            entityDataEnemy.FSM_SERIAL_ID = id;
            entityDataEnemy.Position = position;
            entityDataEnemy.UserData = userData;
            return entityDataEnemy;
        }

        public static EntityDataEnemy Create(EnemyData enemyData, int id, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataEnemy entityDataEnemy = ReferencePool.Acquire<EntityDataEnemy>();
            entityDataEnemy.enemyData = enemyData;
            entityDataEnemy.FSM_SERIAL_ID = id;
            entityDataEnemy.Position = position;
            entityDataEnemy.Rotation = rotation;
            entityDataEnemy.UserData = userData;
            return entityDataEnemy;
        }

        public override void Clear()
        {
            base.Clear();

            enemyData = null;
        }
    }
}
