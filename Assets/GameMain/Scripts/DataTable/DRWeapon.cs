//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-24 15:26:59.643
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
    /// 武器配置表。
    /// </summary>
    public class DRWeapon : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取武器Id。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取武器名。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器类型（1：近战 2：远程）。
        /// </summary>
        public int WeaponType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取抛射物Id。
        /// </summary>
        public int ProjectileId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取抛射物类型。
        /// </summary>
        public string ProjectileType
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
        /// 获取动画参数。
        /// </summary>
        public string Paramater
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
            WeaponType = int.Parse(columnStrings[index++]);
            ProjectileId = int.Parse(columnStrings[index++]);
            ProjectileType = columnStrings[index++];
            Damage = float.Parse(columnStrings[index++]);
            Paramater = columnStrings[index++];

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
                    WeaponType = binaryReader.Read7BitEncodedInt32();
                    ProjectileId = binaryReader.Read7BitEncodedInt32();
                    ProjectileType = binaryReader.ReadString();
                    Damage = binaryReader.ReadSingle();
                    Paramater = binaryReader.ReadString();
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
