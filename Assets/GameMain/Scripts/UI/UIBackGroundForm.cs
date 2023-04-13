using DG.Tweening;
using StarForce.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UIBackGroundForm : UGuiForm
    {
        [SerializeField] private Transform btnEnter;
        [SerializeField] private Text intro;

        private DataDialog dataDialog;

        private float SPEED = 10F;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataDialog = GameEntry.Data.GetData<DataDialog>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            ShowIntro();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            if (DOTween.IsTweening("show_intro"))
            {
                DOTween.Kill("show_intro", true);
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

        private void ShowIntro()
        {
            intro.GetComponent<CanvasGroup>().DOFade(1, 1);
            intro.text = string.Empty;

            DialogData data = dataDialog.GetDialogDataById(10001);
            if (data == null) return;

            intro.DOText(data.DialogContent, data.DialogContent.Length / SPEED).SetId("show_intro").
                OnComplete(() =>
                {
                    btnEnter.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(() => {
                        btnEnter.GetComponent<CanvasGroup>().interactable = true;
                    });
                });
        }

        public void OnBtnNextClick()
        {
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt(Constant.Config.Character)));
        }

        public void OnBtnSkipClick()
        {
            if (DOTween.IsTweening("show_intro"))
            {
                DOTween.Kill("show_intro", true);
            }
        }
    }
}
