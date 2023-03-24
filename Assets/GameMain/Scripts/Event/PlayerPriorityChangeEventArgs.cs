using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{

    public class PlayerPriorityChangeEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(PlayerPriorityChangeEventArgs).GetHashCode();

        public PlayerPriorityChangeEventArgs()
        {
            PriorityType = EnumPriority.None;
            LastValue = 0;
            CurrentValue = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public float LastValue
        {
            get;
            private set;
        }

        public float CurrentValue
        {
            get;
            private set;
        }

        public int SerialId
        {
            get;
            private set;
        }

        public EnumPriority PriorityType
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static PlayerPriorityChangeEventArgs Create(int serialId, EnumPriority priorityType, float lastValue, float currentValue, object userData = null)
        {
            PlayerPriorityChangeEventArgs playerPriorityChangeEventArgs = ReferencePool.Acquire<PlayerPriorityChangeEventArgs>();
            playerPriorityChangeEventArgs.SerialId = serialId;
            playerPriorityChangeEventArgs.PriorityType = priorityType;
            playerPriorityChangeEventArgs.LastValue = lastValue;
            playerPriorityChangeEventArgs.CurrentValue= currentValue;
            playerPriorityChangeEventArgs.UserData = userData;
            return playerPriorityChangeEventArgs;
        }

        public override void Clear()
        {
            PriorityType = EnumPriority.None;
            LastValue = 0;
            CurrentValue = 0;
        }
    }
}
