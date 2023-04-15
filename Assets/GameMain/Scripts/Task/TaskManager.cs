using GameFramework;
using StarForce.Data;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using OpenUIFormSuccessEventArgs = UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;


public class TaskManager : IReference
{
    private DataTask dataTask;
    private DataPlayer dataPlayer;
    private DataEvent dataEvent;

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

        dataTask = GameEntry.Data.GetData<DataTask>();
        dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        dataEvent = GameEntry.Data.GetData<DataEvent>();

        dataTask.LoadGameTask();
    }

    public void Clear()
    {
        GameEntry.Event.Unsubscribe(DialogFinishEventArgs.EventId, OnDialogFinish);
        GameEntry.Event.Unsubscribe(WorkFinishEventArgs.EventId, OnWorkFinish);
        GameEntry.Event.Unsubscribe(LoadSceneCompleteEventArgs.EventId, OnLoadSceneComplete);
        GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUISuccess);
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
            Debug.Log(value + " ----------  " + condValue);
            if (value == condValue)
            {
                Debug.Log("11111111111111");
                // 发放任务奖励
                dataPlayer.AddRewardByConfiger(dataTask.CurrentMainTask.Reward);
                // 修改任务状态
                dataTask.ChangeTaskState(EnumTaskType.RandomTask, EnumTaskState.Finish);
            }
        }
    }

    private void CheckEventCondition(EnumEventType condition, string parameter, string value)
    {
        int? conditionType = dataTask.CurrentMainTask?.SubTaskType;
        // 检查事件是否完成
        if (dataEvent.CurrentEvent.Count > 0)
        {
            for (int i = 0; i < dataEvent.CurrentEvent.Count; i++)
            {
                StarForce.Data.Event curEvent = dataEvent.CurrentEvent[i];
                conditionType = curEvent.EventType;
                if (conditionType == (int)condition)
                {
                    string condValue = dataEvent.GetEventConditionValue(curEvent.Condition, parameter);
                    if (condValue == string.Empty) continue;

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
