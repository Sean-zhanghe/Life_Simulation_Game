using DG.Tweening;
using GameFramework.Event;
using StarForce.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UILevelForm : UGuiForm
    {
        public Slider HPBar;
        public Image Portrait;
        public CharacterSpriteList_SO characterSpriteList_SO;

        private DataPlayer dataPlayer;
        private DataLevel dataLevel;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataLevel = GameEntry.Data.GetData<DataLevel>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            RefreshHP();
            RefreshPortrait();

            GameEntry.Event.Subscribe(PlayerPriorityChangeEventArgs.EventId, OnPriorityChange);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(PlayerPriorityChangeEventArgs.EventId, OnPriorityChange);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        private void OnPriorityChange(object sender, GameEventArgs e)
        {
            PlayerPriorityChangeEventArgs ne = (PlayerPriorityChangeEventArgs)e;
            if (ne == null)
            {
                return;
            }

            if (ne.PriorityType == EnumPriority.HP)
            {
                RefreshHP();
            }
        }

        private void RefreshHP()
        {
            HPBar.value = dataPlayer.player.HP / dataPlayer.player.MaxHP;
        }

        private void RefreshPortrait()
        {
            CharacterDetail detail = characterSpriteList_SO.GetCharacterDetail(dataPlayer.player.CharacterId);
            Portrait.sprite = detail.characterIcon;
        }

        public void OnBtnBackClick()
        {
            GameEntry.UI.OpenTips(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString(Constant.Localization.LevelTitle),
                Message = GameEntry.Localization.GetString(Constant.Localization.LevelQuitLevelMessage),
                OnClickConfirm = (object userdatas) => 
                {
                    dataLevel.ExitLevel();
                },
                UserData = null
            });
        }

        public void OnBtnMapClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.UIMapForm);
        }
    }
}
