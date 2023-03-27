using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce.Data
{
    public class PlayerData
    {
        private DRPlayer dRPlayer;
        private PlayerLevelData[] playerLevels;

        public int Id
        {
            get { return dRPlayer.Id; }
        }

        public int EntityId
        {
            get { return dRPlayer.EntityId; } 
        }

        public int Level
        {
            get { return dRPlayer.PlayerLevel; }
        }

        public int WeaponId
        {
            get { return dRPlayer.WeaponId; }
        }

        public PlayerData(DRPlayer dRPlayer, PlayerLevelData[] playerLevels)
        {
            this.dRPlayer = dRPlayer;
            this.playerLevels = playerLevels;
        }

        public PlayerLevelData GetPlayerLevelData(int level)
        {
            if (playerLevels == null || level > GetMaxLevel())
                return null;

            return playerLevels[level - 1];
        }

        public int GetMaxLevel()
        {
            return playerLevels == null ? 1 : playerLevels.Length;
        }
    }
}
