using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class LoadGameSceneEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(LoadGameSceneEventArgs).GetHashCode();

        public LoadGameSceneEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int From
        {
            get;
            private set;
        }

        public int To { 
            get; 
            private set; 
        }

        public object UserData
        {
            get;
            private set;
        }

        public static LoadGameSceneEventArgs Create(int to, object userData = null)
        {
            LoadGameSceneEventArgs loadGameSceneEventArgs = ReferencePool.Acquire<LoadGameSceneEventArgs>();
            loadGameSceneEventArgs.To = to;
            return loadGameSceneEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
