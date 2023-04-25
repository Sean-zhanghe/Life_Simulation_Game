//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-24 15:26:59.745
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
    /// 招聘配置表。
    /// </summary>
    public class DRRecruit : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取招聘工作Id。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取工作名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取薪资。
        /// </summary>
        public int Pay
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取支付方式。
        /// </summary>
        public string PayMode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取工作描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取公司地址。
        /// </summary>
        public string Address
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取招聘条件(l:小于 el:小于等于 e:等于 eg:大于等于 g:大于)。
        /// </summary>
        public string Condition
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取工作类型（1：游戏闯关，2：实体）。
        /// </summary>
        public int WorkType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取申请。
        /// </summary>
        public string Apply
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取完成。
        /// </summary>
        public string Finish
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取奖励。
        /// </summary>
        public string Reward
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
            Pay = int.Parse(columnStrings[index++]);
            PayMode = columnStrings[index++];
            Description = columnStrings[index++];
            Address = columnStrings[index++];
            Condition = columnStrings[index++];
            WorkType = int.Parse(columnStrings[index++]);
            Apply = columnStrings[index++];
            Finish = columnStrings[index++];
            Reward = columnStrings[index++];

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
                    Pay = binaryReader.Read7BitEncodedInt32();
                    PayMode = binaryReader.ReadString();
                    Description = binaryReader.ReadString();
                    Address = binaryReader.ReadString();
                    Condition = binaryReader.ReadString();
                    WorkType = binaryReader.Read7BitEncodedInt32();
                    Apply = binaryReader.ReadString();
                    Finish = binaryReader.ReadString();
                    Reward = binaryReader.ReadString();
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
