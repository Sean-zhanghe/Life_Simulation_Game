using GameFramework;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class Recruit : IReference
    {
        public RecruitData recruitData { get; private set; } 

        public int Id { get { return recruitData.Id; } }

        public string Name { get { return recruitData.Name; } }

        public string Description { get { return recruitData.Description; } }

        public int Pay { get { return recruitData.Pay; } }

        public string PayMode { get { return recruitData.PayMode; } }

        public string Address { get { return recruitData.Address; } }

        public string Condition { get { return recruitData.Condition; } }

        public EnumWorkState state { get; private set; }

        public void ChangeWorkState(EnumWorkState workState)
        {
            if (state == EnumWorkState.Finish) return;

            if (workState == state) return;

            state = workState;
        }

        public Recruit()
        {
            recruitData = null;
            state = EnumWorkState.Apply;
        }

        public static Recruit Create(RecruitData recruitData)
        {
            Recruit recruit = ReferencePool.Acquire<Recruit>();
            recruit.recruitData = recruitData;
            return recruit;
        }

        public void Clear()
        {
            recruitData = null;
            state = EnumWorkState.Apply;
        }
    }
}
