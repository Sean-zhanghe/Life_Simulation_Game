using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class ReleaseTaskEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ReleaseTaskEventArgs).GetHashCode();

        public ReleaseTaskEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public EnumTaskType TaskType
        {
            get;
            private set;
        }

        public Task Task
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ReleaseTaskEventArgs Create(EnumTaskType taskType, Task task, object userData = null)
        {
            ReleaseTaskEventArgs releaseTaskEventArgs = ReferencePool.Acquire<ReleaseTaskEventArgs>();
            releaseTaskEventArgs.TaskType = taskType;
            releaseTaskEventArgs.Task = task;
            return releaseTaskEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
