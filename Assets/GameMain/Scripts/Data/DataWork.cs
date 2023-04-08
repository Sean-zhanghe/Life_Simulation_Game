using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class WorkData
    {
        private DRWork dRWork;

        public int Id { get { return dRWork.Id; } }

        public string Consume { get { return dRWork.Consume; } }

        public string Reward { get { return dRWork.Reward; } }

        public WorkData(DRWork dRWork)
        {
            this.dRWork = dRWork;
        }
    }

    #endregion

    public sealed class DataWork : DataBase
    {
        private IDataTable<DRWork> dtWork;
        private Dictionary<int, WorkData> dicWorkData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtWork = GameEntry.DataTable.GetDataTable<DRWork>();
            if (dtWork == null)
                throw new System.Exception("Can not get data table Sound");

            dicWorkData = new Dictionary<int, WorkData>();

            DRWork[] dRWorks = dtWork.GetAllDataRows();
            foreach (var dRWork in dRWorks)
            {
                WorkData workData = new WorkData(dRWork);
                dicWorkData.Add(workData.Id, workData);
            }
        }

        public WorkData GetWorkDataById(int workId)
        {
            if (dicWorkData.ContainsKey(workId))
            {
                return dicWorkData[workId];
            }

            return null;
        }

        public WorkData[] GetAllWorkData()
        {
            int index = 0;
            WorkData[] results = new WorkData[dicWorkData.Count];
            foreach (var workData in dicWorkData.Values)
            {
                results[index++] = workData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRWork>();

            dtWork = null;
            dicWorkData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}