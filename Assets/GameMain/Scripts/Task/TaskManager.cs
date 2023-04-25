using GameFramework;
using StarForce.Data;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using OpenUIFormSuccessEventArgs = UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
using ShowEntitySuccessEventArgs = UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
using HideEntityCompleteEventArgs = UnityGameFramework.Runtime.HideEntityCompleteEventArgs;

public class TaskManager : IReference
{
    private DataTask dataTask;
    private DataPlayer dataPlayer;
    private DataEvent dataEvent;
    private DataRecruit dataRecruit;
    private DataEntity dataEntity;

    public TaskManager()
    {

    }

    public static TaskManager Create()
    {
        TaskManager taskManager = ReferencePool.Acquire<TaskManager>();
        return taskManager;
    }

    public void Initialize()
    {
        GameEntry.Event.Subscribe(DialogFinishEventArgs.EventId, OnDialogFinish);
        GameEntry.Event.Subscribe(WorkFinishEventArgs.EventId, OnWorkFinish);
        GameEntry.Event.Subscribe(LoadSceneCompleteEventArgs.EventId, OnLoadSceneComplete);
        GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUISuccess);
        GameEntry.Event.Subscribe(ReleaseEventEventArgs.EventId, OnReleaseEvent);
        GameEntry.Event.Subscribe(EventFinishEventArgs.EventId, OnEventFinish);
        GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Subscribe(HideEntityCompleteEventArgs.EventId, OnHideEntityComplete);

        dataTask = GameEntry.Data.GetData<DataTask>();
        dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        dataEvent = GameEntry.Data.GetData<DataEvent>();
        dataRecruit = GameEntry.Data.GetData<DataRecruit>();
        dataEntity = GameEntry.Data.GetData<DataEntity>();

        dataTask.LoadGameTask();
    }

    public void Clear()
    {
        GameEntry.Event.Unsubscribe(DialogFinishEventArgs.EventId, OnDialogFinish);
        GameEntry.Event.Unsubscribe(WorkFinishEventArgs.EventId, OnWorkFinish);
        GameEntry.Event.Unsubscribe(LoadSceneCompleteEventArgs.EventId, OnLoadSceneComplete);
        GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUISuccess);
        GameEntry.Event.Unsubscribe(ReleaseEventEventArgs.EventId, OnReleaseEvent);
        GameEntry.Event.Unsubscribe(EventFinishEventArgs.EventId, OnEventFinish);
        GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        GameEntry.Event.Unsubscribe(HideEntityCompleteEventArgs.EventId, OnHideEntityComplete);
    }

    public void OnDialogFinish(object sender, GameEventArgs e)
    {
        DialogFinishEventArgs ne = (DialogFinishEventArgs)e;
        if (ne == null)
            return;

        int dialogId = ne.DialogId;

        CheckTaskCondition(EnumTaskCondition.Dialog, Constant.Parameter.Dialog, dialogId.ToString());
        CheckEventCondition(EnumEventType.Dialog, Constant.Parameter.Dialog, dialogId.ToString());
        CheckEventCondition(EnumEventType.Phone, Constant.Parameter.Dialog, dialogId.ToString());
    }

    private void OnWorkFinish(object sender, GameEventArgs e)
    {
        WorkFinishEventArgs ne = (WorkFinishEventArgs)e;
        if (ne == null)
        {
            return;
        }

        int workId = ne.WorkId;

        CheckTaskCondition(EnumTaskCondition.Work, Constant.Parameter.Work, workId.ToString());
        CheckEventCondition(EnumEventType.Work, Constant.Parameter.Work, workId.ToString());
    }

    private void OnLoadSceneComplete(object sender, GameEventArgs e)
    {
        LoadSceneCompleteEventArgs ne = (LoadSceneCompleteEventArgs)e;
        if (ne == null)
        {
            return;
        }

        string currentScene = ne.CurrentScene;

        CheckTaskCondition(EnumTaskCondition.Scene, Constant.Parameter.Scene, currentScene);
        CheckEventCondition(EnumEventType.Scene, Constant.Parameter.Scene, currentScene);
    }

    private void OnOpenUISuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
        if (ne == null) return;

        string UIForm = ne.UIForm.name.Replace("(Clone)", "");

        CheckTaskCondition(EnumTaskCondition.UI, Constant.Parameter.UI, UIForm);
        CheckEventCondition(EnumEventType.UI, Constant.Parameter.UI, UIForm);
    }

    private void OnReleaseEvent(object sender, GameEventArgs e)
    {
        ReleaseEventEventArgs ne = (ReleaseEventEventArgs)e;
        if (ne == null) return;

        StarForce.Data.Event m_Event = ne.m_Event;

        if (m_Event.EventType == (int)EnumEventType.UI)
        {
            string value = dataEvent.GetEventConditionValue(m_Event.Parameter, Constant.Parameter.UI);
            GameEntry.UI.OpenUIForm(int.Parse(value));
        }
    }

    private void OnEventFinish(object sender, GameEventArgs e)
    {
        EventFinishEventArgs ne = (EventFinishEventArgs)e;
        if (ne == null) return;

        StarForce.Data.Event m_Event = ne.m_Event;
        dataRecruit.CheckRecruit(Constant.Parameter.Event, ne.m_Event.Id.ToString());
    }

    private void OnShowEntitySuccess(object sender, GameEventArgs e)
    {
        ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
        if (ne == null) return;

        CheckEventCondition(EnumEventType.Entity, Constant.Parameter.Entity, ne.Entity.Id.ToString());
    }

    private void OnHideEntityComplete(object sender, GameEventArgs e)
    {
        HideEntityCompleteEventArgs ne = (HideEntityCompleteEventArgs)e;
        if (ne == null) return;

        string[] paths = ne.EntityAssetName.Split('/');
        string name = paths[paths.Length - 1].Replace(".prefab", "");
        dataRecruit.CheckRecruit(Constant.Parameter.Entity, name);
    }

    private void CheckTaskCondition(EnumTaskCondition condition, string parameter, string value)
    {
        int? conditionType = dataTask.CurrentMainTask?.SubTaskType;
        if (conditionType == (int)condition)
        {
            string condValue = dataTask.GetTaskConditionValue(dataTask.CurrentMainTask.TaskCondition, parameter);
            if (value == condValue)
            {
                // 发放任务奖励
                dataPlayer.AddRewardByConfiger(dataTask.CurrentMainTask.Reward);
                // 修改任务状态
                dataTask.ChangeTaskState(EnumTaskType.MainTask, EnumTaskState.Finish);
            }
        }

        conditionType = dataTask.CurrentRandomTask?.SubTaskType;
        if (conditionType == (int)condition)
        {
            string condValue = dataTask.GetTaskConditionValue(dataTask.CurrentRandomTask.TaskCondition, parameter);
            if (value == condValue)
            {
                // 发放任务奖励
                dataPlayer.AddRewardByConfiger(dataTask.CurrentMainTask.Reward);
                // 修改任务状态
                dataTask.ChangeTaskState(EnumTaskType.RandomTask, EnumTaskState.Finish);
            }
        }
    }

    private void CheckEventCondition(EnumEventType condition, string parameter, string value)
    {
        // 检查事件是否完成
        if (dataEvent.CurrentEvent.Count > 0)
        {
            for (int i = 0; i < dataEvent.CurrentEvent.Count; i++)
            {
                StarForce.Data.Event curEvent = dataEvent.CurrentEvent[i];
                if (curEvent.EventType == (int)condition)
                {
                    string condValue = dataEvent.GetEventConditionValue(curEvent.Condition, parameter);
                    if (condValue == string.Empty) continue;

                    if (condition == EnumEventType.Entity)
                    {
                        int entityId = int.Parse(condValue.Split('=')[0]);
                        int count = int.Parse(condValue.Split('=')[1]);
                        StarForce.Data.EntityData entityData = dataEntity.GetEntityData(entityId);
                        if (entityData.Id == entityId)
                        {
                            if (curEvent.Progress != count)
                            {
                                dataEvent.UpdateEventProgress(curEvent.Id, 1);
                            }
                            if (dataEvent.CurrentEvent[i].Progress >= count)
                            {
                                // 发放事件奖励
                                dataPlayer.AddRewardByConfiger(curEvent.Reward);
                                // 修改事件状态
                                dataEvent.ChangeEventState(curEvent.Id, EnumEventState.Finish);
                            }
                        }
                        return;
                    }

                    if (condValue == value)
                    {
                        // 发放事件奖励
                        dataPlayer.AddRewardByConfiger(curEvent.Reward);
                        // 修改事件状态
                        dataEvent.ChangeEventState(curEvent.Id, EnumEventState.Finish);
                    }
                }
            }
        }
    }
}
