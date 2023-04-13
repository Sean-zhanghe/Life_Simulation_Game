using GameFramework;
using StarForce.Data;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;

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
    }

    public void OnDialogFinish(object sender, GameEventArgs e)
    {
        DialogFinishEventArgs ne = (DialogFinishEventArgs)e;
        if (ne == null)
            return;

        int dialogId = ne.DialogId;

        // 检查主线任务是否完成
        int? conditionType = dataTask.CurrentMainTask?.SubTaskType;
        if (conditionType == (int)EnumTaskCondition.Dialog)
        {
            string value = dataTask.GetTaskConditionValue(dataTask.CurrentMainTask.TaskCondition, Constant.Parameter.Dialog);
            int condDialogId = 0;
            if (value != string.Empty)
                condDialogId = int.Parse(value);
            if (condDialogId == dialogId)
            {
                // 发放主线任务奖励
                dataPlayer.AddRewardByConfiger(dataTask.CurrentMainTask.Reward);
                // 修改主线任务状态
                dataTask.ChangeTaskState(EnumTaskType.MainTask, EnumTaskState.Finish);
            }
        }

        // 检查随机任务是否完成
        conditionType = dataTask.CurrentRandomTask?.SubTaskType;
        if (conditionType == (int)EnumTaskCondition.Dialog)
        {
            string value = dataTask.GetTaskConditionValue(dataTask.CurrentRandomTask.TaskCondition, Constant.Parameter.Dialog);
            int condDialogId = 0;
            if (value != string.Empty)
                condDialogId = int.Parse(value);
            if (condDialogId == dialogId)
            {
                // 发放随机任务奖励
                dataPlayer.AddRewardByConfiger(dataTask.CurrentRandomTask.Reward);
                // 修改随机任务状态
                dataTask.ChangeTaskState(EnumTaskType.RandomTask, EnumTaskState.Finish);
            }
        }

        // 检查事件是否完成
        if (dataEvent.CurrentEvent.Count > 0)
        {
            for (int i = 0; i < dataEvent.CurrentEvent.Count; i++)
            {
                StarForce.Data.Event curEvent = dataEvent.CurrentEvent[i];
                conditionType = curEvent.EventType;
                if (conditionType == (int)EnumEventType.Dialog || conditionType == (int)EnumEventType.Phone)
                {
                    string value = dataEvent.GetEventConditionValue(curEvent.Condition, Constant.Parameter.Dialog);
                    if (value == string.Empty) continue;
                    int condDialogId = int.Parse(value);
                    if (condDialogId == dialogId)
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

    private void OnWorkFinish(object sender, GameEventArgs e)
    {
        WorkFinishEventArgs ne = (WorkFinishEventArgs)e;
        if (ne == null)
        {
            return;
        }

        int workId = ne.WorkId;

        int? conditionType = dataTask.CurrentMainTask?.SubTaskType;
        if (conditionType == (int)EnumTaskCondition.Work)
        {
            string value = dataTask.GetTaskConditionValue(dataTask.CurrentMainTask.TaskCondition, Constant.Parameter.Work);
            int condWorkId = 0;
            if (value != string.Empty)
                condWorkId = int.Parse(value);
            if (condWorkId == workId)
            {
                dataPlayer.AddRewardByConfiger(dataTask.CurrentMainTask.Reward);
                dataTask.ChangeTaskState(EnumTaskType.MainTask, EnumTaskState.Finish);
            }
        }

        conditionType = dataTask.CurrentRandomTask?.SubTaskType;
        if (conditionType == (int)EnumTaskCondition.Work)
        {
            string value = dataTask.GetTaskConditionValue(dataTask.CurrentRandomTask.TaskCondition, Constant.Parameter.Work);
            int condWorkId = 0;
            if (value != string.Empty)
                condWorkId = int.Parse(value);
            if (condWorkId == workId)
            {
                dataPlayer.AddRewardByConfiger(dataTask.CurrentRandomTask.Reward);
                dataTask.ChangeTaskState(EnumTaskType.RandomTask, EnumTaskState.Finish);
            }
        }

        // 检查事件是否完成
        if (dataEvent.CurrentEvent.Count > 0)
        {
            for (int i = 0; i < dataEvent.CurrentEvent.Count; i++)
            {
                StarForce.Data.Event curEvent = dataEvent.CurrentEvent[i];
                conditionType = curEvent.EventType;
                if (conditionType == (int)EnumEventType.Work)
                {
                    string condWorkId = dataEvent.GetEventConditionValue(curEvent.Condition, Constant.Parameter.Work);
                    if (condWorkId == string.Empty) continue;
                    int id = int.Parse(condWorkId);
                    if (id == workId)
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

    private void OnLoadSceneComplete(object sender, GameEventArgs e)
    {
        LoadSceneCompleteEventArgs ne = (LoadSceneCompleteEventArgs)e;
        if (ne == null)
        {
            return;
        }

        string currentScene = ne.CurrentScene;

        int? conditionType = dataTask.CurrentMainTask?.SubTaskType;
        if (conditionType == (int)EnumTaskCondition.Scene)
        {
            string condScene = dataTask.GetTaskConditionValue(dataTask.CurrentMainTask.TaskCondition, Constant.Parameter.Scene);
            if (condScene == currentScene)
            {
                // 发放任务奖励
                dataPlayer.AddRewardByConfiger(dataTask.CurrentMainTask.Reward);
                // 修改任务状态
                dataTask.ChangeTaskState(EnumTaskType.MainTask, EnumTaskState.Finish);
            }
        }

        conditionType = dataTask.CurrentRandomTask?.SubTaskType;
        if (conditionType == (int)EnumTaskCondition.Scene)
        {
            string condScene = dataTask.GetTaskConditionValue(dataTask.CurrentRandomTask.TaskCondition, Constant.Parameter.Scene);
            if (condScene == currentScene)
            {
                // 发放任务奖励
                dataPlayer.AddRewardByConfiger(dataTask.CurrentMainTask.Reward);
                // 修改任务状态
                dataTask.ChangeTaskState(EnumTaskType.RandomTask, EnumTaskState.Finish);
            }
        }

        // 检查事件是否完成
        if (dataEvent.CurrentEvent.Count > 0)
        {
            for (int i = 0; i < dataEvent.CurrentEvent.Count; i++)
            {
                StarForce.Data.Event curEvent = dataEvent.CurrentEvent[i];
                conditionType = curEvent.EventType;
                if (conditionType == (int)EnumEventType.Scene)
                {
                    string condScene = dataEvent.GetEventConditionValue(curEvent.Condition, Constant.Parameter.Scene);
                    if (condScene == string.Empty) continue;

                    if (condScene == currentScene)
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
