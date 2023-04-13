using StarForce.Data;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvent : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private DataWeapon dataWeapon;
    private DataProjectile dataProjectile;
    private DataPlayer dataPlayer;

    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        dataWeapon = GameEntry.Data.GetData<DataWeapon>();
        dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        dataProjectile = GameEntry.Data.GetData<DataProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

            float inputX = animator.GetFloat(Constant.Animator.InputX);
            float inputY = animator.GetFloat(Constant.Animator.InputY);
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
}
