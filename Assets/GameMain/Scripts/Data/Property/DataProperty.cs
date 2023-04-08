using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class PropertyData
    {
        private DRProperty dRProperty;

        public int Id { get { return dRProperty.Id; } }

        public string Name { get { return dRProperty.Name; } }

        public int IconId { get { return dRProperty.IconId; } }

        public int MaxStack { get { return dRProperty.MaxStack; } }

        public string Description { get { return dRProperty.Description; } }

        public int Sell { get { return dRProperty.Sell; } }

        public PropertyData(DRProperty dRProperty)
        {
            this.dRProperty = dRProperty;
        }
    }

    #endregion

    public sealed class DataProperty : DataBase
    {
        private IDataTable<DRProperty> dtProperty;
        private Dictionary<int, PropertyData> dicPropertyData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtProperty = GameEntry.DataTable.GetDataTable<DRProperty>();
            if (dtProperty == null)
                throw new System.Exception("Can not get data table Sound");

            dicPropertyData = new Dictionary<int, PropertyData>();

            DRProperty[] dRPropertys = dtProperty.GetAllDataRows();
            foreach (var dRProperty in dRPropertys)
            {
                PropertyData propertyData = new PropertyData(dRProperty);
                dicPropertyData.Add(propertyData.Id, propertyData);
            }
        }

        public PropertyData GetPropertyDataById(int propertyId)
        {
            if (dicPropertyData.ContainsKey(propertyId))
            {
                return dicPropertyData[propertyId];
            }

            return null;
        }

        public PropertyData[] GetAllPropertyData()
        {
            int index = 0;
            PropertyData[] results = new PropertyData[dicPropertyData.Count];
            foreach (var propertyData in dicPropertyData.Values)
            {
                results[index++] = propertyData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRProperty>();

            dtProperty = null;
            dicPropertyData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}