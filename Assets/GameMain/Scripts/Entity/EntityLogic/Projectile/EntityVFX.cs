using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class EntityVFX : EntityLogicEx, IPause
    {

        protected bool pause = false;

        protected EntityDataFollow entityDataFollow;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataFollow = userData as EntityDataFollow;
            if (entityDataFollow == null)
            {
                return;
            }

            GameEntry.Sound.PlaySound(entityDataFollow.ShowSound, Entity);

            transform.localScale = entityDataFollow.Scale;
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (entityDataFollow != null && entityDataFollow.Follow != null)
            {
                transform.position = entityDataFollow.Follow.position + entityDataFollow.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDataFollow = null;

            transform.localScale = Vector3.one;
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }
    }
}
