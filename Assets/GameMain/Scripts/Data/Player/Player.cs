using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGameFramework.Runtime;

namespace StarForce.Data
{
    public class Player : IReference
    {
        private PlayerData playerData;

        public int Level { get; private set; }

        public int SerialId { get; private set; }

        public string Name { get; private set; }

        public int WeaponId { get; private set; }

        public float Power { get; private set; }

        public float Energy { get; private set; }

        public float Hygiene { get; private set; }

        public float Health { get; private set; }

        public float HP { get; private set; }

        public float EXP { get; private set; }

        public float Money { get; private set; }

        public int EntityId { get { return playerData.EntityId; } }

        public int MaxLevel { get { return playerData.GetMaxLevel(); } }

        public bool IsMaxLevel { get { return Level >= playerData.GetMaxLevel(); } }

        public float Attack { get { return GetMaxPriority(EnumPriority.Attack, Level); } }

        public float Defence { get { return GetMaxPriority(EnumPriority.Defence, Level); } }

        public float MaxPower { get { return GetMaxPriority(EnumPriority.Power, Level); } }

        public float MaxEnergy { get { return GetMaxPriority(EnumPriority.Energy, Level); } }

        public float MaxHygiene { get { return GetMaxPriority(EnumPriority.Hygiene, Level); } }

        public float MaxHealth { get { return GetMaxPriority(EnumPriority.Health, Level); } }

        public float MaxHP { get { return GetMaxPriority(EnumPriority.HP, Level); } }

        public float MaxEXP { get { return GetMaxPriority(EnumPriority.EXP, Level); } }

        public bool IsDead { get { return HP <= 0; } }

        public float AttackInterval { get { return GameEntry.Config.GetInt(Constant.Config.AttackInterval); } }

        public Player()
        {
            playerData = null;
            SerialId = 0;
            Name = "";
            Level = 1;
            HP = 0;
            Power = 0;
            Energy = 0;
            Hygiene = 0;
            Health = 0;
            EXP = 0;
            Money = 0;
        }

        public float GetMaxPriority(EnumPriority priorityType, int level = 1)
        {
            if (level < 1 || level > MaxLevel)
            {
                Log.Error("Param level '{0} invaild'", level);
                return 0;
            }

            float result = 0;
            switch (priorityType)
            {
                case EnumPriority.Power:
                    //result = playerData.GetPlayerLevelData(level).Power;
                    result = GameEntry.Config.GetInt(Constant.Config.Power);
                    break;
                case EnumPriority.Energy:
                    //result = playerData.GetPlayerLevelData(level).Energy;
                    result = GameEntry.Config.GetInt(Constant.Config.Energy);
                    break;
                case EnumPriority.Hygiene:
                    //result = playerData.GetPlayerLevelData(level).Hygiene;
                    result = GameEntry.Config.GetInt(Constant.Config.Hygiene);
                    break;
                case EnumPriority.Health:
                    //result = playerData.GetPlayerLevelData(level).Health;
                    result = GameEntry.Config.GetInt(Constant.Config.Health);
                    break;
                case EnumPriority.HP:
                    result = playerData.GetPlayerLevelData(level).MaxHP;
                    break;
                case EnumPriority.EXP:
                    result = playerData.GetPlayerLevelData(level).MaxEXP;
                    break;
                case EnumPriority.Attack:
                    result = playerData.GetPlayerLevelData(level).Attack;
                    break;
                case EnumPriority.Defence:
                    result = playerData.GetPlayerLevelData(level).Defence;
                    break;
                case EnumPriority.Level:
                    result = playerData.GetMaxLevel();
                    break;
                default:
                    break;
            }
            return result;
        }

        public void SetWeapon(int weapon)
        {
            WeaponId = weapon;
        }

        public void AddPriority(EnumPriority priorityType, float value)
        {
            switch (priorityType)
            {
                case EnumPriority.Power:
                    Power = Mathf.Clamp(Power + value, 0, MaxPower);
                    break;
                case EnumPriority.Energy:
                    Energy = Mathf.Clamp(Energy + value, 0, MaxEnergy);
                    break;
                case EnumPriority.Hygiene:
                    Hygiene = Mathf.Clamp(Hygiene + value, 0, MaxHygiene);
                    break;
                case EnumPriority.Health:
                    Health = Mathf.Clamp(Health + value, 0, MaxHealth);
                    break;
                case EnumPriority.HP:
                    HP = Mathf.Clamp(HP + value, 0, MaxHP);
                    break;
                case EnumPriority.EXP:
                    EXP = Mathf.Clamp(EXP + value, 0, MaxEXP);
                    break;
                case EnumPriority.Money:
                    Money = Mathf.Max(0, Money + value);
                    break;
                default:
                    break;
            }
        }

        public void SetPriority(EnumPriority priorityType, float value)
        {
            switch (priorityType)
            {
                case EnumPriority.Power:
                    Power = Mathf.Clamp(value, 0, MaxPower);
                    break;
                case EnumPriority.Energy:
                    Energy = Mathf.Clamp(value, 0, MaxEnergy);
                    break;
                case EnumPriority.Hygiene:
                    Hygiene = Mathf.Clamp(value, 0, MaxHygiene);
                    break;
                case EnumPriority.Health:
                    Health = Mathf.Clamp(value, 0, MaxHealth);
                    break;
                case EnumPriority.HP:
                    HP = Mathf.Clamp(value, 0, MaxHP);
                    break;
                case EnumPriority.EXP:
                    EXP = Mathf.Clamp(value, 0, MaxEXP);
                    break;
                case EnumPriority.Money:
                    Money = Mathf.Max(0, value);
                    break;
                default:
                    break;
            }
        }

        public void SetLevel(int level)
        {
            Level = Mathf.Clamp(level, 0, MaxLevel);
        }

        public void Upgrade()
        {
            if (!IsMaxLevel)
            {
                Level++;
            }
            else
            {
                Log.Error("Player (serialId:'{0}') has reached the highest level", SerialId);
            }
        }

        public static Player Create(PlayerData playerData, int serialId, int level = 1, int weapon = 0, string Name = "")
        {
            Player player = ReferencePool.Acquire<Player>();
            player.playerData = playerData;
            player.SerialId = serialId;
            player.Level = level;
            player.WeaponId = weapon;
            player.Name = Name;
            player.HP = player.MaxHP;
            player.Power = player.MaxPower;
            player.Energy = player.MaxEnergy;
            player.Hygiene = player.MaxHygiene;
            player.Health = player.MaxHealth;
            player.EXP = 0;
            player.Money = 0;
            return player;
        }

        public void Clear()
        {
            playerData = null;
            SerialId = 0;
            Level = 1;
            Name = "";
            Level = 0;
            HP = 0;
            Power = 0;
            Energy = 0;
            Hygiene = 0;
            Health = 0;
            EXP = 0;
            Money = 0;
        }
    }
}
