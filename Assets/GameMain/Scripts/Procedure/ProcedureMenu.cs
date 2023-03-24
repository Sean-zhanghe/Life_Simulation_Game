using UnityEngine;
using UnityGameFramework.Runtime;
using StarForce;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Event;

public class ProcedureMenu : ProcedureBase
{
    public override bool UseNativeDialog
    {
        get
        {
            return false;
        }
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        StarForce.GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        StarForce.GameEntry.UI.OpenUIForm(UIFormId.UIMenuForm, this);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        StarForce.GameEntry.Event.Unsubscribe(OpenUIFormDependencyAssetEventArgs.EventId, OnOpenUIFormSuccess);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
    }

    private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }
        Debug.Log("open ui menu 1111111111");
    }
}
