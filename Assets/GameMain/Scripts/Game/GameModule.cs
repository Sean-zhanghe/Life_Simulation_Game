﻿using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<StarForce.GameManager>;

/// <summary>
/// 游戏模式
/// </summary>
public class GameModule : BaseModule
{
    private DataLevel dataLevel;

    protected override void OnInit(ProcedureOwner fsm)
    {
        base.OnInit(fsm);

        dataLevel = GameEntry.Data.GetData<DataLevel>();
    }

    protected override void OnEnter(ProcedureOwner fsm)
    {
        base.OnEnter(fsm);

        GameEntry.Event.Subscribe(LoadSceneCompleteEventArgs.EventId, OnLoadSceneComplete);
        GameEntry.Event.Subscribe(GameoverEventArgs.EventId, OnGameover);

        GameEntry.UI.OpenUIForm(UIFormId.UILevelForm, this);
        GameEntry.UI.OpenUIForm(UIFormId.UIPopupForm, this);

        gameManager.sceneControl.CreatePlayer<EntityLogicPlayerCombat>();
    }

    protected override void OnUpdate(ProcedureOwner fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
    }

    protected override void OnLeave(ProcedureOwner fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        GameEntry.Event.Unsubscribe(LoadSceneCompleteEventArgs.EventId, OnLoadSceneComplete);
        GameEntry.Event.Unsubscribe(GameoverEventArgs.EventId, OnGameover);
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

        gameManager.sceneControl.CreatePlayer<EntityLogicPlayerCombat>();
        GameEntry.UI.OpenUIForm(UIFormId.UILevelForm, this);
        GameEntry.UI.OpenUIForm(UIFormId.UIPopupForm, this);
    }

    private void OnGameover(object sender, GameEventArgs e)
    {
        GameoverEventArgs ne = (GameoverEventArgs)e;
        if (ne == null)
        {
            return;
        }

        LevelData levelData = dataLevel.GetLevelDataById(dataLevel.CurLevelId);
        GameEntry.UI.OpenUIForm(UIFormId.UILevelOverForm, UIGameOverFormOpenParam.Create(levelData, ne.EnumGameOverType));
    }
}
