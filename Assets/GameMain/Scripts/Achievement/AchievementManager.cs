using GameFramework;
using GameFramework.Event;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : IReference
{
    private DataWork dataWork;
    private DataPlayer dataPlayer;

    public AchievementManager()
    {
        dataWork = null;
        dataPlayer = null;
    }

    public static AchievementManager Create()
    {
        AchievementManager achievementManager = ReferencePool.Acquire<AchievementManager>();
        return achievementManager;
    }

    public void Initialize()
    {
        GameEntry.Event.Subscribe(WorkFinishEventArgs.EventId, OnWorkFinish);

        dataWork = GameEntry.Data.GetData<DataWork>();
        dataPlayer = GameEntry.Data.GetData<DataPlayer>();
    }

    public void Clear()
    {
        GameEntry.Event.Unsubscribe(WorkFinishEventArgs.EventId, OnWorkFinish);

        dataWork = null;
        dataPlayer = null;
    }

    private void OnWorkFinish(object sender, GameEventArgs e)
    {
        WorkFinishEventArgs ne = (WorkFinishEventArgs)e;
        if (ne == null)
        {
            return;
        }

        int workId = ne.WorkId;
        WorkData workData = dataWork.GetWorkDataById(workId);
        if (workData == null)
        {
            return;
        }

        string consume = workData.Consume;
        dataPlayer.AddRewardByConfiger(consume);

        string reward = workData.Reward;
        dataPlayer.AddRewardByConfiger(reward);
    }
}
