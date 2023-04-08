using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    #region Data

    public sealed class PetData
    {
        private DRPet dRPet;

        public int Id { get { return dRPet.Id; } }

        public string Name { get { return dRPet.Name; } }

        public int IconId { get { return dRPet.IconId; } }

        public int MaxStack { get { return dRPet.MaxStack; } }

        public string Description { get { return dRPet.Description; } }

        public int Sell { get { return dRPet.Sell; } }

        public PetData(DRPet dRPet)
        {
            this.dRPet = dRPet;
        }
    }

    #endregion

    public sealed class DataPet : DataBase
    {
        private IDataTable<DRPet> dtPet;
        private Dictionary<int, PetData> dicPetData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtPet = GameEntry.DataTable.GetDataTable<DRPet>();
            if (dtPet == null)
                throw new System.Exception("Can not get data table Sound");

            dicPetData = new Dictionary<int, PetData>();

            DRPet[] dRPets = dtPet.GetAllDataRows();
            foreach (var dRPet in dRPets)
            {
                PetData petData = new PetData(dRPet);
                dicPetData.Add(petData.Id, petData);
            }
        }

        public PetData GetPetDataById(int petId)
        {
            if (dicPetData.ContainsKey(petId))
            {
                return dicPetData[petId];
            }

            return null;
        }

        public PetData[] GetAllFoodData()
        {
            int index = 0;
            PetData[] results = new PetData[dicPetData.Count];
            foreach (var petData in dicPetData.Values)
            {
                results[index++] = petData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRFood>();

            dtPet = null;
            dicPetData = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}