using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace StarForce.Data
{
    public class DataRecruit : DataBase
    {
        private IDataTable<DRRecruit> dtRecruit;
        private Dictionary<int, RecruitData> dicRecruitData = new Dictionary<int, RecruitData>();

        private Dictionary<int, Recruit> dicRecruit = new Dictionary<int, Recruit>();

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtRecruit = GameEntry.DataTable.GetDataTable<DRRecruit>();
            if (dtRecruit == null)
                throw new System.Exception("Can not get data table Task");

            DRRecruit[] dRRecruits = dtRecruit.GetAllDataRows();
            foreach (var dRRecruit in dRRecruits)
            {
                RecruitData recruitData = new RecruitData(dRRecruit);
                dicRecruitData.Add(recruitData.Id, recruitData);

                Recruit recruit = Recruit.Create(recruitData);
                dicRecruit.Add(recruitData.Id, recruit);
            }
        }

        public RecruitData GetRecruitDataById(int id)
        {
            if (dicRecruitData.ContainsKey(id))
            {
                return dicRecruitData[id];
            }
            return null;
        }

        public RecruitData[] GetAllRecruitData()
        {
            int index = 0;
            RecruitData[] results = new RecruitData[dicRecruitData.Count];
            foreach (var recruitData in dicRecruitData.Values)
            {
                results[index++] = recruitData;
            }
            return results;
        }

        public Recruit[] GetAllRecruit()
        {
            int index = 0;
            Recruit[] results = new Recruit[dicRecruit.Count];
            foreach (var recruit in dicRecruit.Values)
            {
                results[index++] = recruit;
            }
            return results;
        }

        public Recruit GetRecruit(int Id)
        {
            if (dicRecruit.ContainsKey(Id))
            {
                return dicRecruit[Id];
            }
            return null;
        }

        public bool CheckRecruitCondition(int recruitId)
        {
            if (!dicRecruitData.ContainsKey(recruitId)) return false;

            RecruitData recruitData = dicRecruitData[recruitId];

            if (recruitData.Condition == string.Empty) return true;

            DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            string condition = recruitData.Condition;
            string[] conditions = condition.Split('&');
            Dictionary<string, float> playerPriority = new Dictionary<string, float>()
            {
                { Constant.Parameter.Power, dataPlayer.player.Power },
                { Constant.Parameter.Energy, dataPlayer.player.Energy },
                { Constant.Parameter.Hygiene, dataPlayer.player.Hygiene },
                { Constant.Parameter.Health, dataPlayer.player.Health },
            };
            foreach (var cond in conditions)
            {
                string type = cond.Split('=')[0];
                int value = 0;
                string parameter = string.Empty;
                if (playerPriority.ContainsKey(type))
                {
                    value = int.Parse(cond.Split('=')[1]);
                    parameter = cond.Split('=')[2];
                    switch (parameter)
                    {
                        case "l":       // 小于
                            if (!(playerPriority[type] < value)) return false;
                            break;
                        case "le":      // 小于等于
                            if (!(playerPriority[type] <= value)) return false;
                            break;
                        case "e":       // 等于
                            if (!(playerPriority[type] == value)) return false;
                            break;
                        case "ge":      // 大于等于
                            if (!(playerPriority[type] >= value)) return false;
                            break;
                        case "g":       // 大于
                            if (!(playerPriority[type] > value)) return false;
                            break;
                    }
                }
            }
            return true;
        }

        public void ChangeRecruitState(int id, EnumWorkState state)
        {
            if (!dicRecruit.ContainsKey(id)) return;

            Recruit recruit = dicRecruit[id];

            if (recruit.state == state) return;

            if (recruit.state != EnumWorkState.Apply) return;

            recruit.ChangeWorkState(state);
            GameEntry.Event.Fire(ChangeRecruitStateEventArgs.EventId, ChangeRecruitStateEventArgs.Create(recruit));
        }

        private void TriggerTask(Task task)
        {
            if (task == null) return;

            if (task.EventId == string.Empty) return;

            DataEvent dataEvent = GameEntry.Data.GetData<DataEvent>();
            string[] ids = task.EventId.Split('|');
            for (int i = 0; i < ids.Length; i++)
            {
                int id = int.Parse(ids[i]);
                dataEvent.TriggerEvent(id);
            }
        }


        protected override void OnUnload()
        {

        }

        protected override void OnShutdown()
        {
        }
    }

}
