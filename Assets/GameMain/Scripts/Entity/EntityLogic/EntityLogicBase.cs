using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class EntityLogicBase : EntityLogicEx, IPause
    {
        //表示是否死亡或已攻击玩家即将回收，以防重复执行回收逻辑
        private bool hide = false;
        protected bool pause = false;

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

            if (pause) return;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            hide = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

        }

        private void OnTriggerExit2D(Collider2D collision)
        {

        }

        public virtual void Pause()
        {
            pause = true;
        }

        public virtual void Resume()
        {
            pause = false;
        }

        public virtual void Damage(float value)
        {

        }

        public virtual void Dead()
        {
            
        }
    }

}
