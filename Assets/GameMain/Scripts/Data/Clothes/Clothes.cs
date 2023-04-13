using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Clothes : IReference
    {
        public ClothesData clothesData { get; private set; }

        public int Number { get; private set; }


        public Clothes()
        {
            clothesData = null;
            Number = 0;
        }

        public void SetClothes(ClothesData data)
        {
            clothesData = data;
            Number = 1;

            if (clothesData == null)
            {
                Number = 0;
            }
        }

        public void AddNumber(int value)
        {
            if (clothesData == null) return;

            if (Number >= clothesData.MaxStack) return;

            Number += value;

            if (Number < 0)
            {
                Number = 0;
                clothesData = null;
            }

            if (Number > clothesData.MaxStack)
            {
                Number = clothesData.MaxStack;
            }
        }

        public void SetNumber(int value)
        {
            if (clothesData == null) return;

            if (value < 0) return;

            Number = value;
        }

        public static Clothes Create()
        {
            Clothes clothes = ReferencePool.Acquire<Clothes>();
            return clothes;
        }

        public void Clear()
        {
            clothesData = null;
            Number = 0;
        }
    }

}
