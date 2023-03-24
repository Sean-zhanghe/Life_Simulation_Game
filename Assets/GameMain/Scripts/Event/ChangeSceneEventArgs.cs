using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class ChangeSceneEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ChangeSceneEventArgs).GetHashCode();

        public ChangeSceneEventArgs()
        {
            SceneId = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int SceneId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ChangeSceneEventArgs Create(int nextSceneId, object userData = null)
        {
            ChangeSceneEventArgs changeSceneEventArgs = ReferencePool.Acquire<ChangeSceneEventArgs>();
            changeSceneEventArgs.SceneId = nextSceneId;
            changeSceneEventArgs.UserData = userData;
            return changeSceneEventArgs;
        }

        public override void Clear()
        {
            SceneId = 0;
        }
    }
}
