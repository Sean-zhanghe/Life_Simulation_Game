using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    public sealed class DataBag : DataBase
    {
        public List<Property> listProperty;
        public List<Clothes> listClothes;
        public List<Food> listFood;
        public List<Equipment> listEquipment;
        public List<Pet> listPet;
        
        public Clothes clothes;
        public Equipment equipment;
        public Property property;
        public Pet pet;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            float propertySlotCount = GameEntry.Config.GetFloat(Constant.Config.BagPropertySlotCount);
            listProperty = new List<Property>();
            for (int i = 0; i < propertySlotCount; i++)
            {
                listProperty.Add(Property.Create());
            }
            property = new Property();

            float clothesSlotCount = GameEntry.Config.GetFloat(Constant.Config.BagClothesSlotCount);
            listClothes = new List<Clothes>();
            for (int i = 0; i < clothesSlotCount; i++)
            {
                listClothes.Add(Clothes.Create());
            }
            clothes = new Clothes();

            float foodSlotCount = GameEntry.Config.GetFloat(Constant.Config.BagFoodSlotCount);
            listFood = new List<Food>();
            for (int i = 0; i < foodSlotCount; i++)
            {
                listFood.Add(Food.Create());
            }

            float equipmentSlotCount = GameEntry.Config.GetFloat(Constant.Config.BagEquipmentSlotCount);
            listEquipment = new List<Equipment>();
            for (int i = 0; i < equipmentSlotCount; i++)
            {
                listEquipment.Add(Equipment.Create());
            }
            equipment = new Equipment();

            float petSlotCount = GameEntry.Config.GetFloat(Constant.Config.BagPetSlotCount);
            listPet = new List<Pet>();
            for (int i = 0; i < petSlotCount; i++)
            {
                listPet.Add(Pet.Create());
            }
            pet = new Pet();

            DataClothes dataClothes = GameEntry.Data.GetData<DataClothes>();
            ClothesData data = dataClothes.GetClothesDataById(1001);
            ClothesData data1 = dataClothes.GetClothesDataById(1002);

            listClothes[0].SetClothes(data);
            listClothes[0].SetNumber(1);
        }

        public void ExchangeBagSlot(EnumBag bag, string from, string to)
        {
            int fromId = int.Parse(from.Split('_')[1]);
            int toId = int.Parse(to.Split('_')[1]);
            switch (bag)
            {
                case EnumBag.Property:
                    listProperty = SwapListItem<Property>(listProperty, fromId, toId);
                    break;
                case EnumBag.Clothes:
                    listClothes = SwapListItem<Clothes>(listClothes, fromId, toId);
                    break;
                case EnumBag.Food:
                    listFood = SwapListItem<Food>(listFood, fromId, toId);
                    break;
                case EnumBag.Equipment:
                    listEquipment = SwapListItem<Equipment>(listEquipment, fromId, toId);
                    break;
                case EnumBag.Pet:
                    listPet = SwapListItem<Pet>(listPet, fromId, toId);
                    break;
                default:
                    break;
            }
        }

        public void ExchangeClothes(string from, string to)
        {
            int index;
            if (from == "Slot_Clothes")
            {
                index = int.Parse(to.Split('_')[1]);
            }
            else
            {
                index = int.Parse(from.Split('_')[1]);
            }
            Clothes temp = listClothes[index];
            listClothes[index] = clothes;
            clothes = temp;

            GameEntry.Event.Fire(this, ChangeClothesEventArgs.Create());
        }

        public void ExchangeWeapon(string from, string to)
        {
            int index;
            if (from == "Slot_Weapon")
            {
                index = int.Parse(to.Split('_')[1]);
            }
            else
            {
                index = int.Parse(from.Split('_')[1]);
            }
            Equipment temp = listEquipment[index];
            listEquipment[index] = equipment;
            equipment = temp;
        }

        public void ExchangeRing(string from, string to)
        {
            int index;
            if (from == "Slot_Ring")
            {
                index = int.Parse(to.Split('_')[1]);
            }
            else
            {
                index = int.Parse(from.Split('_')[1]);
            }
            Property temp = listProperty[index];
            listProperty[index] = property;
            property = temp;
        }

        public void ExchangePet(string from, string to)
        {
            int index;
            if (from == "Slot_Pet")
            {
                index = int.Parse(to.Split('_')[1]);
            }
            else
            {
                index = int.Parse(from.Split('_')[1]);
            }
            Pet temp = listPet[index];
            listPet[index] = pet;
            pet = temp;
        }

        private List<T> SwapListItem<T>(List<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }

        protected override void OnUnload()
        {

        }

        protected override void OnShutdown()
        {

        }
    }

}