using StarForce;
using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Transform firePoint;

    private DataWeapon dataWeapon;
    private DataProjectile dataProjectile;
    private DataPlayer dataPlayer;

    private float m_FireTimer;

    public bool IsPause { get; private set; }
    public bool IsDead { get; private set; }

    public void OnInit(object userData)
    {
        animator = GetComponent<Animator>();
        firePoint = transform.GetChild(0).GetComponent<Transform>();

        dataWeapon = GameEntry.Data.GetData<DataWeapon>();
        dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        dataProjectile = GameEntry.Data.GetData<DataProjectile>();
    }

    public void OnShow(object userData)
    {
        m_FireTimer = 0;

        IsPause = false;
        IsDead = false;
    }

    public void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (IsPause) return;

        if (IsDead) return;

        m_FireTimer -= elapseSeconds;

        if (m_FireTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Attack();
                m_FireTimer = dataPlayer.player.AttackInterval;
            }
        }
    }

    public void OnHide(bool isShutdown, object userData)
    {

    }

    public void OnPause()
    {
        IsPause = true;
    }

    public void OnResume()
    {
        IsPause = false;
    }

    public void OnDead()
    {
        IsDead = true;
    }

    private void SetUpTimers()
    {

    }

    private void Attack()
    {
        int weaponId = dataPlayer.player.WeaponId;
        WeaponData weapon = dataWeapon.GetWeaponDataById(weaponId);

        if (weapon == null)
        {
            // TODO 无装备武器空手近战
            animator.SetTrigger("Attack");
            return;
        }

        animator.SetTrigger(weapon.Parameter);

        // 近战
        if (weapon.WeaponType == (int)EnumWeaponType.Melee)
        {

            return;
        }

        // 远程
        if (weapon.WeaponType == (int)EnumWeaponType.Ranged)
        {
            animator.SetTrigger(weapon.Parameter);

            ProjectileData projectile = dataProjectile.GetProjectileDataById(weapon.ProjectileId);
            if (projectile == null) return;

            float inputX = animator.GetFloat(Constant.Animator.InputX);
            float inputY = animator.GetFloat(Constant.Animator.InputY);
            Vector2 direction = (transform.right * inputX + transform.up * inputY).normalized;

            GameEntry.Event.Fire(this, ShowEntityInGameEventArgs.Create(
                projectile.ProjectileEntityId,
                TypeUtility.GetEntityType(weapon.ProjectileType),
                null,
                EntityDataProjectile.Create(projectile, direction, firePoint.position)
                ));
        }
    }
}
