//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace StarForce
{
    public static partial class Constant
    {
        /// <summary>
        /// 层。
        /// </summary>
        public static class Layer
        {
            public const string DefaultLayerName = "Default";
            public static readonly int DefaultLayerId = LayerMask.GetMask(DefaultLayerName);
            public static readonly int DefaultNameLayerId = LayerMask.NameToLayer(DefaultLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.GetMask(UILayerName);
            public static readonly int UINameLayerId = LayerMask.NameToLayer(UILayerName);

            public const string CursorLayerName = "Cursor";
            public static readonly int CursorLayerId = LayerMask.GetMask(CursorLayerName);
            public static readonly int CursorNameLayerId = LayerMask.NameToLayer(CursorLayerName);

            public const string PlayerLayerName = "Player";
            public static readonly int PlayerLayerId = LayerMask.GetMask(PlayerLayerName);
            public static readonly int PlayerNameLayerId = LayerMask.NameToLayer(PlayerLayerName);

            public const string BulletIgnoreLayerName = "BulletIgnore";
            public static readonly int BulletIgnoreLayerId = LayerMask.GetMask(BulletIgnoreLayerName);
            public static readonly int BulletIgnoreNameLayerId = LayerMask.NameToLayer(BulletIgnoreLayerName);

        }
    }
}
