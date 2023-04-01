using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class SkeletonAttackTrigger : MonoBehaviour
    {
        private Animator animator;
        private AnimatorStateInfo info;

        private EntityLogicSkeleton skeleton;

        private void Start()
        {
            animator = transform.parent.GetComponent<Animator>();
            skeleton = transform.parent.GetComponent<EntityLogicSkeleton>();

        }

        private void OnEnable()
        {
            gameObject.layer = Constant.Layer.BulletIgnoreNameLayerId;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(Constant.Tag.Player)) return;

            EntityLogicPlayerCombat player = collision.GetComponent<EntityLogicPlayerCombat>();
            if (player == null) return;

            float damage = skeleton.enemyData.Damage;

            player.Damage(damage);
        }
    }

}
