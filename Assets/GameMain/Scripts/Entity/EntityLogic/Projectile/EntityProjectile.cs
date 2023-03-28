using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{

    public class EntityProjectile : EntityLogicEx, IPause
    {
        private int collisionVFXEntityId;

        protected EntityDataProjectile entityDataProjectile;

        protected bool pause;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataProjectile = userData as EntityDataProjectile;

            if (entityDataProjectile == null)
            {
                Log.Error("Entity EntityProjectile '{0}' entity data invaild.", Id);
                return;
            }

            collisionVFXEntityId = entityDataProjectile.projectileData.VFXEntityId;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDataProjectile = null;
        }

        protected void SpawnCollisionParticles()
        {
            if (collisionVFXEntityId <= 0)
            {
                return;
            }

            GameEntry.Event.Fire(this, ShowEntityInGameEventArgs.Create(
                collisionVFXEntityId,
                typeof(EntityVFXAutoHide),
                null,
                EntityData.Create(transform.position, transform.rotation)));
        }

        public virtual void Pause()
        {
            pause = true;
        }

        public virtual void Resume()
        {
            pause = false;
        }
    }
}
