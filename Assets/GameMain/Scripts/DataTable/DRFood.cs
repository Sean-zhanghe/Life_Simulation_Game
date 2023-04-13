//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-13 17:37:11.556
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
    /// 食物配置表。
    /// </summary>
    public class DRFood : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取食物编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取名字。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取图标Id。
        /// </summary>
        public int IconId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取最大堆叠数。
        /// </summary>
        public int MaxStack
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取食物描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取属性。
        /// </summary>
        public string Attribute
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取售价。
        /// </summary>
        public int Sell
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
            IconId = int.Parse(columnStrings[index++]);
            MaxStack = int.Parse(columnStrings[index++]);
            Description = columnStrings[index++];
            Attribute = columnStrings[index++];
            Sell = int.Parse(columnStrings[index++]);

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
                    IconId = binaryReader.Read7BitEncodedInt32();
                    MaxStack = binaryReader.Read7BitEncodedInt32();
                    Description = binaryReader.ReadString();
                    Attribute = binaryReader.ReadString();
                    Sell = binaryReader.Read7BitEncodedInt32();
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
