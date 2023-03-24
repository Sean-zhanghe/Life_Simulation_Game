using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;
using GameFramework;

namespace StarForce
{
    public abstract class EntityLogicEx : EntityLogicWithData
    {
        private EventSubscriber eventSubscriber;
        private EntityLoader entityLoader;

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            UnSubscribeAll();
            if (eventSubscriber != null)
            {
                ReferencePool.Release(eventSubscriber);
                eventSubscriber = null;
            }

            HideAllEntity();
            if (entityLoader != null)
            {
                ReferencePool.Release(entityLoader);
                entityLoader = null;
            }
        }

        protected void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber == null)
                eventSubscriber = EventSubscriber.Create(this);

            eventSubscriber.Subscribe(id, handler);
        }

        protected void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber != null)
                eventSubscriber.UnSubscribe(id, handler);
        }

        protected void UnSubscribeAll()
        {
            if (eventSubscriber != null)
                eventSubscriber.UnSubscribeAll();
        }

        protected int ShowEntity(EnumEntity enumEntity, Type entityLogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity(enumEntity, entityLogicType, onShowSuccess, userData);
        }

        protected int ShowEntity(int entityId, Type entityLogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity(entityId, entityLogicType, onShowSuccess, userData);
        }

        protected int ShowEntity<T>(EnumEntity enumEntity, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity<T>(enumEntity, onShowSuccess, userData);
        }

        protected int ShowEntity<T>(int entityId, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            if (entityLoader == null)
            {
                entityLoader = EntityLoader.Create(this);
            }

            return entityLoader.ShowEntity<T>(entityId, onShowSuccess, userData);
        }

        protected bool HasEntity(int serialId)
        {
            if (entityLoader == null)
                return false;

            return entityLoader.GetEntity(serialId);
        }

        protected Entity GetEntity(int serialId)
        {
            if (entityLoader == null)
                return null;

            return entityLoader.GetEntity(serialId);
        }

        protected void HideEntity(int serialId)
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideEntity(serialId);
        }

        protected void HideEntity(Entity entity)
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideEntity(entity);
        }

        protected void HideAllEntity()
        {
            if (entityLoader == null)
            {
                return;
            }

            entityLoader.HideAllEntity();
        }
    }
}
