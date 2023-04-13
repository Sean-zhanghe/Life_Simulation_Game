using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class EventFinishEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(EventFinishEventArgs).GetHashCode();

        public EventFinishEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Data.Event m_Event
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static EventFinishEventArgs Create(Data.Event m_Event, object userData = null)
        {
            EventFinishEventArgs eventFinishEventArgs = ReferencePool.Acquire<EventFinishEventArgs>();
            eventFinishEventArgs.m_Event = m_Event;
            return eventFinishEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
