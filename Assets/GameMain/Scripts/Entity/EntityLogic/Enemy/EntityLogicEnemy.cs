using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class EntityLogicEnemy : EntityLogicBase
    {
        protected IFsm<EntityLogicEnemy> fsm;
        private int SERIAL_ID = 0;

        protected EntityDataEnemy entityDataEnemy;
        public EnemyData enemyData;

        public Animator animator;
        public Vector3 origion;
        public Transform target;
        public Transform hpBarRoot;

        public Transform reactPoint;
        public Transform attackPoint;
        public float attackArea = 0.75f;

        public float HP { get; private set; }

        public bool IsDead { get { return HP <= 0; } }

        public bool IsHit { get; set; }

        private bool loadedHPBar = false;
        private EntityLogicHPBar entityHPBar;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            entityDataEnemy = (EntityDataEnemy)userData;
            if (entityDataEnemy == null)
            {
                return;
            }

            enemyData = entityDataEnemy.enemyData;
            SERIAL_ID = entityDataEnemy.FSM_SERIAL_ID;
            // 记录起始位置
            origion = entityDataEnemy.Position;

            animator = GetComponent<Animator>();
            reactPoint = transform.GetChild(0);
            attackPoint = transform.GetChild(1);
            hpBarRoot = transform.GetChild(2);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            InitFsm();

            HP = enemyData.MaxHP;
        }

        protected virtual void InitFsm()
        {
            List<FsmState<EntityLogicEnemy>> stateList = new List<FsmState<EntityLogicEnemy>>() {
                new IdleState(),
                new AttackState(),
                new HitState(),
                new PatrolState(),
                new ChaseState(),
                new DeadState(),
            };

            fsm = GameEntry.Fsm.CreateFsm(SERIAL_ID++.ToString(), this, stateList);
            fsm.Start<IdleState>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause) return;

            FindTarget();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            origion = Vector3.zero;

            HideHpBar();
        }

        private void FindTarget()
        {
            RaycastHit2D hit = Physics2D.Raycast(reactPoint.position, transform.localScale.x * new Vector3(0.25f, 0, 0)
                , 5, Constant.Layer.PlayerLayerId);
            if (hit)
            {
                if (hit.transform.CompareTag(Constant.Tag.Player))
                    target = hit.transform;
            }
            else
            {
                Debug.DrawRay(reactPoint.position, transform.localScale.x * new Vector3(5f, 0, 0), Color.red);
            }
        }

        public override void Pause()
        {
            base.Pause();
        }

        public override void Resume()
        {
            base.Resume();
        }

        public override void Damage(float value)
        {
            base.Damage(value);

            if (IsDead) return;

            if (!loadedHPBar)
            {
                GameEntry.Event.Fire(this, ShowEntityInGameEventArgs.Create(
                    (int)EnumEntity.HPBar,
                    typeof(EntityLogicHPBar),
                    OnLoadHpBarSuccess,
                    EntityDataFollow.Create(hpBarRoot)
                    ));
                loadedHPBar = true;
            }

            IsHit = true;

            HP -= value;

            if (entityHPBar)
            {
                entityHPBar.UpdateHealth(HP / enemyData.MaxHP);
            }
        }

        public override void Dead()
        {
            base.Dead();

            GameEntry.Event.Fire(this, HideEntityInGameEventArgs.Create(Entity.Id));
        }

        public void FlipTo(Vector3 target)
        {
            if (target == null) return;

            if (transform.position.x > target.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private void OnLoadHpBarSuccess(Entity entity)
        {
            entityHPBar = entity.Logic as EntityLogicHPBar;
            if (IsDead || !Available)
            {
                HideHpBar();
            }
            else
            {
                entityHPBar.UpdateHealth(HP / enemyData.MaxHP);
            }
        }
        private void HideHpBar()
        {
            if (entityHPBar)
            {
                GameEntry.Event.Fire(this, HideEntityInGameEventArgs.Create(entityHPBar.Id));
                loadedHPBar = false;
                entityHPBar = null;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackArea);
        }
    }

}
