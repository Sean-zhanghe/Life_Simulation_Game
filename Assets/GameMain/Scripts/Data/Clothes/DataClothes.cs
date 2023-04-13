using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class ClothesData
    {
        private DRClothes dRClothes;

        public int Id { get { return dRClothes.Id; } }

        public string Name { get { return dRClothes.Name; } }

        public int IconId { get { return dRClothes.IconId; } }

        public int MaxStack { get { return dRClothes.MaxStack; } }

        public int Endurance { get { return dRClothes.Endurance; } }

        public string Description { get { return dRClothes.Description; } }

        public int Sell { get { return dRClothes.Sell; } }

        public int Clothes { get { return dRClothes.Clothes; } }

        public ClothesData(DRClothes dRClothes)
        {
            this.dRClothes = dRClothes;
        }
    }

    #endregion

    public sealed class DataClothes : DataBase
    {
        private IDataTable<DRClothes> dtClothes;
        private Dictionary<int, ClothesData> dicClothesData;

        public List<int> listClothesStore;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtClothes = GameEntry.DataTable.GetDataTable<DRClothes>();
            if (dtClothes == null)
                throw new System.Exception("Can not get data table Sound");

            dicClothesData = new Dictionary<int, ClothesData>();

            DRClothes[] dRClothes = dtClothes.GetAllDataRows();
            foreach (var dRCloth in dRClothes)
            {
                ClothesData clothesData = new ClothesData(dRCloth);
                dicClothesData.Add(clothesData.Id, clothesData);
            }

            listClothesStore = new List<int>() { 
                1001,
            };
        }

        public ClothesData GetClothesDataById(int clothesId)
        {
            if (dicClothesData.ContainsKey(clothesId))
            {
                return dicClothesData[clothesId];
            }

            return null;
        }

        public ClothesData[] GetAllClothesData()
        {
            int index = 0;
            ClothesData[] results = new ClothesData[dicClothesData.Count];
            foreach (var clothesData in dicClothesData.Values)
            {
                results[index++] = clothesData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRClothes>();

            dtClothes = null;
            dicClothesData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}