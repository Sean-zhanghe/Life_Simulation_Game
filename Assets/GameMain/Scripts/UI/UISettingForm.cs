//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Localization;
using StarForce.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UISettingForm : UGuiForm
    {
        [SerializeField]
        private Slider m_MusicVolumeSlider = null;

        [SerializeField]
        private Slider m_SFXVolumeSlider = null;

        [SerializeField]
        private Slider m_UISoundVolumeSlider = null;

        private DataGame dataGame;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataGame = GameEntry.Data.GetData<DataGame>();

            m_MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);
            m_UISoundVolumeSlider.onValueChanged.AddListener(OnUISoundVolumeSliderChange);
            m_SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChange);
        }


#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_MusicVolumeSlider.value = GameEntry.Sound.GetVolume("Music");
            m_UISoundVolumeSlider.value = GameEntry.Sound.GetVolume("SFX/UI");
            m_SFXVolumeSlider.value = GameEntry.Sound.GetVolume("SFX");

            // 游戏转为暂停状态
            dataGame.GamePause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            // 游戏转为正常状态
            dataGame.GameResume();
        }

        private void OnMusicVolumeSliderChange(float value)
        {
            GameEntry.Sound.SetVolume("Music", value);
        }

        private void OnUISoundVolumeSliderChange(float value)
        {
            GameEntry.Sound.SetVolume("SFX/UI", value);
        }

        private void OnSFXVolumeSliderChange(float value)
        {
            GameEntry.Sound.SetVolume("SFX", value);
        }

        public void OnBtnExit()
        {
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }
    }
}
