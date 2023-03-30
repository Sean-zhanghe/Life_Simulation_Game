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

        public Transform attackPoint;
        public float attackArea = 0.75f;

        public float HP { get; private set; }

        public bool IsDead { get { return HP <= 0; } }

        public bool IsHit { get; set; }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            entityDataEnemy = (EntityDataEnemy)userData;
            if (entityDataEnemy == null)
            {
                return;
            }

            enemyData = entityDataEnemy.enemyData;
            // 记录起始位置
            origion = entityDataEnemy.Position;

            animator = GetComponent<Animator>();
            attackPoint = transform.GetChild(1);
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
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            origion = Vector3.zero;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("1111111111");
            if (collision.CompareTag(Constant.Tag.Player))
            {
                Debug.Log("player enter 11111111111111");
                target = collision.transform;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                Debug.Log("player exit 11111111111111");
                target = null;
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

            IsHit = true;

            Debug.Log("damage 111111111111111");
            Debug.Log(value);
            HP -= value;
            Debug.Log(HP);
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

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackArea);
        }
    }

}
