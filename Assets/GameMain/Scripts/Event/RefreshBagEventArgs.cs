using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class RefreshBagEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(RefreshBagEventArgs).GetHashCode();

        public RefreshBagEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public EnumBag bag
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static RefreshBagEventArgs Create(EnumBag bag, object userData = null)
        {
            RefreshBagEventArgs refreshBagEventArgs = ReferencePool.Acquire<RefreshBagEventArgs>();
            refreshBagEventArgs.bag = bag;
            return refreshBagEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
