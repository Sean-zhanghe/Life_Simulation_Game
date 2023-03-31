using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.EntityLogicEnemy>;

/// <summary>
/// 死亡状态
/// </summary>
public class DeadState : BaseState
{
    private AnimatorStateInfo info;

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        logic.animator.Play(Constant.Animation.SkeletonDead);
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        // 死亡销毁
        info = logic.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= .95f)
        {
            logic.Dead();
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
