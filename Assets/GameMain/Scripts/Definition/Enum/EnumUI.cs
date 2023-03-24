﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace StarForce
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : int
    {
        Undefined = 0,

        /// <summary>
        /// 主菜单。
        /// </summary>
        UIMenuForm = 1000,

        /// <summary>
        /// 人物选择
        /// </summary>
        UICharacterForm = 1001,

        /// <summary>
        /// 主场景
        /// </summary>
        UIMainForm = 1002,

        /// <summary>
        /// 设置
        /// </summary>
        UISettingForm = 1003,

        /// <summary>
        /// 设置
        /// </summary>
        UIDialogForm = 1004,

        /// <summary>
        /// 等待
        /// </summary>
        UIWaitingForm = 1005,

        /// <summary>
        /// 弹出框。
        /// </summary>
        UITipsForm = 2001,
    }
}