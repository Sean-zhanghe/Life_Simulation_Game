using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Property : IReference
    {
        public PropertyData propertyData { get; private set; }

        public int Number { get; private set; }

        public Property()
        {
            propertyData = null;
            Number = 0;
        }

        public void SetProperty(PropertyData data)
        {
            propertyData = data;
            Number = 1;

            if (propertyData == null)
            {
                Number = 0;
            }
        }

        public void AddNumber(int value)
        {
            if (propertyData == null) return;

            if (Number >= propertyData.MaxStack) return;

            Number += value;

            if (Number < 0)
            {
                Number = 0;
                propertyData = null;
            }

            if (Number > propertyData.MaxStack)
            {
                Number = propertyData.MaxStack;
            }
        }

        public void SetNumber(int value)
        {
            if (propertyData == null) return;

            if (value < 0) return;

            Number = value;
        }

        public static Property Create()
        {
            Property property = ReferencePool.Acquire<Property>();
            return property;
        }

        public void Clear()
        {
            propertyData = null;
            Number = 0;
        }
    }

}
