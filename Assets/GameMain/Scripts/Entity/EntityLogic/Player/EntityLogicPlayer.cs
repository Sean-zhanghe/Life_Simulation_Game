using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using Cinemachine;
using StarForce.Data;

namespace StarForce
{
    public class EntityLogicPlayer : EntityLogicBase
    {
        protected DataPlayer dataPlayer;
        protected EntityDataPlayer entityDataPlayer;

        protected PlayerMovement playerMovement;
        protected PlayerClothes playerClothes;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            playerMovement = transform.GetComponent<PlayerMovement>();
            playerClothes = transform.GetComponent<PlayerClothes>();
            
            playerMovement.OnInit(userData);
            playerClothes.OnInit(userData);
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
            playerClothes.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            playerMovement.OnUpdate(elapseSeconds, realElapseSeconds);
            playerClothes.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            playerMovement.OnHide(isShutdown, userData);
            playerClothes.OnHide(isShutdown, userData);

            entityDataPlayer = null;
        }

        public override void Pause()
        {
            pause = true;

            playerMovement.OnPause();
        }

        public override void Resume()
        {
            pause = false;

            playerMovement.OnResume();
        }

        public override void Dead()
        {
            playerMovement.OnDead();
        }
    }
}

