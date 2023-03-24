using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class PlayerLevelData
    {
        private DRPlayerLevel dRPlayerLevel;

        public int Level
        {
            get { return dRPlayerLevel.Id; }
        }

        public float Power
        {
            get { return dRPlayerLevel.Power; }
        }

        public float Energy
        {
            get { return dRPlayerLevel.Energy; }
        }

        public float Hygiene
        {
            get { return dRPlayerLevel.Hygiene; }
        }

        public float Health
        {
            get { return dRPlayerLevel.Health; }
        }

        public float Attack
        {
            get { return dRPlayerLevel.Attack; }
        }

        public float Defence
        {
            get { return dRPlayerLevel.Defence; }
        }

        public float MaxHP
        {
            get { return dRPlayerLevel.MaxHP; }
        }

        public float MaxEXP
        {
            get { return dRPlayerLevel.MaxEXP; }
        }

        public PlayerLevelData(DRPlayerLevel dRPlayerLevel)
        {
            this.dRPlayerLevel = dRPlayerLevel;
        }
    }

}
