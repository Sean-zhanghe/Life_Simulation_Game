using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class EnemyAttack : MonoBehaviour
    {
        protected Animator animator;
        protected AnimatorStateInfo info;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected virtual void StartAttack()
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
        }

        protected virtual void EndAttack()
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
        }
    }

}
