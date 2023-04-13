﻿using GameFramework;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Event : IReference
    {
        public EventData eventData { get; private set; } 

        public int Id { get { return eventData.Id; } }

        public int EventType { get { return eventData.EventType; } }

        public string Parameter { get { return eventData.Parameter; } }

        public string Condition { get { return eventData.Condition; } }

        public bool IsForce { get { return eventData.IsForce; } }

        public string Reward { get { return eventData.Reward; } }

        public string Trigger { get { return eventData.Trigger; } }

        public EnumEventState state { get; private set; }

        public void ChangeEventState(EnumEventState eventState)
        {
            if (state == EnumEventState.Finish) return;

            if (eventState == state) return;

            state = eventState;
        }

        public Event()
        {
            eventData = null;
            state = EnumEventState.UnFinish;
        }

        public static Event Create(EventData eventData)
        {
            Event m_Event = ReferencePool.Acquire<Event>();
            m_Event.eventData = eventData;
            return m_Event;
        }

        public void Clear()
        {

        }
    }
}
