using GameFramework;
using GameFramework.DataTable;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace StarForce.Data
{
    public class DataEvent : DataBase
    {
        private IDataTable<DREvent> dtEvent;
        private Dictionary<int, EventData> dicEventData = new Dictionary<int, EventData>();

        private Dictionary<int, Event> dicEvent = new Dictionary<int, Event>();

        public List<Event> CurrentEvent { get; private set; }


        protected override void OnInit()
        {
            CurrentEvent = null;
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtEvent = GameEntry.DataTable.GetDataTable<DREvent>();
            if (dtEvent == null)
                throw new System.Exception("Can not get data table Task");

            DREvent[] dREvents = dtEvent.GetAllDataRows();
            foreach (var dREvent in dREvents)
            {
                EventData eventData = new EventData(dREvent);
                dicEventData.Add(eventData.Id, eventData);

                Event m_Event = Event.Create(eventData);

                // TODO 读取数据库事件数据

                dicEvent.Add(m_Event.Id, m_Event);
            }

            CurrentEvent = new List<Event>();
        }

        public EventData GetEventDataById(int id)
        {
            if (dicEventData.ContainsKey(id))
            {
                return dicEventData[id];
            }
            return null;
        }

        public EventData[] GetAllEventDatas()
        {
            int index = 0;
            EventData[] result = new EventData[dicEventData.Count];
            foreach (var taskData in dicEventData.Values)
            {
                result[index++] = taskData;
            }
            return result;
        }

        public Event GetEvent(int Id)
        {
            if (dicEvent.ContainsKey(Id))
            {
                return dicEvent[Id];
            }
            return null;
        }

        public void ChangeEventState(int id, EnumEventState eventState)
        {
            if (CurrentEvent.Count == 0) return;

            for (int i = 0; i < CurrentEvent.Count; i++)
            {
                Event m_Event = CurrentEvent[i];
                if (m_Event.Id == id)
                {
                    m_Event.ChangeEventState(eventState);
                    if (dicEvent.ContainsKey(m_Event.Id))
                    {
                        dicEvent[m_Event.Id].ChangeEventState(eventState);
                    }

                    if (m_Event.state == EnumEventState.Finish)
                    {
                        
                        CurrentEvent.Remove(m_Event);
                        GameEntry.Event.Fire(EventFinishEventArgs.EventId, EventFinishEventArgs.Create(m_Event));
                    }

                    return;
                }
            }
        }

        public void TriggerEvent(int eventId)
        {
            if (!dicEvent.ContainsKey(eventId)) return;

            if (dicEvent[eventId].state == EnumEventState.Finish) return;

            CurrentEvent.Add(dicEvent[eventId]);
            GameEntry.Event.Fire(ReleaseEventEventArgs.EventId, ReleaseEventEventArgs.Create(dicEvent[eventId]));
        }

        public string GetEventConditionValue(string condition, string key)
        {
            if (condition == string.Empty) return string.Empty;

            string[] conditions = condition.Split('&');
            foreach (var cond in conditions)
            {
                if (cond.StartsWith(key + "="))
                {
                    return cond.Substring(key.Length + 1);
                }
            }
            return string.Empty;
        }

        protected override void OnUnload()
        {

        }

        protected override void OnShutdown()
        {
        }
    }

}
