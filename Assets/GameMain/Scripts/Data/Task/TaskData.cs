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

        public string TaskCondition { get { return dRTask.TaskCondition; } }

        public string Reward { get { return dRTask.Reward; } }

        public int NextTaskId { get { return dRTask.NextTaskId; } }

        public TaskData(DRTask dRTask)
        {
            this.dRTask = dRTask;
        }
    }
}
