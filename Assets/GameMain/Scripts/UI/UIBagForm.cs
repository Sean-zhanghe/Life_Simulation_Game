using DG.Tweening;
using GameFramework.Event;
using StarForce.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UIBagForm : UGuiForm
    {

        [SerializeField] private Transform bag;

        [SerializeField] private Slider power;
        [SerializeField] private Text powerProgress;
        [SerializeField] private Slider energy;
        [SerializeField] private Text energyProgress;
        [SerializeField] private Slider hygiene;
        [SerializeField] private Text hygieneProgress;
        [SerializeField] private Slider health;
        [SerializeField] private Text healthProgress;

        [SerializeField] private Transform btnBagPanel;
        [SerializeField] private Transform btnUse;
        [SerializeField] private Image player;

        [SerializeField] private PropertySpriteList_SO propertySpriteList_SO;
        [SerializeField] private ClothesSpriteList_SO clothesSpriteList_SO;
        [SerializeField] private FoodSpriteList_SO foodSpriteList_SO;
        [SerializeField] private EquipmentSpriteList_SO equipmentSpriteList_SO;
        [SerializeField] private PetSpriteList_SO petSpriteList_SO;

        [SerializeField] private GameObject slotTemplete;

        public Transform Tips;

        public int curBagIndex { get; private set; }
        public int curSlotIndex { get; private set; }

        private DataPlayer dataPlayer;
        private DataBag dataBag;
        private DataGame dataGame;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataBag = GameEntry.Data.GetData<DataBag>();
            dataGame = GameEntry.Data.GetData<DataGame>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(PlayerPriorityChangeEventArgs.EventId, OnRefreshPriority);
            GameEntry.Event.Subscribe(ChangeClothesEventArgs.EventId, OnRefreshClothes);

            RefreshPriority();
            RefreshPlayerClothes();

            curBagIndex = 0;
            OnBtnBagClick(curBagIndex);

            curSlotIndex = -1;

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
            GameEntry.Event.Unsubscribe(ChangeClothesEventArgs.EventId, OnRefreshClothes);

            OnBtnBagClick(0);

            if (DOTween.IsTweening("tips_show"))
            {
                DOTween.Kill("tips_show", false);
            }

            if (DOTween.IsTweening("tips_hide"))
            {
                DOTween.Kill("tips_hide", true);
            }

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
                    power.value = player.Power / player.MaxPower;
                    powerProgress.text = string.Format("{0}/{1}", player.Power, player.MaxPower);
                    break;
                case EnumPriority.Energy:
                    energy.value = player.Energy / player.MaxEnergy;
                    energyProgress.text = string.Format("{0}/{1}", player.Energy, player.MaxEnergy);
                    break;
                case EnumPriority.Hygiene:
                    hygiene.value = player.Hygiene / player.MaxHygiene;
                    hygieneProgress.text = string.Format("{0}/{1}", player.Hygiene, player.MaxHygiene);
                    break;
                case EnumPriority.Health:
                    health.value = player.Health / player.MaxHealth;
                    healthProgress.text = string.Format("{0}/{1}", player.Health, player.MaxHealth);
                    break;
                default:
                    break;
            }
        }

        private void RefreshBag()
        {
            curSlotIndex = -1;
            int count = 0;
            switch (curBagIndex)
            {
                case (int) EnumBag.Property:
                    count = dataBag.listProperty.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Property property = dataBag.listProperty[i];
                        int number = 0;
                        int spriteId = 0;
                        if (property.Number > 0)
                        {
                            number = property.Number;
                            spriteId = property.propertyData.IconId;
                        }
                        RefreshSlot(EnumBag.Property, bag.GetChild(i), number, spriteId);
                    }
                    break;
                case (int)EnumBag.Clothes:
                    count = dataBag.listClothes.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Clothes clothes = dataBag.listClothes[i];
                        int number = 0;
                        int spriteId = 0;
                        if (clothes.Number > 0)
                        {
                            number = clothes.Number;
                            spriteId = clothes.clothesData.IconId;
                        }
                        RefreshSlot(EnumBag.Clothes, bag.GetChild(i), number, spriteId);
                    }
                    break;
                case (int)EnumBag.Food:
                    count = dataBag.listFood.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Food food = dataBag.listFood[i];
                        int number = 0;
                        int spriteId = 0;
                        if (food.Number > 0)
                        {
                            number = food.Number;
                            spriteId = food.foodData.IconId;
                        }
                        RefreshSlot(EnumBag.Food, bag.GetChild(i), number, spriteId);
                    }
                    break;
                case (int)EnumBag.Equipment:
                    count = dataBag.listEquipment.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Equipment equipment = dataBag.listEquipment[i];
                        int number = 0;
                        int spriteId = 0;
                        if (equipment.Number > 0)
                        {
                            number = equipment.Number;
                            spriteId = equipment.equipmentData.IconId;
                        }
                        RefreshSlot(EnumBag.Equipment, bag.GetChild(i), number, spriteId);
                    }
                    break;
                case (int)EnumBag.Pet:
                    count = dataBag.listPet.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Pet pet = dataBag.listPet[i];
                        int number = 0;
                        int spriteId = 0;
                        if (pet.Number > 0)
                        {
                            number = pet.Number;
                            spriteId = pet.petData.IconId;
                        }
                        RefreshSlot(EnumBag.Pet, bag.GetChild(i), number, spriteId);
                    }
                    break;
                default:
                    break;
            }
        }

        private void RefreshSlot(EnumBag bag, Transform slot, int number, int spriteId)
        {
            Image item = slot.Find("Item").GetComponent<Image>();
            Text num = item.transform.GetChild(0).GetComponent<Text>();
            Transform selectBox = slot.Find("SelectBox");

            if (number > 0)
            {
                item.gameObject.SetActive(true);

                switch(bag)
                {
                    case EnumBag.Property:
                        PropertyDetail propertyDetail = propertySpriteList_SO.GetPropertyDetail(spriteId);
                        item.sprite = propertyDetail.PropertyIcon;
                        break;
                    case EnumBag.Clothes:
                        ClothesDetail clothesDetail = clothesSpriteList_SO.GetClothesDetail(spriteId);
                        item.sprite = clothesDetail.ClothesIcon;
                        break;
                    case EnumBag.Food:
                        FoodDetail foodDetail = foodSpriteList_SO.GetFoodDetail(spriteId);
                        item.sprite = foodDetail.FoodIcon;
                        break;
                    case EnumBag.Equipment:
                        EquipmentDetail detail = equipmentSpriteList_SO.GetEquipmentDetail(spriteId);
                        item.sprite = detail.EquipmentIcon;
                        break;
                    case EnumBag.Pet:
                        PetDetail petDetail = petSpriteList_SO.GetPetDetail(spriteId);
                        item.sprite = petDetail.PetIcon;
                        break;
                    default:
                        break;
                }

                if (number > 1)
                {
                    num.gameObject.SetActive(true);
                    num.text = number.ToString();
                }
                else
                {
                    num.gameObject.SetActive(false);
                }
            }
            else
            {
                item.gameObject.SetActive(false);
            }

            selectBox?.gameObject.SetActive(false);
        }

        private void RefreshPlayerClothes()
        {
            int id = 0;
            if (dataBag.clothes.Number > 0)
            {
                id = dataBag.clothes.clothesData.IconId;
            }
            ClothesDetail detail = clothesSpriteList_SO.GetClothesDetail(id);
            player.sprite = detail.PlayerImage;
        }

        private void OnBtnBagClick(int index)
        {
            Transform bagBtn = btnBagPanel.GetChild(curBagIndex);
            bagBtn.GetChild(0).gameObject.SetActive(false);

            curBagIndex = index;
            bagBtn = btnBagPanel.GetChild(curBagIndex);
            bagBtn.GetChild(0).gameObject.SetActive(true);

            curSlotIndex = -1;
            RefreshBag();

            btnUse.gameObject.SetActive(false);
            if (curBagIndex == (int)EnumBag.Food)
            {
                btnUse.gameObject.SetActive(true);
            }
        }

        public void OnSlotClick(GameObject obj)
        {
            int index = int.Parse(obj.name.Split('_')[1]);
            if (0 <= curSlotIndex && curSlotIndex < bag.childCount)
            {
                Transform slot = bag.GetChild(curSlotIndex);
                slot?.Find("SelectBox")?.gameObject.SetActive(false);
            }

            curSlotIndex = index;
            obj.transform.Find("SelectBox")?.gameObject.SetActive(true);
        }

        public void OnBtnUseClick()
        {

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

        private void OnRefreshClothes(object sender, GameEventArgs e)
        {
            ChangeClothesEventArgs ne = (ChangeClothesEventArgs)e;
            if (ne == null)
            {
                return;
            }

            RefreshPlayerClothes();
        }
    }
}
