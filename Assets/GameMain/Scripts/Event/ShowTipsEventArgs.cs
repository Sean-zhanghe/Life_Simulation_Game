using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class ShowTipsEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ShowTipsEventArgs).GetHashCode();

        public ShowTipsEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string Content
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ShowTipsEventArgs Create(string content, object userData = null)
        {
            ShowTipsEventArgs showTipsEventArgs = ReferencePool.Acquire<ShowTipsEventArgs>();
            showTipsEventArgs.Content = content;
            return showTipsEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
