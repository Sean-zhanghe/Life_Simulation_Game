using DG.Tweening;
using StarForce.Data;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UIDialogueForm : UGuiForm
    {
        [SerializeField] private Text NPCName;
        [SerializeField] private Image icon;
        [SerializeField] private Transform content;
        [SerializeField] private Scrollbar vertical;
        [SerializeField] private Sprite next;
        [SerializeField] private Sprite complete;
        [SerializeField] private Image button;
        [SerializeField] private GameObject dialogItemTemplate;
        [SerializeField] private NPCSpriteList_SO npcSpriteList_SO;

        private DataGame dataGame;
        private DataDialog dataDialog;

        private int oldDialog;
        private int currentDialog;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataGame = GameEntry.Data.GetData<DataGame>();
            dataDialog = GameEntry.Data.GetData<DataDialog>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
            OpenDialogEventArgs args = (OpenDialogEventArgs) userData;
            if (args == null)
                Log.Error("UI Dialog can't get data");

            NPCName.text = args.Name;
            NPCDetail detail = npcSpriteList_SO.GetNPCDetail(args.IconId);
            icon.sprite = detail.NPCIcon;

            currentDialog = args.DialogId;
            DialogData dialog = dataDialog.GetDialogDataById(currentDialog);
            string dialogContent = dialog.DialogContent;
            CreateDialogItem(dialogContent);

            oldDialog = currentDialog;
            currentDialog = dialog.NextDialog;
            if (currentDialog != 0)
            {
                button.sprite = next;
            }
            else
            {
                button.sprite = complete;
            }

            // 游戏转为暂停状态
            dataGame.GamePause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Fire(this, DialogFinishEventArgs.Create(oldDialog));
            // 游戏转为正常状态
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

        private void CreateDialogItem(string dialogContent)
        {
            GameObject dialogItem = Instantiate(dialogItemTemplate, content);
            dialogItem.GetComponent<Text>().text = dialogContent;
            //dialogItem.transform.SetParent(content);

            //vertical.value = 0;
            float timer = 0;
            DOTween.To(() => timer, x => timer = x, 1, 0.1f)
                      .OnStepComplete(() =>
                      {
                          vertical.value = 0;
                      });
        }

        private void ClearContent()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

        public void OnBtnNextClick()
        {
            if (currentDialog == 0)
            {
                ClearContent();
                this.Close();
                return;
            }

            DialogData dialog = dataDialog.GetDialogDataById(currentDialog);
            if (dialog.NextDialog == 0)
            {
                //buttonText.text = GameEntry.Localization.GetString(Constant.Localization.UIDialogFinish);
                button.sprite = complete;
            }

            string content = dialog.DialogContent;
            if (dialog.Parameter != "")
            {
                string[] parameters = dialog.Parameter.Split('&');
                for (int i = 0; i < parameters.Length; i++)
                {
                    string parameter = "";
                    string type = parameters[i].Split('=')[0];
                    string value = parameters[i].Split('=')[1];
                    switch (type)
                    {
                        case Constant.Parameter.Setting:
                            parameter = GameEntry.Setting.GetString(value);
                            content = string.Format(content, parameter);
                            break;
                        default:
                            break;
                    }
                }
            }

            CreateDialogItem(content);

            oldDialog = currentDialog;
            currentDialog = dialog.NextDialog;
        }

    }
}
