using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Equipment : IReference
    {
        public EquipmentData equipmentData { get; private set; }

        public int Number { get; private set; }

        public Equipment()
        {
            equipmentData = null;
            Number = 0;
        }

        public void SetEquipment(EquipmentData data)
        {
            equipmentData = data;

            if (equipmentData == null)
            {
                Number = 0;
            }
        }

        public void AddNumber(int value)
        {
            if (equipmentData == null) return;

            if (Number >= equipmentData.MaxStack) return;

            Number += value;

            if (Number < 0)
            {
                Number = 0;
                equipmentData = null;
            }

            if (Number > equipmentData.MaxStack)
            {
                Number = equipmentData.MaxStack;
            }
        }

        public void SetNumber(int value)
        {
            if (equipmentData == null) return;

            if (value < 0) return;

            Number = value;
        }

        public static Equipment Create()
        {
            Equipment equipment = ReferencePool.Acquire<Equipment>();
            return equipment;
        }

        public void Clear()
        {
            equipmentData = null;
            Number = 0;
        }
    }

}
