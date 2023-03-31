using GameFramework;
using StarForce.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UILevelOverForm : UGuiForm
    {
        public Text title;
        public CommonButton btnNext;

        private DataGame dataGame;
        private DataLevel dataLevel;

        private LevelData level;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataGame = GameEntry.Data.GetData<DataGame>();
            dataLevel = GameEntry.Data.GetData<DataLevel>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            UIGameOverFormOpenParam uIGameOverFormOpenParam = userData as UIGameOverFormOpenParam;
            if (uIGameOverFormOpenParam == null)
            {
                Log.Error("UIGameOverForm open param tyoe invaild");
                return;
            }

            level = uIGameOverFormOpenParam.LevelData;
            Debug.Log(uIGameOverFormOpenParam.EnumGameOverType);
            switch (uIGameOverFormOpenParam.EnumGameOverType)
            {
                case EnumGameOverType.Success:
                    Debug.Log("success 11111111111");
                    title.text = string.Format(GameEntry.Localization.GetString(Constant.Localization.LevelComplete), level.Name);
                    //GameEntry.Sound.PlaySound(EnumSound.TDVictory);
                    break;
                case EnumGameOverType.Fail:
                    Debug.Log("fail 2222222222222");
                    title.text = string.Format(GameEntry.Localization.GetString(Constant.Localization.LevelFailed), level.Name);
                    //GameEntry.Sound.PlaySound(EnumSound.TDDefeat);
                    break;
            }

            btnNext.gameObject.SetActive(uIGameOverFormOpenParam.EnumGameOverType == EnumGameOverType.Success);
            int nextLevel = dataLevel.CurLevelIndex + 1;
            btnNext.interactable = nextLevel < dataLevel.listLevelId.Count;

            ReferencePool.Release(uIGameOverFormOpenParam);

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

        public void OnBtnNextClick()
        {
            dataLevel.LoadNextLevel();
        }

        public void OnBtnMenuClick()
        {
            dataLevel.ExitLevel();
        }

        public void OnBtnRestarClick()
        {
            dataLevel.LoadLevel(dataLevel.CurLevelId);
            Close();
        }
    }
}
