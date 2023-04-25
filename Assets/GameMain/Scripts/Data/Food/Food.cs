using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Food : IReference
    {
        public FoodData foodData { get; private set; }

        public int Number { get; private set; }

        public Food()
        {
            foodData = null;
            Number = 0;
        }

        public void SetFood(FoodData data)
        {
            foodData = data;
            Number = 1;

            if (foodData == null)
            {
                Number = 0;
            }
        }

        public void AddNumber(int value)
        {
            if (foodData == null) return;

            Number += value;

            if (Number < 0)
            {
                Number = 0;
                foodData = null;
            }

            if (Number > foodData.MaxStack)
            {
                Number = foodData.MaxStack;
            }
        }

        public void SetNumber(int value)
        {
            if (foodData == null) return;

            if (value < 0) return;

            Number = value;
        }

        public static Food Create()
        {
            Food food = ReferencePool.Acquire<Food>();
            return food;
        }

        public void Clear()
        {
            foodData = null;
            Number = 0;
        }
    }

}
