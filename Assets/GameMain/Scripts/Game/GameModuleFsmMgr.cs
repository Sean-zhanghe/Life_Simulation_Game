using GameFramework;
using GameFramework.Fsm;
using StarForce;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModuleFsmMgr : IReference
{
    private GameManager gameManager;

    private static int SERIAL_ID = 0;

    private IFsm<GameManager> fsm;

    public GameModuleFsmMgr()
    {
        this.gameManager = null;
    }

    public static GameModuleFsmMgr Create(GameManager gameManager)
    {
        GameModuleFsmMgr gameModuleFsmMgr = ReferencePool.Acquire<GameModuleFsmMgr>();
        gameModuleFsmMgr.gameManager = gameManager;
        return gameModuleFsmMgr;
    }

    public void Initialize()
    {
        List<FsmState<GameManager>> moduleList = new List<FsmState<GameManager>>() { 
            new RealityModule(),
            new GameModule()
        };

        fsm = GameEntry.Fsm.CreateFsm(SERIAL_ID++.ToString(), this.gameManager, moduleList);
        fsm.Start<RealityModule>();
    }

    public void Update()
    {

    }

    public void Clear()
    {
        GameEntry.Fsm.DestroyFsm(fsm);
    }
}
