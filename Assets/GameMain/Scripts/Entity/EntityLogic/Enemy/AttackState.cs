using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.EntityLogicEnemy>;

/// <summary>
/// 攻击
/// </summary>
public class AttackState : BaseState
{
    private float timer;
    private AnimatorStateInfo info;

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        logic.animator.Play(Constant.Animation.SkeletonAttack);
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

        info = logic.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            ChangeState<ChaseState>(fsm);
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
