using GameFramework;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Task : IReference
    {
        public TaskData taskData { get; private set; } 

        public int Id { get { return taskData.Id; } }

        public int TaskType { get { return taskData.TaskType; } }

        public string Description { get { return taskData.Description; } }

        public string TaskCondition { get { return taskData.TaskCondition; } }

        public string Reward { get { return taskData.Reward; } }

        public int NextTaskId { get { return taskData.NextTaskId; } }

        public EnumTaskState state { get; private set; }

        public void ChangeTaskState(EnumTaskState taskState)
        {
            if (state == EnumTaskState.Finish) return;

            if (taskState == state) return;

            state = taskState;
        }

        public Task()
        {
            taskData = null;
            state = EnumTaskState.UnFinish;
        }

        public static Task Create(TaskData taskData)
        {
            Task task = ReferencePool.Acquire<Task>();
            task.taskData = taskData;
            return task;
        }

        public void Clear()
        {

        }
    }
}
