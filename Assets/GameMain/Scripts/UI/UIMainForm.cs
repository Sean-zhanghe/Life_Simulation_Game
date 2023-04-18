using StarForce.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;
using DG.Tweening;
using GameFramework.Event;
using GameFramework.Data;

namespace StarForce
{
    public class UIMainForm : UGuiForm
    {
        [SerializeField]
        private Sprite hideSprite;

        [SerializeField]
        private Sprite showSprite;

        [SerializeField] private Transform taskPanel;
        [SerializeField] private Text mainTaskText;
        [SerializeField] private Text randomTaskText;
        [SerializeField] private Image btnHide;
        [SerializeField] private Text powerText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text hygieneText;
        [SerializeField] private Text healthText;
        [SerializeField] private Text moneyText;
        [SerializeField] private Transform phone;

        private bool isShow;
        private bool isCall;

        private DataPlayer dataPlayer;
        private DataTask dataTask;

        private Data.Event m_Event;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataTask = GameEntry.Data.GetData<DataTask>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            RefreshPriority();
            RefreshTaskByType(EnumTaskType.MainTask);
            RefreshTaskByType(EnumTaskType.RandomTask);

            GameEntry.Event.Subscribe(PlayerPriorityChangeEventArgs.EventId, OnRefreshPriority);
            GameEntry.Event.Subscribe(ReleaseTaskEventArgs.EventId, OnRefreshTask);
            GameEntry.Event.Subscribe(ReleaseEventEventArgs.EventId, OnRelesaseEvent);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(PlayerPriorityChangeEventArgs.EventId, OnRefreshPriority);
            GameEntry.Event.Unsubscribe(ReleaseTaskEventArgs.EventId, OnRefreshTask);
            GameEntry.Event.Unsubscribe(ReleaseEventEventArgs.EventId, OnRelesaseEvent);

            if (DOTween.IsTweening("task_move"))
                DOTween.Kill("task_move", true);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        private void RefreshPriority()
        {
            EnumPriority[] priorities = new EnumPriority[] { 
                EnumPriority.Power, EnumPriority.Energy, EnumPriority.Health, 
                EnumPriority.Money, EnumPriority.Hygiene
            };

            foreach (var priority in priorities)
            {
                RefreshPriorityByType(priority);
            }
        }

        private void RefreshPriorityByType(EnumPriority priority)
        {
            Player player = dataPlayer.player;

            if (player == null)
            {
                Log.Error("Can't get DataPlayer");
                return;
            }

            switch (priority)
            {
                case EnumPriority.Power:
                    powerText.text = player.Power.ToString();
                    break;
                case EnumPriority.Energy:
                    energyText.text = player.Energy.ToString();
                    break;
                case EnumPriority.Hygiene:
                    hygieneText.text = player.Hygiene.ToString();
                    break;
                case EnumPriority.Health:
                    healthText.text = player.Health.ToString();
                    break;
                case EnumPriority.Money:
                    moneyText.text = player.Money.ToString();
                    break;
                default:
                    break;
            }
        }

        public void RefreshTaskByType(EnumTaskType taskType)
        {
            if (taskType == EnumTaskType.MainTask)
            {
                Task currentMainTask = dataTask.CurrentMainTask;
                string mainContent = GameEntry.Localization.GetString(Constant.Localization.TaskNoTask);
                if (currentMainTask != null)
                {
                    mainContent = currentMainTask.Description;
                }
                mainTaskText.text = mainContent;

                isShow = false;
                this.OnBtnHideClick();
            }

            if (taskType == EnumTaskType.RandomTask)
            {
                Task currentRandomTask = dataTask.CurrentRandomTask;
                string randomContent = GameEntry.Localization.GetString(Constant.Localization.TaskNoTask);
                if (currentRandomTask != null)
                {
                    randomContent = currentRandomTask.Description;
                }
                randomTaskText.text = randomContent;

                isShow = false;
                this.OnBtnHideClick();
            }

        }

        public void OnBtnBagClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.UIBagForm);
        }

        public void OnBtnAchievementClick()
        {
            GameEntry.UI.OpenTips(new DialogParams()
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString(Constant.Localization.TipsFunctionTitle),
                Message = GameEntry.Localization.GetString(Constant.Localization.TipsFunctionForbidden),
                UserData = null
            });
        }

        public void OnBtnPhoneClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.UIPhoneForm);
        }

        public void OnBtnMapClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.UIMapForm);
        }

        public void OnBtnSettingClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.UISettingForm);
        }

        public void OnBtnHideClick()
        {
            isShow = !isShow;
            if (isShow)
            {
                btnHide.sprite = showSprite;
            }
            else
            {
                btnHide.sprite = hideSprite;
            }

            float x = isShow ? 0 : -283;
            taskPanel.DOMoveX(x, 0.5f).SetId("task_move");
        }

        public void OnBtnPhoneCallClick()
        {
            if (!isCall) return;

            isCall = false;

            DataNPC dataNPC = GameEntry.Data.GetData<DataNPC>();
            DataEvent dataEvent = GameEntry.Data.GetData<DataEvent>();

            int dialogId = int.Parse(dataEvent.GetEventConditionValue(m_Event.Parameter, Constant.Parameter.Dialog));
            int npcId = int.Parse(dataEvent.GetEventConditionValue(m_Event.Parameter, Constant.Parameter.NPC)); ;

            NPCData npc = dataNPC.GetNPCDataById(npcId);

            GameEntry.Event.Fire(this, OpenDialogEventArgs.Create(npc.Name, npc.IconId, dialogId));
            
            m_Event = null;
            phone.gameObject.SetActive(false);
        }

        private void OnRefreshPriority(object sender, GameEventArgs e)
        {
            PlayerPriorityChangeEventArgs ne = (PlayerPriorityChangeEventArgs)e;
            if (ne == null)
            {
                return;
            }

            RefreshPriorityByType(ne.PriorityType);
        }

        private void OnRefreshTask(object sender, GameEventArgs e)
        {
            ReleaseTaskEventArgs ne = (ReleaseTaskEventArgs)e;
            if (ne == null)
            {
                return;
            }

            RefreshTaskByType(ne.TaskType);
        }

        private void OnRelesaseEvent(object sender, GameEventArgs e)
        {
            ReleaseEventEventArgs ne = (ReleaseEventEventArgs)e;
            if (ne == null)
            {
                return;
            }
            m_Event = ne.m_Event;

            if (m_Event.EventType == (int)EnumEventType.Phone)
            {
                isCall = true;
                phone.gameObject.SetActive(true);
            }
        }
    }
}
