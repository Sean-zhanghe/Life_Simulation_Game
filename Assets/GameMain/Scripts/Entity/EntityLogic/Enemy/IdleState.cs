using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.EntityLogicEnemy>;

/// <summary>
/// 待机
/// </summary>
public class IdleState : BaseState
{
    private float timer;
     
    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        logic.animator.Play(Constant.Animation.SkeletonIdle);
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        timer += elapseSeconds;

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

        if (timer >= enemyData.IdleTime)
        {
            ChangeState<PatrolState>(fsm);
        }
    }

    protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        timer = 0;
    }

    protected override void OnDestroy(ProcedureOwner fsm)
    {
        base.OnDestroy(fsm);
    }
}
