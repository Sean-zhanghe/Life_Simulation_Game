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
    /// 游戏正常
    /// </summary>
    GameNormal = 3,

    /// <summary>
    /// 游戏结束
    /// </summary>
    Gameover = 4,

    /// <summary>
    /// 现实结束
    /// </summary>
    RealityOver = 5
}

public enum EnumGameOverType : byte
{
    Success,
    Fail
}
