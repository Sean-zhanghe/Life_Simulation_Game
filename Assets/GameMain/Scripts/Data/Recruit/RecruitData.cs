using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class RecruitData
    {
        private DRRecruit dRRecruit;

        public int Id { get { return dRRecruit.Id; } }

        public string Name { get { return dRRecruit.Name; } }

        public string Description { get { return dRRecruit.Description; } }

        public int Pay { get { return dRRecruit.Pay; } }

        public string PayMode { get { return dRRecruit.PayMode; } }

        public string Address { get { return dRRecruit.Address; } }

        public string Condition { get { return dRRecruit.Condition; } }

        public RecruitData(DRRecruit dRRecruit)
        {
            this.dRRecruit = dRRecruit;
        }
    }
}
