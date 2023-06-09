﻿using GameFramework;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Rendering;
using UnityGameFramework.Runtime;

namespace StarForce.Data
{
    public class DataPlayer : DataBase
    {
        private IDataTable<DRPlayer> dtPlayer;
        private IDataTable<DRPlayerLevel> dtPlayerLevel;

        private Dictionary<int, PlayerData> dicPlayerData;
        private Dictionary<int, PlayerLevelData> dicPlayerLevelData;
        //private Dictionary<int, Player> dicPlayer;
        public Player player;

        private int serialId = 0;

        protected override void OnInit()
        {
            player = null;
        }

        protected override void OnLoad()
        {
            dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            if (dtPlayer == null)
                throw new System.Exception("Can not get data table Player");

            dtPlayerLevel = GameEntry.DataTable.GetDataTable<DRPlayerLevel>();
            if (dtPlayerLevel == null)
                throw new System.Exception("Can not get data table PlayerLevel");

            dicPlayerData = new Dictionary<int, PlayerData>();
            dicPlayerLevelData = new Dictionary<int, PlayerLevelData>();
            //dicPlayer = new Dictionary<int, Player>();

            DRPlayerLevel[] drPlayerLevels = dtPlayerLevel.GetAllDataRows();
            foreach (var drPlayerLevel in drPlayerLevels)
            {
                if (dicPlayerLevelData.ContainsKey(drPlayerLevel.Id))
                {
                    throw new System.Exception(string.Format("Data tower level id '{0}' duplicate.", drPlayerLevel.Id));
                }

                PlayerLevelData playerLevelData = new PlayerLevelData(drPlayerLevel);
                dicPlayerLevelData.Add(drPlayerLevel.Id, playerLevelData);
            }

            DRPlayer[] drPlayers = dtPlayer.GetAllDataRows();
            foreach (var drPlayer in drPlayers)
            {
                PlayerLevelData[] playerLevelDatas = new PlayerLevelData[drPlayerLevels.Length];
                for (int i = 0; i < drPlayerLevels.Length; i++)
                {
                    if (!dicPlayerLevelData.ContainsKey(drPlayerLevels[i].Id))
                    {
                        throw new System.Exception(string.Format("Can not find tower level id '{0}' in DataTable TowerLevel.", drPlayerLevels[i].Id));
                    }

                    playerLevelDatas[i] = dicPlayerLevelData[drPlayerLevels[i].Id];
                }
                
                PlayerData playerData = new PlayerData(drPlayer, playerLevelDatas);
                dicPlayerData.Add(drPlayer.Id, playerData);
            }
        }
        private int GenerateSerialId()
        {
            return --serialId;
        }

        public PlayerData GetPlayerData(int id)
        {
            if (!dicPlayerData.ContainsKey(id))
            {
                Log.Error("Can not find player data id '{0}'.", id);
                return null;
            }

            return dicPlayerData[id];
        }

        public Player CreatePlayer(int playerId, string name = "")
        {
            if (player != null)
            {
                return player;
            }

            if (!dicPlayerData.ContainsKey(playerId))
            {
                Log.Error("Can not find player data id '{0}'.", playerId);
                return null;
            }

            int serialId = GenerateSerialId();
            PlayerData playerData = dicPlayerData[playerId];
            player = Player.Create(playerData, serialId, playerData.Level, playerData.WeaponId, name);
            //dicPlayer.Add(serialId, player);
            return player;
        }

        public void AddPriority(string priorityType, float value = 0)
        {
            //if (!dicPlayer.ContainsKey(serialId))
            //{
            //    Log.Error("Can not find player serialId '{0}'.", serialId);
            //    return;
            //}

            //Player player = dicPlayer[serialId];

            float lastPriority = 0;
            float currentPriority = 0;
            EnumPriority prioritye = EnumPriority.None;
            string content = string.Empty;
            switch (priorityType)
            {
                case Constant.Parameter.Power:
                    prioritye = EnumPriority.Power;
                    lastPriority = player.Power;
                    player.AddPriority(EnumPriority.Power, value);
                    currentPriority = player.Power;

                    content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupPower), value >= 0 ? "+" + value : value.ToString());
                    GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                    break;
                case Constant.Parameter.Energy:
                    prioritye = EnumPriority.Energy;
                    lastPriority = player.Energy;
                    player.AddPriority(EnumPriority.Energy, value);
                    currentPriority = player.Energy;

                    content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupHygiene), value >= 0 ? "+" + value : value.ToString());
                    GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                    break;
                case Constant.Parameter.Hygiene:
                    prioritye = EnumPriority.Hygiene;
                    lastPriority = player.Hygiene;
                    player.AddPriority(EnumPriority.Hygiene, value);
                    currentPriority = player.Hygiene;

                    content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupHygiene), value >= 0 ? "+" + value : value.ToString());
                    GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                    break;
                case Constant.Parameter.Health:
                    prioritye = EnumPriority.Health;
                    lastPriority = player.Health;
                    player.AddPriority(EnumPriority.Health, value);
                    currentPriority = player.Health;

                    content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupHealth), value >= 0 ? "+" + value : value.ToString());
                    GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                    break;
                case Constant.Parameter.HP:
                    prioritye = EnumPriority.HP;
                    lastPriority = player.HP;
                    player.AddPriority(EnumPriority.HP, value);
                    currentPriority = player.HP;
                    break;
                case Constant.Parameter.EXP:
                    prioritye = EnumPriority.EXP;
                    lastPriority = player.EXP;
                    player.AddPriority(EnumPriority.EXP, value);
                    currentPriority = player.EXP;
                    break;
                case Constant.Parameter.Money:
                    prioritye = EnumPriority.Money;
                    lastPriority = player.Money;
                    player.AddPriority(EnumPriority.Money, value);
                    currentPriority = player.Money;

                    content = string.Format(GameEntry.Localization.GetString(Constant.Localization.PopupMoney), value >= 0 ? "+" + value : value.ToString());
                    GameEntry.Event.Fire(this, ShowTipsEventArgs.Create(content));
                    break;
                default:
                    break;
            }

            GameEntry.Event.Fire(this, PlayerPriorityChangeEventArgs.Create(player.SerialId, prioritye, lastPriority, currentPriority));
        }

        public void SetPriority(string priorityType, float value = 0)
        {

            //if (!dicPlayer.ContainsKey(serialId))
            //{
            //    Log.Error("Can not find player serialId '{0}'.", serialId);
            //    return;
            //}

            //Player player = dicPlayer[serialId];

            float lastPriority = 0;
            float currentPriority = 0;
            EnumPriority prioritye = EnumPriority.None;
            switch (priorityType)
            {
                case Constant.Parameter.Power:
                    prioritye = EnumPriority.Power;
                    lastPriority = player.Power;
                    player.SetPriority(EnumPriority.Power, value);
                    currentPriority = player.Power;
                    break;
                case Constant.Parameter.Energy:
                    prioritye = EnumPriority.Energy;
                    lastPriority = player.Energy;
                    player.SetPriority(EnumPriority.Energy, value);
                    currentPriority = player.Energy;
                    break;
                case Constant.Parameter.Hygiene:
                    prioritye = EnumPriority.Hygiene;
                    lastPriority = player.Hygiene;
                    player.SetPriority(EnumPriority.Hygiene, value);
                    currentPriority = player.Hygiene;
                    break;
                case Constant.Parameter.Health:
                    prioritye = EnumPriority.Health;
                    lastPriority = player.Health;
                    player.SetPriority(EnumPriority.Health, value);
                    currentPriority = player.Health;
                    break;
                case Constant.Parameter.HP:
                    prioritye = EnumPriority.HP;
                    lastPriority = player.HP;
                    player.SetPriority(EnumPriority.HP, value);
                    currentPriority = player.HP;
                    break;
                case Constant.Parameter.EXP:
                    prioritye = EnumPriority.EXP;
                    lastPriority = player.EXP;
                    player.SetPriority(EnumPriority.EXP, value);
                    currentPriority = player.EXP;
                    break;
                default:
                    break;
            }

            GameEntry.Event.Fire(this, PlayerPriorityChangeEventArgs.Create(player.SerialId, prioritye, lastPriority, currentPriority));
        }

        public void AddItem(string key, int value)
        {
            DataBag dataBag = GameEntry.Data.GetData<DataBag>();
            switch (key)
            {
                case Constant.Parameter.Property:
                    dataBag.AddProperty(value);
                    break;
                case Constant.Parameter.Clothes:
                    dataBag.AddClothes(value);
                    break;
                case Constant.Parameter.Food:
                    break;
                case Constant.Parameter.Equipment:
                    break;
                case Constant.Parameter.Pet:
                    break;
                default:
                    break;
            }
        }

        public void SetLevel(int level)
        {
            player.SetLevel(level);
        }

        public void Reset()
        {
            SetPriority(Constant.Parameter.HP, player.MaxHP);
        }

        public void Upgrade()
        {
            player.Upgrade();
        }

        public void AddRewardByConfiger(string configer)
        {
            if (configer == string.Empty) return;

            string[] priorities = configer.Split('&');
            foreach (var priority in priorities)
            {
                string key = priority.Split('=')[0];
                int value = int.Parse(priority.Split('=')[1]);
                AddPriority(key, value);
                AddItem(key, value);
            }
        }

        public void Damage(float value)
        {
            if (value == 0) return;

            AddPriority(Constant.Parameter.HP, -value);

            if (player.HP <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            GameEntry.Data.GetData<DataLevel>().LevelGameFail();
        }

        protected override void OnUnload()
        {
            GameEntry.DataTable.DestroyDataTable<DRPlayer>();
            GameEntry.DataTable.DestroyDataTable<DRPlayerLevel>();

            dicPlayerData = null;
            dicPlayerLevelData = null;

            //if (dicPlayer != null)
            //{
            //    foreach (var item in dicPlayer.Values)
            //    {
            //        ReferencePool.Release(item);
            //    }
            //    dicPlayer.Clear();
            //}

            ReferencePool.Release(player);
            player = null;

            serialId = 0;
        }

        protected override void OnShutdown()
        {

        }
    }
}
