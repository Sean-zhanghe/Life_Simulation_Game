using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public abstract class DataBase : GameFramework.Data.Data
    {
        private Dictionary<string, bool> loadedFlag = new Dictionary<string, bool>();
        private EventSubscriber eventSubscriber;

        public bool IsPreloadReady
        {
            get
            {
                foreach (var item in loadedFlag)
                {
                    if (!item.Value)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public sealed override void Init()
        {
            OnInit();
        }

        public sealed override void Preload()
        {
            OnPreload();
        }

        public sealed override void Load()
        {
            OnLoad();
        }

        public sealed override void Unload()
        {
            if (eventSubscriber != null)
            {
                eventSubscriber.UnSubscribeAll();
                ReferencePool.Release(eventSubscriber);
                eventSubscriber = null;
            }

            OnUnload();
        }

        public sealed override void Shutdown()
        {
            OnShutdown();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnPreload()
        {
        }

        protected virtual void OnLoad()
        {
        }

        protected virtual void OnUnload()
        {
        }

        protected virtual void OnShutdown()
        {
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

    }

}


