using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class EventData
    {
        private DREvent dREvent;

        public int Id { get { return dREvent.Id; } }

        public int EventType { get { return dREvent.EventType; } }

        public string Parameter { get { return dREvent.Parameter; } }

        public string Condition { get { return dREvent.Condition; } }

        public bool IsForce { get { return dREvent.IsForce; } }

        public string Reward { get { return dREvent.Reward; } }

        public string Trigger { get { return dREvent.Trigger; } }

        public bool IsRepeat { get { return dREvent.IsRepeat; } }

        public EventData(DREvent dREvent)
        {
            this.dREvent = dREvent;
        }
    }
}
