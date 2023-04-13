//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-13 17:37:11.534
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// 敌人配置表。
    /// </summary>
    public class DREnemy : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取敌人Id。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取敌人名字。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int EntityId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取敌人类型。
        /// </summary>
        public string EnemyType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取血量。
        /// </summary>
        public float MaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取伤害。
        /// </summary>
        public float Damage
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取速度。
        /// </summary>
        public float Speed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取追击速度。
        /// </summary>
        public float Chase
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取停止时间。
        /// </summary>
        public float IdleTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取巡逻范围。
        /// </summary>
        public float PatrolRadius
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取追击范围。
        /// </summary>
        public float ChaseRadius
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击频率。
        /// </summary>
        public float AttackRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取掉落物品。
        /// </summary>
        public string Drop
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Name = columnStrings[index++];
            EntityId = int.Parse(columnStrings[index++]);
            EnemyType = columnStrings[index++];
            MaxHP = float.Parse(columnStrings[index++]);
            Damage = float.Parse(columnStrings[index++]);
            Speed = float.Parse(columnStrings[index++]);
            Chase = float.Parse(columnStrings[index++]);
            IdleTime = float.Parse(columnStrings[index++]);
            PatrolRadius = float.Parse(columnStrings[index++]);
            ChaseRadius = float.Parse(columnStrings[index++]);
            AttackRate = float.Parse(columnStrings[index++]);
            Drop = columnStrings[index++];

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    EntityId = binaryReader.Read7BitEncodedInt32();
                    EnemyType = binaryReader.ReadString();
                    MaxHP = binaryReader.ReadSingle();
                    Damage = binaryReader.ReadSingle();
                    Speed = binaryReader.ReadSingle();
                    Chase = binaryReader.ReadSingle();
                    IdleTime = binaryReader.ReadSingle();
                    PatrolRadius = binaryReader.ReadSingle();
                    ChaseRadius = binaryReader.ReadSingle();
                    AttackRate = binaryReader.ReadSingle();
                    Drop = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
