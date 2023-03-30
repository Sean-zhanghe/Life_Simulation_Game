//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-03-30 18:10:58.430
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
    /// 抛射物配置表。
    /// </summary>
    public class DRProjectile : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取抛射物Id。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取抛射物实体Id。
        /// </summary>
        public int ProjectileEntityId
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
        /// 获取特效实体Id。
        /// </summary>
        public int VFXEntityId
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
            ProjectileEntityId = int.Parse(columnStrings[index++]);
            Speed = float.Parse(columnStrings[index++]);
            VFXEntityId = int.Parse(columnStrings[index++]);
            Damage = float.Parse(columnStrings[index++]);

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
                    ProjectileEntityId = binaryReader.Read7BitEncodedInt32();
                    Speed = binaryReader.ReadSingle();
                    VFXEntityId = binaryReader.Read7BitEncodedInt32();
                    Damage = binaryReader.ReadSingle();
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
