using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Event;
using StarForce;

public class ProcedureBackGround : ProcedureBase
{
    public override bool UseNativeDialog
    {
        get { return false; }
    }

    private ProcedureOwner procedureOwner;
    private bool changeScene = false;


    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        this.procedureOwner = procedureOwner;
        this.changeScene = false;

        StarForce.GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);

        StarForce.GameEntry.UI.OpenUIForm(UIFormId.UIBackGroundForm, this);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        StarForce.GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (changeScene)
        {
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }

    private void OnChangeScene(object sender, GameEventArgs e)
    {
        ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
        if (ne == null)
            return;

        changeScene = true;
        procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
    }
}
