using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class ReleaseEventEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ReleaseEventEventArgs).GetHashCode();

        public ReleaseEventEventArgs()
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

        public static ReleaseEventEventArgs Create(Data.Event m_Event, object userData = null)
        {
            ReleaseEventEventArgs releaseEventEventArgs = ReferencePool.Acquire<ReleaseEventEventArgs>();
            releaseEventEventArgs.m_Event = m_Event;
            return releaseEventEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
