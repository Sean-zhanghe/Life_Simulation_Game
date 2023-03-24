using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public enum EnumTaskCondition : byte
    {
        None = 0,

        /// <summary>
        /// 对话类型
        /// </summary>
        Dialog = 1,

        /// <summary>
        /// 工作类型
        /// </summary>
        Work = 2,

        /// <summary>
        /// 对战类型
        /// </summary>
        Battle = 3,

    }
}
