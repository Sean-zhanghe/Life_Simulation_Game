using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class EquipmentData
    {
        private DREquipment dREquipment;

        public int Id { get { return dREquipment.Id; } }

        public string Name { get { return dREquipment.Name; } }

        public int IconId { get { return dREquipment.IconId; } }

        public int MaxStack { get { return dREquipment.MaxStack; } }

        public string Description { get { return dREquipment.Description; } }

        public int WeaponId { get { return dREquipment.WeaponId; } }

        public int Sell { get { return dREquipment.Sell; } }

        public EquipmentData(DREquipment dREquipment)
        {
            this.dREquipment = dREquipment;
        }
    }

    #endregion

    public sealed class DataEquipment : DataBase
    {
        private IDataTable<DREquipment> dtEquipment;
        private Dictionary<int, EquipmentData> dicEquipmentData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtEquipment = GameEntry.DataTable.GetDataTable<DREquipment>();
            if (dtEquipment == null)
                throw new System.Exception("Can not get data table Sound");

            dicEquipmentData = new Dictionary<int, EquipmentData>();

            DREquipment[] dREquipments = dtEquipment.GetAllDataRows();
            foreach (var dREquipment in dREquipments)
            {
                EquipmentData equipmentData = new EquipmentData(dREquipment);
                dicEquipmentData.Add(equipmentData.Id, equipmentData);
            }
        }

        public EquipmentData GetEquipmentDataById(int equipmentId)
        {
            if (dicEquipmentData.ContainsKey(equipmentId))
            {
                return dicEquipmentData[equipmentId];
            }

            return null;
        }

        public EquipmentData[] GetAllEquipmentData()
        {
            int index = 0;
            EquipmentData[] results = new EquipmentData[dicEquipmentData.Count];
            foreach (var equipmentData in dicEquipmentData.Values)
            {
                results[index++] = equipmentData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DREquipment>();

            dtEquipment = null;
            dicEquipmentData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}