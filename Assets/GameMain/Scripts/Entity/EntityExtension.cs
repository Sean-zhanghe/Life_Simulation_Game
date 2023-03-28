//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using StarForce.Data;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static void ShowEntity<T>(this EntityComponent entityComponent, int serialId, EnumEntity enumEntity, object userData = null)
        {
            entityComponent.ShowEntity(serialId, enumEntity, typeof(T), userData);
        }

        public static void ShowEntity(this EntityComponent entityComponent, int serialId, EnumEntity enumEntity, Type logicType, object userData = null)
        {
            entityComponent.ShowEntity(serialId, (int)enumEntity, logicType, userData);
        }

        public static void ShowEntity<T>(this EntityComponent entityComponent, int serialId, int entityId, object userData = null)
        {
            entityComponent.ShowEntity(serialId, entityId, typeof(T), userData);
        }

        public static void ShowEntity(this EntityComponent entityComponent, int serialId, int entityId, Type logicType, object userData = null)
        {
            Data.EntityData entityData = GameEntry.Data.GetData<DataEntity>().GetEntityData(entityId);

            if (entityData == null)
            {
                Log.Error("Can not load entity id '{0}' from data table.", entityId.ToString());
                return;
            }

            if (!entityComponent.HasEntityGroup(entityData.EntityGroupData.Name))
            {
                PoolParamData poolParamData = entityData.EntityGroupData.PoolParamData;
                GameEntry.Entity.AddEntityGroup(entityData.EntityGroupData.Name, poolParamData.InstanceAutoReleaseInterval, poolParamData.InstanceCapacity, poolParamData.InstanceExpireTime, poolParamData.InstancePriority);
            }

            entityComponent.ShowEntity(serialId, logicType, AssetUtility.GetEntityAsset(entityData.Name), entityData.EntityGroupData.Name, Constant.AssetPriority.EntityAsset, userData);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
