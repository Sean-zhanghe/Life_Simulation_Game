using StarForce.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UIMapForm : UGuiForm
    {
        [SerializeField] private RenderTexture[] mapsRender;
        [SerializeField] private RawImage mapRaw;

        private Dictionary<string, RenderTexture> dicMap;

        private DataScene dataScene;
        private DataGame dataGame;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataScene = GameEntry.Data.GetData<DataScene>();
            dataGame = GameEntry.Data.GetData<DataGame>();

            dicMap = new Dictionary<string, RenderTexture>()
            {
                { EnumScene.MainGame.ToString(), mapsRender[0] },
                { EnumScene.Dormitory.ToString(), mapsRender[1] },
                { EnumScene.Level_1.ToString(), mapsRender[2] },
                { EnumScene.Level_2.ToString(), mapsRender[3] },
                { EnumScene.Level_3.ToString(), mapsRender[4] }
            };
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            if (mapsRender.Length == 0) return;

            if (!dicMap.ContainsKey(dataScene.scene)) return;

            RenderTexture render = dicMap[dataScene.scene];
            mapRaw.texture = render;
            mapRaw.rectTransform.sizeDelta = new Vector2(render.width, render.height);

            dataGame.GamePause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            dataGame.GameResume();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
