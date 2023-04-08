using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class ChangeClothesEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ChangeClothesEventArgs).GetHashCode();

        public ChangeClothesEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ChangeClothesEventArgs Create(object userData = null)
        {
            ChangeClothesEventArgs changeClothesEventArgs = ReferencePool.Acquire<ChangeClothesEventArgs>();
            return changeClothesEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
