using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class EntityProjectileArrow : EntityHideSelfProjectile
    {
        public float speed;

        private Rigidbody2D m_Rigidbody;
        private Vector3 tempVelocity;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Rigidbody.velocity = entityDataProjectile.direction * entityDataProjectile.projectileData.Speed;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpawnCollisionParticles();

            if (collision.CompareTag(Constant.Tag.Enemy))
            {
                EntityLogicEnemy enemy = collision.GetComponent<EntityLogicEnemy>();
                if (enemy != null)
                {
                    if (!enemy.IsDead)
                        enemy.Damage(entityDataProjectile.projectileData.Damage);
                }
            }

            if (!hide)
            {
                GameEntry.Event.Fire(this, HideEntityInGameEventArgs.Create(Entity.Id));
                hide = true;
            }
        }
        public override void Pause()
        {
            base.Pause();
            tempVelocity = m_Rigidbody.velocity;
            m_Rigidbody.velocity = Vector3.zero;
        }

        public override void Resume()
        {
            base.Resume();
            m_Rigidbody.velocity = tempVelocity;
        }
    }
}
