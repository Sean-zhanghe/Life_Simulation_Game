//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-15 10:19:55.672
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
    /// 事件配置表。
    /// </summary>
    public class DREvent : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取事件ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取事件类型(1: 对话 2:电话)。
        /// </summary>
        public int EventType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取参数。
        /// </summary>
        public string Parameter
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取完成条件。
        /// </summary>
        public string Condition
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取事件完成奖励。
        /// </summary>
        public string Reward
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否强制执行。
        /// </summary>
        public bool IsForce
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发任务。
        /// </summary>
        public string Trigger
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
            EventType = int.Parse(columnStrings[index++]);
            Parameter = columnStrings[index++];
            Condition = columnStrings[index++];
            Reward = columnStrings[index++];
            IsForce = bool.Parse(columnStrings[index++]);
            Trigger = columnStrings[index++];

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
                    EventType = binaryReader.Read7BitEncodedInt32();
                    Parameter = binaryReader.ReadString();
                    Condition = binaryReader.ReadString();
                    Reward = binaryReader.ReadString();
                    IsForce = binaryReader.ReadBoolean();
                    Trigger = binaryReader.ReadString();
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
