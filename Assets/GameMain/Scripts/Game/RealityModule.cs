using GameFramework.Event;
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
public class RealityModule : BaseModule
{

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        GameEntry.Event.Subscribe(LoadSceneCompleteEventArgs.EventId, OnLoadSceneComplete);

        GameEntry.UI.OpenUIForm(UIFormId.UIMainForm, this);
        GameEntry.UI.OpenUIForm(UIFormId.UIPopupForm, this);

        gameManager.sceneControl.CreatePlayer<EntityLogicPlayer>();
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
    }

    protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        GameEntry.Event.Unsubscribe(LoadSceneCompleteEventArgs.EventId, OnLoadSceneComplete);
    }

    protected override void OnDestroy(ProcedureOwner fsm)
    {
        base.OnDestroy(fsm);
    }

    private void OnLoadSceneComplete(object sender, GameEventArgs e)
    {
        LoadSceneCompleteEventArgs ne = (LoadSceneCompleteEventArgs)e;
        if (ne == null)
        {
            return;
        }

        string currentScene = ne.CurrentScene;

        if (currentScene == Constant.Scene.LevelMenu)
        {
            ChangeState<GameMenuModule>(procedureOwner);
            return;
        }

        GameEntry.UI.OpenUIForm(UIFormId.UIMainForm, this);
        GameEntry.UI.OpenUIForm(UIFormId.UIPopupForm, this);
        gameManager.sceneControl.CreatePlayer<EntityLogicPlayer>();
    }
}
