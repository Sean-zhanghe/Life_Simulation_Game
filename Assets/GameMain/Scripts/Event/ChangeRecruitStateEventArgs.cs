using GameFramework;
using GameFramework.Event;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class ChangeRecruitStateEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ChangeRecruitStateEventArgs).GetHashCode();

        public ChangeRecruitStateEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Recruit recruit
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ChangeRecruitStateEventArgs Create(Recruit recruit, object userData = null)
        {
            ChangeRecruitStateEventArgs changeRecruitStateEventArgs = ReferencePool.Acquire<ChangeRecruitStateEventArgs>();
            changeRecruitStateEventArgs.recruit = recruit;
            return changeRecruitStateEventArgs;
        }

        public override void Clear()
        {

        }
    }
}
