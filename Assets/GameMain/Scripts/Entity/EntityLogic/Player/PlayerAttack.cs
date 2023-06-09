﻿using StarForce;
using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animatorHead;
    [SerializeField] private Animator animatorArm;
    [SerializeField] private Animator animatorBody;
    [SerializeField] private Animator animatorLeg;

    private Transform firePoint;

    private DataWeapon dataWeapon;
    private DataProjectile dataProjectile;
    private DataPlayer dataPlayer;

    private float m_FireTimer;

    public bool IsPause { get; private set; }
    public bool IsDead { get; private set; }

    public void OnInit(object userData)
    {
        firePoint = transform.Find("FirePoint").GetComponent<Transform>();

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
        IsDead = false;
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

    private void Attack()
    {
        int weaponId = dataPlayer.player.WeaponId;
        WeaponData weapon = dataWeapon.GetWeaponDataById(weaponId);

        if (weapon == null)
        {
            // TODO 无装备武器空手近战
            SetAnimatorTrigger(Constant.Animator.Attack);
            return;
        }

        SetAnimatorTrigger(weapon.Parameter);

        
    }

    public void AttackEnemy()
    {
        int weaponId = dataPlayer.player.WeaponId;
        WeaponData weapon = dataWeapon.GetWeaponDataById(weaponId);

        // 空手
        if (weapon == null) 
        {

            return;
        }

        // 近战
        if (weapon.WeaponType == (int)EnumWeaponType.Melee)
        {

            return;
        }

        // 远程
        if (weapon.WeaponType == (int)EnumWeaponType.Ranged)
        {

            ProjectileData projectile = dataProjectile.GetProjectileDataById(weapon.ProjectileId);
            if (projectile == null) return;

            float inputX = animatorBody.GetFloat(Constant.Animator.InputX);
            float inputY = animatorBody.GetFloat(Constant.Animator.InputY);
            Vector2 direction = (transform.right * inputX + transform.up * inputY).normalized;
            float angel = Random.Range(-2f, 2f);
            direction = Quaternion.AngleAxis(angel, Vector3.forward) * direction;

            // 调整箭矢旋转方向
            Quaternion rotation = Quaternion.identity;
            if (inputX == 0 && inputY == -1)
            {
                rotation = Quaternion.AngleAxis(90, Vector3.forward);
            }

            if (inputX == 1 && inputY == 0)
            {
                rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }

            if (inputX == 0 && inputY == 1)
            {
                rotation = Quaternion.AngleAxis(270, Vector3.forward);
            }

            GameEntry.Event.Fire(this, ShowEntityInGameEventArgs.Create(
                projectile.ProjectileEntityId,
                TypeUtility.GetEntityType(weapon.ProjectileType),
                null,
                EntityDataProjectile.Create(projectile, direction, firePoint.position, rotation)
                ));

            GameEntry.Sound.PlaySound(EnumSound.SFXThrow);
        }

    }

    public void EndAttackEnemy()
    {

    }

    private void SetAnimatorTrigger(string parameters)
    {
        animatorHead.SetTrigger(parameters);
        animatorArm.SetTrigger(parameters);
        animatorBody.SetTrigger(parameters);
        animatorLeg.SetTrigger(parameters);
    }
}
