using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.EntityLogicEnemy>;

/// <summary>
/// 追击状态
/// </summary>
public class ChaseState : BaseState
{

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        logic.animator.Play(Constant.Animation.SkeletonWalk);
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);


        // 收到伤害
        if (logic.IsHit)
        {
            ChangeState<HitState>(fsm);
            return;
        }

        // 目标不为空 追击
        if (logic.target)
        {
            logic.FlipTo(logic.target.position);

            Vector3 pos = Vector3.zero;

            int dir = logic.target.position.x > logic.transform.position.x ? -1 : 1;
            pos = logic.target.position + new Vector3(dir * 0.7f, 0, 0);

            logic.transform.position = Vector2.MoveTowards(logic.transform.position,
                pos, logic.enemyData.ChaseSpeed * Time.deltaTime);
        }
        
        // 目标丢失或超出追击范围 Idle
        if (logic.target == null || 
            Vector2.Distance(logic.target.position, logic.origion) > logic.enemyData.ChaseRadius)
        {
            // logic.target = null;
            ChangeState<IdleState>(fsm);
            return;
        }

        if (Mathf.Abs(logic.target.position.y - logic.transform.position.y) < .1f &&
            Physics2D.OverlapCircle(logic.attackPoint.position, logic.attackArea, Constant.Layer.PlayerLayerId))
        {
            ChangeState<AttackState>(fsm);
        }
    }

    protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnDestroy(ProcedureOwner fsm)
    {
        base.OnDestroy(fsm);
    }
}
