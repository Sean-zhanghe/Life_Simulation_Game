using GameFramework.Event;
using StarForce.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UIClothesStoreForm : UGuiForm
    {
        [SerializeField] private Transform store;
        [SerializeField] private Text money;
        [SerializeField] private ClothesSpriteList_SO clothesSpriteList_SO;

        private DataGame dataGame;
        private DataPlayer dataPlayer;
        private DataClothes dataClothes;
        private DataBag dataBag;

        private int curSlotIndex;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataGame = GameEntry.Data.GetData<DataGame>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataClothes = GameEntry.Data.GetData<DataClothes>();
            dataBag = GameEntry.Data.GetData<DataBag>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(PlayerPriorityChangeEventArgs.EventId, OnRefreshPriority);

            curSlotIndex = -1;
            RefreshPriorityByType(EnumPriority.Money);
            RefreshCanteen();

            dataGame.GamePause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(PlayerPriorityChangeEventArgs.EventId, OnRefreshPriority);

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

        private void RefreshCanteen()
        {
            for (int i = 0; i < store.childCount; i++)
            {
                if (i >= dataClothes.listClothesStore.Count)
                {
                    store.GetChild(i).gameObject.SetActive(false);
                    continue;
                }
                store.GetChild(i).gameObject.SetActive(true);

                int id = dataClothes.listClothesStore[i];
                ClothesData data = dataClothes.GetClothesDataById(id);

                Transform slot = store.GetChild(i);
                Transform selectBox = slot.GetChild(0);
                Image item = slot.GetChild(1).GetComponent<Image>();
                Text name = slot.GetChild(2).GetComponent<Text>();
                Text price = slot.GetChild(3).GetComponent<Text>();

                selectBox.gameObject.SetActive(false);
                ClothesDetail detail = clothesSpriteList_SO.GetClothesDetail(data.IconId);
                item.sprite = detail.ClothesIcon;
                price.text = data.Sell.ToString();
                name.text = data.Name;
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
                case EnumPriority.Money:
                    money.text = player.Money.ToString();
                    break;
                default:
                    break;
            }
        }

        public void OnSlotClick(GameObject obj)
        {
            int index = int.Parse(obj.name.Split('_')[1]);
            if (0 <= curSlotIndex && curSlotIndex < dataClothes.listClothesStore.Count)
            {
                Transform slot = store.GetChild(curSlotIndex);
                slot?.Find("SelectBox")?.gameObject.SetActive(false);
            }

            curSlotIndex = index;
            obj.transform.Find("SelectBox")?.gameObject.SetActive(true);
        }

        public void OnBtnBuyClick()
        {
            if (curSlotIndex < 0 || curSlotIndex >= dataClothes.listClothesStore.Count)
            {
                GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(Constant.Localization.StoreTitle),
                    Message = GameEntry.Localization.GetString(Constant.Localization.StoreSelectMessage),
                    UserData = null
                });
                return;
            }

            int id = dataClothes.listClothesStore[curSlotIndex];
            ClothesData data = dataClothes.GetClothesDataById(id);
            if (dataPlayer.player.Money < data.Sell)
            {
                GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(Constant.Localization.StoreTitle),
                    Message = GameEntry.Localization.GetString(Constant.Localization.StoreBalanceMessage),
                    UserData = null
                });
                return;
            }
            
            if(dataBag.AddClothes(data.Id))
            {
                dataPlayer.AddPriority(Constant.Parameter.Money, -data.Sell);
            }
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

    }
}
