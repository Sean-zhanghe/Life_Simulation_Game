using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class EntityHideSelfProjectile : EntityProjectile
    {
        public float time;

        private float timer;
        public float selfDestroyTime = 10;

        protected bool hide = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            timer += elapseSeconds;

            if (timer > selfDestroyTime)
            {
                if (!hide)
                {
                    GameEntry.Event.Fire(this, HideEntityInGameEventArgs.Create(Entity.Id));
                    hide = true;
                }
            }

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
            hide = false;
        }
    }
}
