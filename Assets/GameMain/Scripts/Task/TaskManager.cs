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

        dataTask = GameEntry.Data.GetData<DataTask>();
        dataPlayer = GameEntry.Data.GetData<DataPlayer>();

        dataTask.LoadGameTask();
    }

    public void Clear()
    {
        GameEntry.Event.Unsubscribe(DialogFinishEventArgs.EventId, OnDialogFinish);
        GameEntry.Event.Unsubscribe(WorkFinishEventArgs.EventId, OnWorkFinish);
    }

    private string GetTaskConditionByType(EnumTaskType taskType)
    {
        string condition = "";
        switch (taskType)
        {
            case EnumTaskType.MainTask:
                if (dataTask.CurrentMainTask == null) return string.Empty;
                condition = dataTask.CurrentMainTask.TaskCondition;
                break;
            case EnumTaskType.RandomTask:
                if (dataTask.CurrentRandomTask == null) return string.Empty;
                condition = dataTask.CurrentRandomTask.TaskCondition;
                break;
            default:
                break;
        }
        return condition;
    }

    private int GetTaskConditionValue(EnumTaskType taskType, string key)
    {
        string condition = GetTaskConditionByType(taskType);
        if (condition == string.Empty) return 0;

        string[] conditions = condition.Split('&');
        foreach (var cond in conditions)
        {
            if (cond.StartsWith(key + "="))
            {
                return int.Parse(cond.Substring(key.Length + 1)); 
            }
        }
        return 0;
    }

    public void OnDialogFinish(object sender, GameEventArgs e)
    {
        DialogFinishEventArgs ne = (DialogFinishEventArgs)e;
        if (ne == null)
            return;

        int dialogId = ne.DialogId;

        // 检查主线任务是否完成
        int conditionType = GetTaskConditionValue(EnumTaskType.MainTask, Constant.Parameter.Type);
        if (conditionType == (int)EnumTaskCondition.Dialog)
        {
            int condDialogId = GetTaskConditionValue(EnumTaskType.MainTask, Constant.Parameter.Dialog);
            if (condDialogId == dialogId)
            {
                // 修改主线任务状态
                dataTask.ChangeTaskState(EnumTaskType.MainTask, EnumTaskState.Finish);
                // 发放主线任务奖励
                dataPlayer.AddPriorityByConfiger(dataTask.CurrentMainTask.Reward);
            }
        }

        // 检查随机任务是否完成
        conditionType = GetTaskConditionValue(EnumTaskType.RandomTask, Constant.Parameter.Type);
        if (conditionType == (int)EnumTaskCondition.Dialog)
        {
            int condDialogId = GetTaskConditionValue(EnumTaskType.RandomTask, Constant.Parameter.Dialog);
            if (condDialogId == dialogId)
            {
                // 修改随机任务状态
                dataTask.ChangeTaskState(EnumTaskType.RandomTask, EnumTaskState.Finish);
                // 发放随机任务奖励
                dataPlayer.AddPriorityByConfiger(dataTask.CurrentRandomTask.Reward);
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

        int conditionType = GetTaskConditionValue(EnumTaskType.MainTask, Constant.Parameter.Type);
        if (conditionType == (int)EnumTaskCondition.Work)
        {
            int condWorkId = GetTaskConditionValue(EnumTaskType.MainTask, Constant.Parameter.Work);
            if (condWorkId == workId)
            {
                dataTask.ChangeTaskState(EnumTaskType.MainTask, EnumTaskState.Finish);
                dataPlayer.AddPriorityByConfiger(dataTask.CurrentMainTask.Reward);
            }
        }

        conditionType = GetTaskConditionValue(EnumTaskType.RandomTask, Constant.Parameter.Type);
        if (conditionType == (int)EnumTaskCondition.Work)
        {
            int condWorkId = GetTaskConditionValue(EnumTaskType.RandomTask, Constant.Parameter.Work);
            if (condWorkId == workId)
            {
                dataTask.ChangeTaskState(EnumTaskType.RandomTask, EnumTaskState.Finish);
                dataPlayer.AddPriorityByConfiger(dataTask.CurrentRandomTask.Reward);
            }
        }
    }
}
