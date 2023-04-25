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

        public string Apply { get { return recruitData.Apply; } }

        public string Finish { get { return recruitData.Finish; } }

        public string Reward { get { return recruitData.Reward; } }

        public EnumWorkState state { get; private set; }

        public int Progress { get; private set; }

        public void ChangeWorkState(EnumWorkState workState)
        {
            if (workState == state) return;

            state = workState;
        }

        public void UpdateProgress(int value)
        {
            Progress += value;
        }

        public void Reset()
        {
            state = EnumWorkState.Apply;
            Progress = 0;
        }

        public Recruit()
        {
            recruitData = null;
            state = EnumWorkState.Apply;
            Progress = 0;
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
            Progress = 0;
        }
    }
}
