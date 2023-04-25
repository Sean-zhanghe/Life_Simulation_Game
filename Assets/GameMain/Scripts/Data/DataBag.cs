using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityEngine.Assertions.Must;
using GameFramework;
using System;

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

        public bool AddProperty(int propertyId)
        {
            DataProperty dataProperty = GameEntry.Data.GetData<DataProperty>();
            PropertyData property = dataProperty.GetPropertyDataById(propertyId);
            if (property == null) return false;

            string content = string.Empty;
            int emptySlot = -1;
            for (int i = 0; i < listProperty.Count; i++)
            {
                if (emptySlot == -1 && listProperty[i].Number == 0)
                {
                    emptySlot = i;
                }

                if (listProperty[i].Number > 0 && listProperty[i].propertyData.Id == property.Id)
                {
                    if (listProperty[i].Number < property.MaxStack)
                    {
                        listProperty[i].AddNumber(1);

                        content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupGetItem), property.Name);
                        GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                        return true;
                    }
                }
            }
            if (emptySlot == -1)
            {
                GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(Constant.Localization.BagTitle),
                    Message = GameEntry.Localization.GetString(Constant.Localization.BagNoEmptySlotMessage),
                    UserData = null
                });
                return false;
            }

            listProperty[emptySlot].SetProperty(property);
            content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupGetItem), property.Name);
            GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
            return true;
        }

        public bool AddClothes(int clothesId)
        {
            DataClothes dataClothes = GameEntry.Data.GetData<DataClothes>();
            ClothesData clothes = dataClothes.GetClothesDataById(clothesId);
            if (clothes == null) return false;

            string content = string.Empty;
            int emptySlot = -1;
            for (int i = 0; i < listClothes.Count; i++)
            {
                if (emptySlot == -1 && listClothes[i].Number == 0)
                {
                    emptySlot = i;
                }

                if (listClothes[i].Number > 0 && listClothes[i].clothesData.Id == clothes.Id)
                {
                    if (listClothes[i].Number < clothes.MaxStack)
                    {
                        listClothes[i].AddNumber(1);

                        content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupGetItem), clothes.Name);
                        GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                        return true;
                    }
                }
            }
            if (emptySlot == -1)
            {
                GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(Constant.Localization.BagTitle),
                    Message = GameEntry.Localization.GetString(Constant.Localization.BagNoEmptySlotMessage),
                    UserData = null
                });
                return false;
            }

            listClothes[emptySlot].SetClothes(clothes);
            content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupGetItem), clothes.Name);
            GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
            return true;
        }


        public bool AddFood(int foodId)
        {
            DataFood dataFood = GameEntry.Data.GetData<DataFood>();
            FoodData food = dataFood.GetFoodDataById(foodId);
            if (food == null) return false;

            string content = string.Empty;
            int emptySlot = -1;
            for (int i = 0; i < listFood.Count; i++)
            {
                if (emptySlot == -1 && listFood[i].Number == 0)
                {
                    emptySlot = i;
                }

                if (listFood[i].Number > 0 && listFood[i].foodData.Id == food.Id)
                {
                    if (listFood[i].Number < food.MaxStack)
                    {
                        listFood[i].AddNumber(1);

                        content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupGetItem), food.Name);
                        GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                        return true;
                    }
                }
            }
            if (emptySlot == -1)
            {
                GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(Constant.Localization.BagTitle),
                    Message = GameEntry.Localization.GetString(Constant.Localization.BagNoEmptySlotMessage),
                    UserData = null
                });
                return false;
            }

            listFood[emptySlot].SetFood(food);
            content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupGetItem), food.Name);
            GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
            return true;
        }

        public bool AddEquipment(int equipmentId)
        {
            return true;

        }

        public bool AddPet(int petId)
        {
            return true;
        }

        public bool ReduceFood(int index, int value)
        {
            if (index < 0 || index >= listFood.Count) return false;

            Food food = listFood[index];
            DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            for (int i = 0; i < value; i++)
            {
                dataPlayer.AddRewardByConfiger(food.foodData.Attribute);
            }
            listFood[index].AddNumber(-value);
            GameEntry.Event.Fire(this, RefreshBagEventArgs.Create(EnumBag.Food));

            return true;
        }

        private List<T> SwapListItem<T>(List<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }

        public bool FindItemIsInBag(EnumBag bag, int id)
        {
            switch (bag)
            {
                case EnumBag.Property:
                    foreach (var property in listProperty)
                    {
                        if (property.Number <= 0) continue;
                        if (property.propertyData.Id == id)
                            return true;
                    }
                    break;
                case EnumBag.Clothes:
                    foreach (var clothes in listClothes)
                    {
                        if (clothes.Number <= 0) continue;
                        if (clothes.clothesData.Id == id)
                            return true;
                    }
                    break;
                case EnumBag.Food:
                    foreach (var food in listFood)
                    {
                        if (food.Number <= 0) continue;
                        if (food.foodData.Id == id)
                            return true;
                    }
                    break;
                case EnumBag.Equipment:
                    foreach (var equipment in listEquipment)
                    {
                        if (equipment.Number <= 0) continue;
                        if (equipment.equipmentData.Id == id)
                            return true;
                    }
                    break;
                case EnumBag.Pet:
                    foreach (var pet in listPet)
                    {
                        if (pet.Number <= 0) continue;
                        if (pet.petData.Id == id)
                            return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        protected override void OnUnload()
        {

        }

        protected override void OnShutdown()
        {

        }
    }

}