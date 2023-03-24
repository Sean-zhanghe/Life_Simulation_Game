using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class DialogFinishEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(DialogFinishEventArgs).GetHashCode();

        public DialogFinishEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int DialogId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static DialogFinishEventArgs Create(int dialog, object userData = null)
        {
            DialogFinishEventArgs dialogFinishEventArgs = ReferencePool.Acquire<DialogFinishEventArgs>();
            dialogFinishEventArgs.DialogId = dialog;
            return dialogFinishEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
