using StarForce.Data;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UILevelMenuForm : UGuiForm
    {
        [SerializeField]
        private GameObject levelTemplate;

        [SerializeField]
        private Transform content;

        private DataLevel dataLevel;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataLevel = GameEntry.Data.GetData<DataLevel>();

            LevelInit();
        }

        private void LevelInit()
        {
            LevelData[] levelDatas = dataLevel.GetAllLevelData();
            if (levelDatas == null)
            {
                Log.Error("Can not load level data from data table.");
                return;
            }

            foreach (var levelData in levelDatas)
            {
                GameObject levelGO = Instantiate(levelTemplate, content);
                levelGO.transform.SetParent(content.transform);
                levelGO.name = "Level_" + levelData.Id;

                levelGO.transform.GetChild(0).gameObject.SetActive(levelData.Id <= dataLevel.MaxLevelId);
                Text Id = levelGO.transform.GetChild(0).GetComponent<Text>();
                Id.text = levelData.Id.ToString();

                levelGO.transform.GetChild(1).gameObject.SetActive(levelData.Id > dataLevel.MaxLevelId);
                
                Text name = levelGO.transform.GetChild(2).GetComponent<Text>();
                name.text = levelData.Name;

                Button btnLevel = levelGO.GetComponent<Button>();
                btnLevel.onClick.AddListener(() => {
                    this.OnBtnLevelClick(levelGO);
                });
            }
        }

        private void OnBtnLevelClick(GameObject sender)
        {
            int id = int.Parse(sender.name.Split('_')[1]);
            if (id > dataLevel.MaxLevelId)
            {
                return;
            }

            dataLevel.LoadLevel(id);
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

        public void OnBtnBackClick()
        {
            GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create((int)EnumScene.MainGame));
        }
    }
}
