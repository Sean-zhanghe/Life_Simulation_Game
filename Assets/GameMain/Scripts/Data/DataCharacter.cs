using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    public sealed class CharacterData
    {
        private DRCharacter dRCharacter;

        public int Id
        {
            get { return dRCharacter.Id; }
        }

        public int PlayerId
        {
            get { return dRCharacter.PlayerId; }
        }


        public CharacterData(DRCharacter dRCharacter)
        {
            this.dRCharacter = dRCharacter;
        }
    }

    public sealed class DataCharacter: DataBase
    {
        private IDataTable<DRCharacter> dtCharacter;
        private Dictionary<int, CharacterData> dicCharacter;

        protected override void OnInit()
        {
        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            if (dtCharacter == null)
                throw new System.Exception("Can not get data table PoolParam");

            dicCharacter = new Dictionary<int, CharacterData>();

            DRCharacter[] dRCharacters = dtCharacter.GetAllDataRows();
            foreach (var dRCharacter in dRCharacters)
            {
                CharacterData characterData = new CharacterData(dRCharacter);
                dicCharacter.Add(dRCharacter.Id, characterData);
            }
        }

        public CharacterData GetCharacterData(int id)
        {
            if (dicCharacter.ContainsKey(id))
            {
                return dicCharacter[id];
            }

            return null;
        }

        public CharacterData[] GetAllCharacterData()
        {
            int index = 0;
            CharacterData[] results = new CharacterData[dicCharacter.Count];
            foreach (var characterData in dicCharacter.Values)
            {
                results[index++] = characterData;
            }

            return results;
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRPoolParam>();

            dtCharacter = null;
            dicCharacter = null;
        }

        protected override void OnShutdown()
        {
        }
    }

}