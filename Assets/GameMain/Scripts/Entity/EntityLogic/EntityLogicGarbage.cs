using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class EntityLogicGarbage : EntityLogicEx, IPause
    {
        private GameObject Tips;
        private bool isEnter = false;
        protected bool pause = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            Tips = transform.GetChild(0).gameObject;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (pause) return;

            if (isEnter)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GameEntry.Event.Fire(HideEntityInGameEventArgs.EventId, HideEntityInGameEventArgs.Create(Id));
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                isEnter = true;
                Tips.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                isEnter = false;
                Tips.SetActive(false);
            }
        }
    }
}
