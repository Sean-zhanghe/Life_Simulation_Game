using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class TaskData
    {
        private DRTask dRTask;

        public int Id { get { return dRTask.Id; } }

        public int TaskType { get { return dRTask.TaskType; } }

        public string Description { get { return dRTask.Description; } }

        public int SubTaskType { get { return dRTask.SubTaskType; } }

        public string Parameter { get { return dRTask.Parameter; } }

        public string TaskCondition { get { return dRTask.TaskCondition; } }

        public bool IsForce { get { return dRTask.IsForce; } }

        public string Reward { get { return dRTask.Reward; } }

        public int NextTaskId { get { return dRTask.NextTaskId; } }

        public string EventId { get { return dRTask.EventId; } }

        public TaskData(DRTask dRTask)
        {
            this.dRTask = dRTask;
        }
    }
}
