using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class EntityLogicHPBar : EntityLogicEx, IPause
    {

        private EntityDataFollow entityDataFollow;
        public Slider slider;

        private bool isEnter = false;
        protected bool pause = false;

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
                Log.Error("EntityHPBar param invaild");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (pause) return;


            if (entityDataFollow != null && entityDataFollow.Follow != null)
            {
                transform.position = entityDataFollow.Follow.position + entityDataFollow.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            entityDataFollow = null;
            UpdateHealth(1);
            SetVisible(false);
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }


        public void UpdateHealth(float normalizedHealth)
        {
            slider.value = normalizedHealth;

            SetVisible(normalizedHealth < 1.0f);
        }


        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
