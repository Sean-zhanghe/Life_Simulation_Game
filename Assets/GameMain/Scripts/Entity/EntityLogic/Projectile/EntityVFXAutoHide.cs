using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace StarForce
{
    public class EntityVFXAutoHide : EntityVFX
    {
        private Animator animator;
        private AnimatorStateInfo info;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            animator = GetComponent<Animator>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1)
            {
                GameEntry.Event.Fire(this, HideEntityInGameEventArgs.Create(Entity.Id));
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            animator = null;
        }
    }
}
