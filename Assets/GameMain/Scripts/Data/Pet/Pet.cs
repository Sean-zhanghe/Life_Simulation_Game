using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Pet : IReference
    {
        public PetData petData { get; private set; }

        public int Number { get; private set; }

        public Pet()
        {
            petData = null;
            Number = 0;
        }

        public void SetPet(PetData data)
        {
            petData = data;

            if (petData == null)
            {
                Number = 0;
            }
        }

        public void AddNumber(int value)
        {
            if (petData == null) return;

            if (Number >= petData.MaxStack) return;

            Number += value;

            if (Number < 0)
            {
                Number = 0;
                petData = null;
            }

            if (Number > petData.MaxStack)
            {
                Number = petData.MaxStack;
            }
        }

        public void SetNumber(int value)
        {
            if (petData == null) return;

            if (value < 0) return;

            Number = value;
        }

        public static Pet Create()
        {
            Pet pet = ReferencePool.Acquire<Pet>();
            return pet;
        }

        public void Clear()
        {
            petData = null;
            Number = 0;
        }
    }

}
