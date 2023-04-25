using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class FoodData
    {
        private DRFood dRFood;

        public int Id { get { return dRFood.Id; } }

        public string Name { get { return dRFood.Name; } }

        public int IconId { get { return dRFood.IconId; } }

        public int MaxStack { get { return dRFood.MaxStack; } }

        public string Description { get { return dRFood.Description; } }

        public string Attribute { get { return dRFood.Attribute; } }

        public int Sell { get { return dRFood.Sell; } }

        public FoodData(DRFood dRFood)
        {
            this.dRFood = dRFood;
        }
    }

    #endregion

    public sealed class DataFood : DataBase
    {
        private IDataTable<DRFood> dtFood;
        private Dictionary<int, FoodData> dicFoodData;

        public List<int> listCanteen;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtFood = GameEntry.DataTable.GetDataTable<DRFood>();
            if (dtFood == null)
                throw new System.Exception("Can not get data table Sound");

            dicFoodData = new Dictionary<int, FoodData>();

            DRFood[] dRFoods = dtFood.GetAllDataRows();
            foreach (var dRFood in dRFoods)
            {
                FoodData foodData = new FoodData(dRFood);
                dicFoodData.Add(foodData.Id, foodData);
            }

            listCanteen = new List<int>() {
                1001,
            };
        }

        public FoodData GetFoodDataById(int foodId)
        {
            if (dicFoodData.ContainsKey(foodId))
            {
                return dicFoodData[foodId];
            }

            return null;
        }

        public FoodData[] GetAllFoodData()
        {
            int index = 0;
            FoodData[] results = new FoodData[dicFoodData.Count];
            foreach (var foodData in dicFoodData.Values)
            {
                results[index++] = foodData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRFood>();

            dtFood = null;
            dicFoodData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}