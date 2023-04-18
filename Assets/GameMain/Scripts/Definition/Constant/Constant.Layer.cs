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
            
            public const string TransparentFXLayerName = "TransparentFX";
            public static readonly int TransparentFXLayerId = LayerMask.GetMask(TransparentFXLayerName);
            public static readonly int TransparentFXNameLayerId = LayerMask.NameToLayer(TransparentFXLayerName);
           
            public const string IngoreRaycastLayerName = "Ingore Raycast";
            public static readonly int IngoreRaycastLayerId = LayerMask.GetMask(IngoreRaycastLayerName);
            public static readonly int IngoreRaycastNameLayerId = LayerMask.NameToLayer(IngoreRaycastLayerName);
           
            public const string WaterLayerName = "Water";
            public static readonly int WaterLayerId = LayerMask.GetMask(WaterLayerName);
            public static readonly int WaterNameLayerId = LayerMask.NameToLayer(WaterLayerName);

            public const string UILayerName = "UI";
            public static readonly int UILayerId = LayerMask.GetMask(UILayerName);
            public static readonly int UINameLayerId = LayerMask.NameToLayer(UILayerName);

            public const string CameraBorderLayerName = "CameraBorder";
            public static readonly int CameraBorderLayerId = LayerMask.GetMask(CameraBorderLayerName);
            public static readonly int CameraBorderNameLayerId = LayerMask.NameToLayer(CameraBorderLayerName);

            public const string CursorLayerName = "Cursor";
            public static readonly int CursorLayerId = LayerMask.GetMask(CursorLayerName);
            public static readonly int CursorNameLayerId = LayerMask.NameToLayer(CursorLayerName);

            public const string BulletLayerName = "Bullet";
            public static readonly int BulletLayerId = LayerMask.GetMask(BulletLayerName);
            public static readonly int BulletNameLayerId = LayerMask.NameToLayer(BulletLayerName);

            public const string PlayerLayerName = "Player";
            public static readonly int PlayerLayerId = LayerMask.GetMask(PlayerLayerName);
            public static readonly int PlayerNameLayerId = LayerMask.NameToLayer(PlayerLayerName);
            
            public const string EnemyLayerName = "Enemy";
            public static readonly int EnemyLayerId = LayerMask.GetMask(EnemyLayerName);
            public static readonly int EnemyNameLayerId = LayerMask.NameToLayer(EnemyLayerName);

            public const string BulletIgnoreLayerName = "BulletIgnore";
            public static readonly int BulletIgnoreLayerId = LayerMask.GetMask(BulletIgnoreLayerName);
            public static readonly int BulletIgnoreNameLayerId = LayerMask.NameToLayer(BulletIgnoreLayerName);
            
            public const string MapLayerName = "Map";
            public static readonly int MapLayerId = LayerMask.GetMask(MapLayerName);
            public static readonly int MapNameLayerId = LayerMask.NameToLayer(MapLayerName);

            public const string NPCLayerName = "NPC";
            public static readonly int NPCLayerId = LayerMask.GetMask(NPCLayerName);
            public static readonly int NPCNameLayerId = LayerMask.NameToLayer(NPCLayerName);

        }
    }
}
