using StarForce.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;

namespace StarForce
{
    public class UICharacterForm : UGuiForm
    {
        [SerializeField]
        private CharacterSpriteList_SO characterSpriteList_SO;

        [SerializeField]
        private GameObject characterTemplate;

        [SerializeField]
        private Transform content;

        [SerializeField]
        private InputField characterNameInput;

        private int curId = 0;
        private GameObject curCharacter = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            CharacterInit();
        }

        private void CharacterInit()
        {
            CharacterData[] characterDatas = GameEntry.Data.GetData<DataCharacter>().GetAllCharacterData();
            if (characterDatas == null)
            {
                Log.Error("Can not load character data from data table.");
                return;
            }

            foreach (var characterData in characterDatas)
            {
                CharacterDetail characterDetail = characterSpriteList_SO.GetCharacterDetail(characterData.Id);
                if (characterDetail == null) continue;

                GameObject characterGO = Instantiate(characterTemplate, content);
                characterGO.transform.SetParent(content.transform);
                characterGO.name = "Character_" + characterData.Id;

                if (curId == 0 || curCharacter == null)
                {
                    curId = characterData.Id;
                    curCharacter = characterGO;
                    curCharacter.transform.GetChild(0).gameObject.SetActive(true);
                }

                Image characterImage = characterGO.transform.GetChild(1).GetComponent<Image>();
                characterImage.sprite = characterDetail.characterSprite;

                Button btnCharacter = characterGO.GetComponent<Button>();
                btnCharacter.onClick.AddListener(() => {
                    this.OnCharacterClick(characterGO);
                });
            }
        }

        public void OnCharacterClick(GameObject sender)
        {
            int id = int.Parse(sender.name.Split('_')[1]);
            if (id == curId) return;

            sender.transform.GetChild(0).gameObject.SetActive(true);
            if (curId == 0 || curCharacter == null)
            {
                curCharacter = sender;
                curId = id;
                return;
            }
            curCharacter.transform.GetChild(0).gameObject.SetActive(false);
            curCharacter = sender;
            curId = id; 
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public void OnBtnConfirmClick()
        {
            string characterName = characterNameInput.text;
            if (characterName == string.Empty)
            {
                GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(Constant.Localization.TipsCharacterTitle),
                    Message = GameEntry.Localization.GetString(Constant.Localization.TipsNameNullMessage),
                    UserData = null
                });
                return;
            }

            GameEntry.Setting.SetInt(Constant.DataNode.CharacterId, curId);
            GameEntry.Setting.SetString(Constant.DataNode.PlayerName, characterName);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt(Constant.Config.MainGame)));
        }
    }
}
