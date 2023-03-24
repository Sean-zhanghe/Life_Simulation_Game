using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumGameState : byte
{
    None = 0,

    /// <summary>
    /// 正常
    /// </summary>
    Normal = 1,

    /// <summary>
    /// 暂停
    /// </summary>
    Pause = 2,

    /// <summary>
    /// 游戏结束
    /// </summary>
    Gameover = 3,

    /// <summary>
    /// 现实结束
    /// </summary>
    RealityOver = 4
}
