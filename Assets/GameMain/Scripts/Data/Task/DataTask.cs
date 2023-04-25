using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce.Data
{
    public class DataTask : DataBase
    {
        private IDataTable<DRTask> dtTask;
        private Dictionary<int, TaskData> dicTaskData = new Dictionary<int, TaskData>();

        private Dictionary<int, Task> dicMainTask = new Dictionary<int, Task>();
        private Dictionary<int, Task> dicRandomTask = new Dictionary<int, Task>();

        public Task CurrentMainTask { get; private set; }

        public Task CurrentRandomTask { get; private set; }

        protected override void OnInit()
        {
            CurrentMainTask = null;
            CurrentRandomTask = null;
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtTask = GameEntry.DataTable.GetDataTable<DRTask>();
            if (dtTask == null)
                throw new System.Exception("Can not get data table Task");

            DRTask[] dRTasks = dtTask.GetAllDataRows();
            foreach (var dRTask in dRTasks)
            {
                TaskData taskData = new TaskData(dRTask);
                dicTaskData.Add(taskData.Id, taskData);

                Task task = Task.Create(taskData);

                // TODO 读取数据库任务数据

                if (taskData.TaskType == (int)EnumTaskType.MainTask)
                {
                    dicMainTask.Add(task.Id, task);
                }

                if (taskData.TaskType == (int)EnumTaskType.RandomTask)
                {
                    dicRandomTask.Add(task.Id, task);
                }
            }

            Subscribe(EventFinishEventArgs.EventId, OnEventFinish);
            Subscribe(ChangeRecruitStateEventArgs.EventId, OnRecruitStateChange);
        }

        public TaskData GetTaskData(int id)
        {
            if (dicTaskData.ContainsKey(id))
            {
                return dicTaskData[id];
            }
            return null;
        }

        public TaskData[] GetTaskDatasByType(EnumTaskType taskType)
        {
            int index = 0;
            TaskData[] result = new TaskData[] { };
            foreach (var taskData in dicTaskData.Values)
            {
                if (taskData.TaskType == (int)taskType)
                {
                    result[index++] = taskData;
                }
            }
            return result;
        }

        public Task GetMainTask(int Id)
        {
            if (dicMainTask.ContainsKey(Id))
            {
                return dicMainTask[Id];
            }
            return null;
        }

        public Task GetRandomTask(int Id)
        {
            if (dicRandomTask.ContainsKey(Id))
            {
                return dicRandomTask[Id];
            }
            return null;
        }

        public Task GetTask(int Id)
        {
            Task task = null;
            TaskData taskData = GetTaskData(Id);

            if (taskData == null) return task;

            switch (taskData.TaskType)
            {
                case (int)EnumTaskType.MainTask:
                    task = GetMainTask(Id);
                    break;
                case (int)EnumTaskType.RandomTask:
                    task = GetRandomTask(Id);
                    break;
                default:
                    break;
            }
            return task;
        }

        public string GetTaskConditionValue(string condition, string key)
        {
            if (condition == string.Empty) return string.Empty;

            string[] conditions = condition.Split('&');
            foreach (var cond in conditions)
            {
                if (cond.StartsWith(key + "="))
                {
                    return cond.Substring(key.Length + 1);
                }
            }
            return string.Empty;
        }

        public void LoadGameTask()
        {
            // 未保存任务数据时 从最小任务ID开始主线任务
            List<int> taskIdList = new List<int>(dicMainTask.Keys);
            taskIdList.Sort();
            CurrentMainTask = dicMainTask[taskIdList[0]];
        }

        public void ChangeTaskState(EnumTaskType taskType, EnumTaskState taskState)
        {
            if (taskType == EnumTaskType.MainTask)
            {
                if (CurrentMainTask == null) return;

                CurrentMainTask.ChangeTaskState(taskState);
                if (dicMainTask.ContainsKey(CurrentMainTask.Id))
                {
                    dicMainTask[CurrentMainTask.Id].ChangeTaskState(taskState);
                }

                if (CurrentMainTask.state == EnumTaskState.Finish)
                {
                    TriggerEvent(CurrentMainTask);
                    if (dicMainTask.ContainsKey(CurrentMainTask.NextTaskId))
                    {
                        CurrentMainTask = dicMainTask[CurrentMainTask.NextTaskId];
                    }
                    else
                    {
                        CurrentMainTask = null;
                    }
                    GameEntry.Event.Fire(this, ReleaseTaskEventArgs.Create(EnumTaskType.MainTask, CurrentMainTask));
                    GameEntry.Sound.PlaySound(EnumSound.UITaskComplete);
                }
            }

            if (taskType == EnumTaskType.RandomTask)
            {
                if (CurrentRandomTask == null) return;

                CurrentRandomTask.ChangeTaskState(taskState);
                if (dicRandomTask.ContainsKey(CurrentRandomTask.Id))
                {
                    dicRandomTask[CurrentRandomTask.Id].ChangeTaskState(taskState);
                }

                if (CurrentRandomTask.state == EnumTaskState.Finish)
                {
                    TriggerEvent(CurrentRandomTask);
                    CurrentRandomTask = null;
                    GameEntry.Event.Fire(this, ReleaseTaskEventArgs.Create(EnumTaskType.RandomTask, CurrentRandomTask));
                    GameEntry.Sound.PlaySound(EnumSound.UITaskComplete);
                }
            }
        }

        private void TriggerEvent(Task task)
        {
            if (task == null) return;

            if (task.EventId == string.Empty) return;

            DataEvent dataEvent = GameEntry.Data.GetData<DataEvent>();
            string[] ids = task.EventId.Split('|');
            for (int i = 0; i < ids.Length; i++)
            {
                int id = int.Parse(ids[i]);
                dataEvent.TriggerEvent(id);
            }
        }

        private void OnEventFinish(object sender, GameEventArgs e)
        {
            EventFinishEventArgs ne = (EventFinishEventArgs)e;
            if (ne == null) return;

            Event m_Event = ne.m_Event;
            if (m_Event.Trigger != string.Empty)
            {
                string[] Ids = m_Event.Trigger.Split('&');
                for (int i = 0; i < Ids.Length; i++)
                {
                    if (!Ids[i].StartsWith(Constant.Parameter.Task)) continue;

                    int taskId = int.Parse(Ids[i].Substring(Constant.Parameter.Task.Length + 1));

                    TaskData taskData = GetTaskData(taskId);
                    if (taskData.TaskType == (int)EnumTaskType.MainTask)
                    {
                        if (CurrentMainTask != null) continue;

                        if (!dicMainTask.ContainsKey(taskId)) continue;

                        if (dicMainTask[taskId].state == EnumTaskState.Finish) continue;

                        CurrentMainTask = dicMainTask[taskId];
                        GameEntry.Event.Fire(this, ReleaseTaskEventArgs.Create(EnumTaskType.MainTask, CurrentMainTask));
                    }

                    if (taskData.TaskType == (int)EnumTaskType.RandomTask)
                    {
                        if (CurrentRandomTask != null) continue;

                        if (!dicRandomTask.ContainsKey(taskId)) continue;

                        if (dicRandomTask[taskId].state == EnumTaskState.Finish) continue;

                        CurrentRandomTask = dicRandomTask[taskId];
                        GameEntry.Event.Fire(this, ReleaseTaskEventArgs.Create(EnumTaskType.RandomTask, CurrentMainTask));
                    }
                }
            }
        }

        protected void OnRecruitStateChange(object sender, GameEventArgs e)
        {
            ChangeRecruitStateEventArgs ne = (ChangeRecruitStateEventArgs)e;
            if (ne == null) return;

            Recruit recruit = ne.recruit;
        }

        protected override void OnUnload()
        {
            UnSubscribe(EventFinishEventArgs.EventId, OnEventFinish);
            UnSubscribe(ChangeRecruitStateEventArgs.EventId, OnRecruitStateChange);
        }

        protected override void OnShutdown()
        {
        }
    }

}
