using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class EnemySkeletonAttack : EnemyAttack
    {
        public Transform skeletonAttackBox;

        protected override void StartAttack()
        {
            base.StartAttack();

            if (info.IsName(Constant.Animation.SkeletonAttack))
            {
                skeletonAttackBox.gameObject.SetActive(true);
            }
        }

        protected override void EndAttack()
        {
            base.EndAttack();

            if (info.IsName(Constant.Animation.SkeletonAttack))
            {
                skeletonAttackBox.gameObject.SetActive(false);
            }
        }
    }
}
