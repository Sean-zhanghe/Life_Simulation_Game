using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using StarForce;
using GameFramework;
using GameFramework.Event;

public class ProcedureGame : ProcedureBase
{
    public override bool UseNativeDialog
    {
        get { return false; }
    }

    private ProcedureOwner procedureOwner;
    private bool changeScene = false;

    private GameManager gameManager;

    protected override void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        gameManager = GameManager.Create();

        StarForce.GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
        StarForce.GameEntry.Event.Subscribe(GameStateChangeEventArgs.EventId, OnGameStateChange);
        StarForce.GameEntry.Event.Subscribe(OpenDialogEventArgs.EventId, OnOpenDialog);

        this.procedureOwner = procedureOwner;
        this.changeScene = false;

        gameManager.OnEnter();
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        if (changeScene)
        {
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        if (gameManager != null)
            gameManager.Update(elapseSeconds, realElapseSeconds);
    }

    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        StarForce.GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
        StarForce.GameEntry.Event.Unsubscribe(GameStateChangeEventArgs.EventId, OnGameStateChange);
        StarForce.GameEntry.Event.Unsubscribe(OpenDialogEventArgs.EventId, OnOpenDialog);

        gameManager.Quick();

        ReferencePool.Release(gameManager);
        gameManager = null;
    }

    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    private void OnChangeScene(object sender, GameEventArgs e)
    {
        ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
        if (ne == null)
            return;

        changeScene = true;
        procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
    }

    private void OnGameStateChange(object sender, GameEventArgs e)
    {
        GameStateChangeEventArgs ne = (GameStateChangeEventArgs)e;
        if (ne == null)
            return;

        if (ne.CurrentState == EnumGameState.Pause)
        {
            gameManager.Pause();
        }
        else if (ne.LastState == EnumGameState.Pause)
        {
            gameManager.Resume();
        }
    }

    private void OnOpenDialog(object sender, GameEventArgs e)
    {
        OpenDialogEventArgs ne = (OpenDialogEventArgs)e;
        if (ne == null)
            return;

        StarForce.GameEntry.UI.OpenUIForm(UIFormId.UIDialogForm, ne);
    }
}
