using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.GameManager>;

/// <summary>
/// 现实模式
/// </summary>
public class RealityModule : FsmState<GameManager>
{
    private DataGame dataGame;

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);

        dataGame = GameEntry.Data.GetData<DataGame>();
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        GameEntry.UI.OpenUIForm(UIFormId.UIMainForm, this);
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

    }

    protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        GameEntry.UI.CloseAllLoadedUIForms();
    }

    protected override void OnDestroy(ProcedureOwner fsm)
    {
        base.OnDestroy(fsm);
    }
}
