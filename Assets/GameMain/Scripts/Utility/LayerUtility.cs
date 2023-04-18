using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    public class LayerUtility : MonoBehaviour
    {
        [SerializeField] EnumLayer layer;

        private Dictionary<EnumLayer, int> dicLayers;

        private void Awake()
        {
            dicLayers = new Dictionary<EnumLayer, int>()
            {
                {EnumLayer.Default, Constant.Layer.DefaultNameLayerId},
                {EnumLayer.TransparentFX, Constant.Layer.TransparentFXNameLayerId},
                {EnumLayer.IngnoreRaycast, Constant.Layer.IngoreRaycastNameLayerId},
                {EnumLayer.Water, Constant.Layer.WaterNameLayerId},
                {EnumLayer.UI, Constant.Layer.UINameLayerId},
                {EnumLayer.CameraBorder, Constant.Layer.CameraBorderNameLayerId},
                {EnumLayer.Cursor, Constant.Layer.CursorNameLayerId},
                {EnumLayer.Bullet, Constant.Layer.BulletNameLayerId},
                {EnumLayer.Player, Constant.Layer.PlayerNameLayerId},
                {EnumLayer.Enemy, Constant.Layer.EnemyNameLayerId},
                {EnumLayer.BulletIngore, Constant.Layer.BulletIgnoreNameLayerId},
                {EnumLayer.Map, Constant.Layer.MapNameLayerId},
                {EnumLayer.NPC, Constant.Layer.NPCNameLayerId},
            };

        }

        private void OnEnable()
        {
            if (!dicLayers.ContainsKey(layer)) return;

            gameObject.layer = dicLayers[layer];
        }
    }
}
