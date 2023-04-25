using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            if (dicRecruit[id].state == state) return;

            dicRecruit[id].ChangeWorkState(state);
            GameEntry.Event.Fire(ChangeRecruitStateEventArgs.EventId, ChangeRecruitStateEventArgs.Create(dicRecruit[id]));
        }

        public void CheckRecruit(string condition, string value)
        {
            DataEntity dataEntity = GameEntry.Data.GetData<DataEntity>();
            RecruitData[] recruitDatas = GetAllRecruitData();
            DataEvent dataEvent = GameEntry.Data.GetData<DataEvent>();
            for (int i = 0; i < recruitDatas.Length; i++)
            {
                RecruitData recruitData = recruitDatas[i];
                Recruit recruit = GetRecruit(recruitData.Id);
                if (recruit.state == EnumWorkState.Applied)
                {
                    if (recruit.Apply == string.Empty) continue;

                    string[] applies = recruit.Apply.Split('&');

                    bool isContinue = false;
                    for (int j = 0; j < applies.Length; j++)
                    {
                        string type = applies[j].Split('=')[0];
                        string applyValue = applies[j].Split('=')[1];

                        if (type == Constant.Parameter.Event)
                        { 
                            Event m_Event = dataEvent.GetEvent(int.Parse(applyValue));
                            if (m_Event.state != EnumEventState.Finish) 
                            {
                                isContinue = true;
                                break;
                            };
                        }
                    }
                    if (isContinue) continue;
                    ChangeRecruitState(recruit.Id, EnumWorkState.Working);
                    continue;
                }

                if (recruit.state == EnumWorkState.Working)
                {
                    if (recruit.Finish == string.Empty) continue;

                    string[] finishes = recruit.Finish.Split('&');

                    bool isContinue = false;
                    for (int j = 0; j < finishes.Length; j++)
                    {
                        string type = finishes[j].Split('=')[0];
                        string finishValue = finishes[j].Split('=')[1];
                        if (type == Constant.Parameter.Entity)
                        { 
                            int count = int.Parse(finishes[j].Split('=')[2]);
                            // 当前检查条件和该条件一致
                            if (condition == type)
                            {
                                EntityData entityData = dataEntity.GetEntityData(int.Parse(finishValue));
                                if (entityData.Name == value)
                                {
                                    if (recruit.Progress < count)
                                    {
                                        UpdateRecruitProgress(recruit.Id, 1);
                                    }
                                }
                            }
                            if (dicRecruit[recruit.Id].Progress < count) isContinue = true;
                            continue;
                        }
                    }
                    if (isContinue) continue;
                    ChangeRecruitState(recruit.Id, EnumWorkState.Finish);
                }
            }
        }

        public void UpdateRecruitProgress(int recruitId, int value)
        {
            if (!dicRecruit.ContainsKey(recruitId)) return;
            dicRecruit[recruitId].UpdateProgress(value);
        }

        protected override void OnUnload()
        {

        }

        protected override void OnShutdown()
        {
        }
    }

}
