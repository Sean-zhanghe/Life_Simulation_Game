using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class NPCData
    {
        private DRNPC dRNPC;

        public int Id { get { return dRNPC.Id; } }

        public string Name { get { return dRNPC.Name; } }

        public int IconId { get { return dRNPC.IconId; } }

        public int EntityId { get { return dRNPC.EntityId; } }

        public int PreTaskId { get { return dRNPC.PreTaskId; } }

        public string Task { get { return dRNPC.Task; } }

        public int DefDialogId { get { return dRNPC.DefDialogId; } }

        public NPCData(DRNPC dRNPC)
        {
            this.dRNPC = dRNPC;
        }
    }

    #endregion

    public sealed class DataNPC : DataBase
    {
        private IDataTable<DRNPC> dtNPC;
        private Dictionary<int, NPCData> dicNPCData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtNPC = GameEntry.DataTable.GetDataTable<DRNPC>();
            if (dtNPC == null)
                throw new System.Exception("Can not get data table Sound");

            dicNPCData = new Dictionary<int, NPCData>();

            DRNPC[] dRNPCs = dtNPC.GetAllDataRows();
            foreach (var dRNPC in dRNPCs)
            {
                NPCData NPCData = new NPCData(dRNPC);
                dicNPCData.Add(NPCData.Id, NPCData);
            }
        }

        public NPCData GetNPCDataById(int NPCId)
        {
            if (dicNPCData.ContainsKey(NPCId))
            {
                return dicNPCData[NPCId];
            }

            return null;
        }

        public NPCData[] GetAllNPCData()
        {
            int index = 0;
            NPCData[] results = new NPCData[dicNPCData.Count];
            foreach (var NPCData in dicNPCData.Values)
            {
                results[index++] = NPCData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRNPC>();

            dtNPC = null;
            dicNPCData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}