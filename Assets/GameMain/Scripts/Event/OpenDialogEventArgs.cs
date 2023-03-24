using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class OpenDialogEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(OpenDialogEventArgs).GetHashCode();

        public OpenDialogEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string Name { 
            get; 
            private set; 
        }

        public int IconId { 
            get;
            private set; 
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

        public static OpenDialogEventArgs Create(string name, int icon, int dialog, object userData = null)
        {
            OpenDialogEventArgs openDialogEventArgs = ReferencePool.Acquire<OpenDialogEventArgs>();
            openDialogEventArgs.Name = name;
            openDialogEventArgs.IconId = icon;
            openDialogEventArgs.DialogId = dialog;
            return openDialogEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
