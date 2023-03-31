using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.EntityLogicEnemy>;

/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : BaseState
{
    private float TIME_OUT = 5f;

    private Vector3 patrolPosition;
    private float timer;

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        logic.animator.Play(Constant.Animation.SkeletonWalk);

        patrolPosition = RandomPatrolPosition();
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (pause) return;

        logic.FlipTo(patrolPosition);

        // 收到伤害
        if (logic.IsHit)
        {
            ChangeState<HitState>(fsm);
            return;
        }

        // 发现玩家并且没有超出追击范围
        if (logic.target != null &&
            Vector2.Distance(logic.target.position, logic.origion) < logic.enemyData.ChaseRadius)
        {
            ChangeState<ChaseState>(fsm);
            return;
        }

        logic.transform.position = Vector2.MoveTowards(logic.transform.position,
            patrolPosition, enemyData.WalkSpeed * Time.deltaTime);

        if (Vector2.Distance(logic.transform.position, patrolPosition) < .1f)
        {
            ChangeState<IdleState>(fsm);
            return;
        }

        // 当超过一定时间还没有到达随机巡逻点，从新随机
        timer += elapseSeconds;
        if (timer > TIME_OUT)
        {
            patrolPosition = RandomPatrolPosition();
            timer = 0;
        }
    }

    protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        patrolPosition = Vector3.zero;
        timer = 0;
    }

    protected override void OnDestroy(ProcedureOwner fsm)
    {
        base.OnDestroy(fsm);

        patrolPosition = Vector3.zero;
        timer = 0;
    }

    private Vector3 RandomPatrolPosition()
    {
        Vector3 result = logic.origion;

        float radius = logic.enemyData.PatrolRadius;

        float randomX = Random.Range(-radius, radius);
        float randomY = Random.Range(-radius, radius);

        result.x += randomX;
        result.y += randomY;

        return result;
    }
}
