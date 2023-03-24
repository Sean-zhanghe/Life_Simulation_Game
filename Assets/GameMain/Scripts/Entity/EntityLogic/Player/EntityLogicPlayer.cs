using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using Cinemachine;


namespace StarForce
{
    public class EntityLogicPlayer : EntityLogicEx, IPause
    {
        protected EntityDataPlayer entityDataPlayer;

        private PlayerMovement playerMovement;

        protected bool pause = false;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            playerMovement = transform.GetComponent<PlayerMovement>();
            
            playerMovement.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataPlayer = userData as EntityDataPlayer;
            if (entityDataPlayer == null)
            {
                Log.Error("Entity tower '{0}' tower data invaild.", Id);
                return;
            }

            playerMovement.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            playerMovement.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            playerMovement.OnHide(isShutdown, userData);

            entityDataPlayer = null;
        }

        public void Pause()
        {
            pause = true;

            playerMovement.OnPause();
        }

        public void Resume()
        {
            pause = false;

            playerMovement.OnResume();
        }

        protected void Dead()
        {
            playerMovement.OnDead();
        }
    }
}

