﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-24 15:26:59.549
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
    /// 任务配置表。
    /// </summary>
    public class DRTask : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取任务ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取任务类型(1:主线，2:随机)。
        /// </summary>
        public int TaskType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取任务描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取任务类型。
        /// </summary>
        public int SubTaskType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取任务参数。
        /// </summary>
        public string Parameter
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取任务完成条件(type=完成任务条件类型&任务目标=(数量或Id)&...)。
        /// </summary>
        public string TaskCondition
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
        /// 获取奖励。
        /// </summary>
        public string Reward
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下一任务ID。
        /// </summary>
        public int NextTaskId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发事件Id。
        /// </summary>
        public string EventId
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
            TaskType = int.Parse(columnStrings[index++]);
            Description = columnStrings[index++];
            SubTaskType = int.Parse(columnStrings[index++]);
            Parameter = columnStrings[index++];
            TaskCondition = columnStrings[index++];
            IsForce = bool.Parse(columnStrings[index++]);
            Reward = columnStrings[index++];
            NextTaskId = int.Parse(columnStrings[index++]);
            EventId = columnStrings[index++];

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
                    TaskType = binaryReader.Read7BitEncodedInt32();
                    Description = binaryReader.ReadString();
                    SubTaskType = binaryReader.Read7BitEncodedInt32();
                    Parameter = binaryReader.ReadString();
                    TaskCondition = binaryReader.ReadString();
                    IsForce = binaryReader.ReadBoolean();
                    Reward = binaryReader.ReadString();
                    NextTaskId = binaryReader.Read7BitEncodedInt32();
                    EventId = binaryReader.ReadString();
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
