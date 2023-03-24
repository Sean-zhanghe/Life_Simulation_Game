using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class WorkFinishEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(WorkFinishEventArgs).GetHashCode();

        public WorkFinishEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int WorkId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static WorkFinishEventArgs Create(int workId, object userData = null)
        {
            WorkFinishEventArgs workFinishEventArgs = ReferencePool.Acquire<WorkFinishEventArgs>();
            workFinishEventArgs.WorkId = workId;
            return workFinishEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
