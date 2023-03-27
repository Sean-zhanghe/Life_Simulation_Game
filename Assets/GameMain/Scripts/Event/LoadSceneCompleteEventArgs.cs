using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class LoadSceneCompleteEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(LoadSceneCompleteEventArgs).GetHashCode();

        public LoadSceneCompleteEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string LastScene
        {
            get;
            private set;
        }

        public string CurrentScene
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static LoadSceneCompleteEventArgs Create(string last, string cur, object userData = null)
        {
            LoadSceneCompleteEventArgs loadSceneCompleteEventArgs = ReferencePool.Acquire<LoadSceneCompleteEventArgs>();
            loadSceneCompleteEventArgs.LastScene = last;
            loadSceneCompleteEventArgs.CurrentScene = cur;
            return loadSceneCompleteEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
