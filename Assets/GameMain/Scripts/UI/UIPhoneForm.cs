using DG.Tweening;
using StarForce.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UIPhoneForm : UGuiForm
    {
        [SerializeField] private Transform recruit;
        [SerializeField] private Transform recruitContent;

        [SerializeField] private GameObject recruitTemplete;

        private DataRecruit dataRecruit;

        private Dictionary<int, GameObject> dicRecruitContent;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            InitRecruitList();
        }

        private void InitRecruitList()
        {
            dataRecruit = GameEntry.Data.GetData<DataRecruit>();
            dicRecruitContent = new Dictionary<int, GameObject>();
            Recruit[] recruits = dataRecruit.GetAllRecruit();

            for (int i = 0; i < recruits.Length; i++)
            {
                Recruit recruit = recruits[i];
                GameObject item = Instantiate(recruitTemplete, recruitContent);
                item.name = "Recruit_" + recruit.Id.ToString();

                Text name = item.transform.GetChild(0).GetComponent<Text>();
                name.text = recruit.Name;

                Text pay = item.transform.GetChild(1).GetComponent<Text>();
                pay.text = recruit.Pay.ToString() + "/" + recruit.PayMode;

                Text description = item.transform.GetChild(2).GetComponent<Text>();
                description.text = recruit.Description;

                Text address = item.transform.GetChild(3).GetComponent<Text>();
                address.text = recruit.Address;

                Button btnApply = item.transform.GetChild(4).GetComponent<Button>();
                btnApply.onClick.AddListener(() => {
                    this.OnRecruitApplyClick(item);
                });

                dicRecruitContent.Add(recruit.Id, item);
                RefreshRecruit(recruit.Id);
            }
        }

        private void RefreshRecruit(int id)
        {
            if (!dicRecruitContent.ContainsKey(id)) return;

            GameObject item = dicRecruitContent[id];
            Recruit recruit = dataRecruit.GetRecruit(id);

            if (recruit == null) return;

            Text applyText = item.transform.GetChild(4).GetComponentInChildren<Text>();
            switch(recruit.state)
            {
                case EnumWorkState.Apply:
                    applyText.text = "申请";
                    break;
                case EnumWorkState.Working:
                    applyText.text = "已申请";
                    break;
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            ResetApp();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            if (DOTween.IsTweening("app_switch"))
            {
                DOTween.Kill("app_switch", true);
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        private void ResetApp()
        {
            recruit.gameObject.SetActive(false);
            recruit.localScale = Vector3.zero;
        }

        private void AppOnOff(Transform app, bool isShow)
        {
            if (isShow)
            {
                app.gameObject.SetActive(isShow);
            }
            Vector3 endValue = isShow ? Vector3.one : Vector3.zero;
            app.DOScale(endValue, 0.5f).SetId("app_switch").OnComplete(() => { 
                if (!isShow)
                    app.gameObject.SetActive(false);
            });
        }

        public void OnWorkAppClick()
        {
            AppOnOff(recruit, true);

            foreach (var key in dicRecruitContent.Keys)
            {
                RefreshRecruit(key);
            }
        }

        public void CloseApp(Transform app)
        {
            AppOnOff(app, false);
        }

        public void OnRecruitApplyClick(GameObject sender)
        {
            int recruitId = int.Parse(sender.name.Split('_')[1]);
            bool isPass = dataRecruit.CheckRecruitCondition(recruitId);
            if (!isPass)
            {
                GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(Constant.Localization.RecruitTitle),
                    Message = GameEntry.Localization.GetString(Constant.Localization.RecruitApplyFail),
                    UserData = null
                });
                return;
            }
            dataRecruit.ChangeRecruitState(recruitId, EnumWorkState.Working);
            RefreshRecruit(recruitId);
        }
    }
}
