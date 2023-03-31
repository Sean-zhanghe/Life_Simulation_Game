using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.EntityLogicEnemy>;

/// <summary>
/// 基础状态
/// </summary>
public class BaseState : FsmState<EntityLogicEnemy>
{
    protected ProcedureOwner procedureOwner;
    protected bool pause = false;

    protected EntityLogicEnemy logic;
    protected EnemyData enemyData;

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);

        procedureOwner = fsm;

        logic = fsm.Owner;
        enemyData = logic.enemyData;
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
    }

    protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        pause = false;
    }

    protected override void OnDestroy(ProcedureOwner fsm)
    {
        base.OnDestroy(fsm);
    }
    public virtual void Pause()
    {
        pause = true;

        logic.animator.speed = 0;
    }

    public virtual void Resume()
    {
        pause = false;

        logic.animator.speed = 1;
    }
}
