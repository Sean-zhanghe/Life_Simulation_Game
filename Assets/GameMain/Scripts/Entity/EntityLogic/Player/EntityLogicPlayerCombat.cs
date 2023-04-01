using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using Cinemachine;


namespace StarForce
{
    public class EntityLogicPlayerCombat : EntityLogicPlayer
    {
        private PlayerAttack playerAttack;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            playerAttack = transform.GetComponent<PlayerAttack>();
            playerAttack.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            playerAttack.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            playerAttack.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            playerAttack.OnHide(isShutdown, userData);
        }

        public override void Pause()
        {
            base.Pause();

            playerAttack.OnPause();
        }

        public override void Resume()
        {
            base.Resume();

            playerAttack.OnResume();
        }

        public override void Damage(float value)
        {
            base.Damage(value);
            Debug.Log("player damage 1111111111111111");
            Debug.Log(value);
            dataPlayer.Damage(value);

        }

        public override void Dead()
        {
            base.Dead();

            playerAttack.OnDead();
        }
    }
}

