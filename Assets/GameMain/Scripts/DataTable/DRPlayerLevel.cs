//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-15 10:19:55.491
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
    /// 玩家等级配置表。
    /// </summary>
    public class DRPlayerLevel : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取等级Id。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取体力。
        /// </summary>
        public float Power
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取精力。
        /// </summary>
        public float Energy
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卫生。
        /// </summary>
        public float Hygiene
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取健康。
        /// </summary>
        public float Health
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取攻击力。
        /// </summary>
        public float Attack
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取防御力。
        /// </summary>
        public float Defence
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大血量。
        /// </summary>
        public float MaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取经验。
        /// </summary>
        public float MaxEXP
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
            Power = float.Parse(columnStrings[index++]);
            Energy = float.Parse(columnStrings[index++]);
            Hygiene = float.Parse(columnStrings[index++]);
            Health = float.Parse(columnStrings[index++]);
            Attack = float.Parse(columnStrings[index++]);
            Defence = float.Parse(columnStrings[index++]);
            MaxHP = float.Parse(columnStrings[index++]);
            MaxEXP = float.Parse(columnStrings[index++]);

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
                    Power = binaryReader.ReadSingle();
                    Energy = binaryReader.ReadSingle();
                    Hygiene = binaryReader.ReadSingle();
                    Health = binaryReader.ReadSingle();
                    Attack = binaryReader.ReadSingle();
                    Defence = binaryReader.ReadSingle();
                    MaxHP = binaryReader.ReadSingle();
                    MaxEXP = binaryReader.ReadSingle();
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
