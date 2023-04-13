using DG.Tweening;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static StarForce.Constant;

namespace StarForce
{
    public class ItemEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler    
    {
        [SerializeField] private UIBagForm uiBagForm;

        private Transform originalParent;
        private Transform Tips;

        private DataBag dataBag;

        List<string> custom = new List<string>()
        {
            "Slot_Clothes", "Slot_Weapon", "Slot_Ring", "Slot_Pet"
        };

        private void Start()
        {
            dataBag = GameEntry.Data.GetData<DataBag>();

            Tips = uiBagForm.Tips;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (DOTween.IsTweening("tips_hide"))
            {
                DOTween.Kill("tips_hide", false);
            }

            Tips.GetComponent<CanvasGroup>().DOFade(1, 0.2f).SetId("tips_show");
            Tips.position = eventData.position;

            Text name = Tips.GetChild(0).GetComponent<Text>();
            Text description = Tips.GetChild(1).GetComponent<Text>();

            if (custom.Contains(transform.parent.name))
            {
                switch (transform.parent.name)
                {
                    case "Slot_Clothes":
                        Clothes clothes = dataBag.clothes;
                        if (clothes.clothesData == null) return;

                        name.text = clothes.clothesData.Name;
                        description.text = clothes.clothesData.Description;
                        break;
                    case "Slot_Ring":
                        break;
                    case "Slot_Weapon":
                        break;
                    case "Slot_Pet":
                        break;
                    default:
                        break;
                }
                return;
            }

            int slot = int.Parse(transform.parent.name.Split('_')[1]);
            switch (uiBagForm.curBagIndex)
            {
                case (int)EnumBag.Clothes:
                    Clothes clothes = dataBag.listClothes[slot];
                    if (clothes.clothesData == null) return;

                    name.text = clothes.clothesData.Name;
                    description.text = clothes.clothesData.Description;
                    break;
                case (int)EnumBag.Property:
                    Property property = dataBag.listProperty[slot];
                    if (property.propertyData == null) return;

                    name.text = property.propertyData.Name;
                    description.text = property.propertyData.Description;
                    break;
                case (int)EnumBag.Food:
                    Food food = dataBag.listFood[slot];
                    if (food.foodData == null) return;

                    name.text = food.foodData.Name;
                    description.text = food.foodData.Description;
                    break;
                case (int)EnumBag.Equipment:
                    Equipment equipment = dataBag.listEquipment[slot];
                    if (equipment.equipmentData == null) return;

                    name.text = equipment.equipmentData.Name;
                    description.text = equipment.equipmentData.Description;
                    break;
                case (int)EnumBag.Pet:
                    Pet pet = dataBag.listPet[slot];
                    if (pet.petData == null) return;

                    name.text = pet.petData.Name;
                    description.text = pet.petData.Description;
                    break;
                default:
                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tips.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetId("tips_hide").OnComplete(() => {
                Tips.position = Vector3.zero;
            });
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Tips.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetId("tips_hide");

            originalParent = transform.parent;
            transform.SetParent(uiBagForm.transform);
            transform.position = eventData.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            string from = string.Empty;
            string to = string.Empty;

            EnumBag bag = EnumBag.None;
            switch (uiBagForm.curBagIndex)
            {
                case 0:
                    bag = EnumBag.Property;
                    break;
                case 1:
                    bag = EnumBag.Clothes;
                    break;
                case 2:
                    bag = EnumBag.Food;
                    break;
                case 3:
                    bag = EnumBag.Equipment;
                    break;
                case 4:
                    bag = EnumBag.Pet;
                    break;
                default:
                    break;
            }

            if (eventData.pointerCurrentRaycast.gameObject.name == "Item")
            {
                from = originalParent.name;
                to = eventData.pointerCurrentRaycast.gameObject.transform.parent.name;

                // 不可交换
                if ((custom.Contains(from) && custom.Contains(to)) ||
                    ((from == "Slot_Clothes" || to == "Slot_Clothes") && bag != EnumBag.Clothes) ||
                    ((from == "Slot_Weapon" || to == "Slot_Weapon") && bag != EnumBag.Equipment) ||
                    ((from == "Slot_Ring" || to == "Slot_Ring") && bag != EnumBag.Property) ||
                    ((from == "Slot_Pet" || to == "Slot_Pet") && bag != EnumBag.Pet)
                    )
                {
                    transform.SetParent(originalParent);
                    transform.localPosition = Vector3.zero;
                    transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }

                ExchangeData(from, to);
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
                transform.localPosition = Vector3.zero;
                eventData.pointerCurrentRaycast.gameObject.transform.SetParent(originalParent);
                eventData.pointerCurrentRaycast.gameObject.transform.localPosition = Vector3.zero;
                transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name.StartsWith("Slot_"))
            {
                from = originalParent.name;
                to = eventData.pointerCurrentRaycast.gameObject.name;

                // 不可交换
                if ((custom.Contains(from) && custom.Contains(to)) ||
                    ((from == "Slot_Clothes" || to == "Slot_Clothes") && bag != EnumBag.Clothes) ||
                    ((from == "Slot_Weapon" || to == "Slot_Weapon") && bag != EnumBag.Equipment) ||
                    ((from == "Slot_Ring" || to == "Slot_Ring") && bag != EnumBag.Property) ||
                    ((from == "Slot_Pet" || to == "Slot_Pet") && bag != EnumBag.Pet)
                    )
                {
                    transform.SetParent(originalParent);
                    transform.localPosition = Vector3.zero;
                    transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }

                ExchangeData(from, to);
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.localPosition = Vector3.zero;
                Transform item = eventData.pointerCurrentRaycast.gameObject.transform.GetChild(1);
                item.SetParent(originalParent);
                item.localPosition = Vector3.zero;
                transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        private void ExchangeData(string from, string to)
        {
            EnumBag bag = EnumBag.None;
            switch (uiBagForm.curBagIndex)
            {
                case 0:
                    bag = EnumBag.Property;
                    break;
                case 1:
                    bag = EnumBag.Clothes;
                    break;
                case 2:
                    bag = EnumBag.Food;
                    break;
                case 3:
                    bag = EnumBag.Equipment;
                    break;
                case 4:
                    bag = EnumBag.Pet;
                    break;
                default:
                    break;
            }

            if (custom.Contains(from) && custom.Contains(to)) return;

            if ((from == "Slot_Clothes" || to == "Slot_Clothes") && bag != EnumBag.Clothes) return;

            if ((from == "Slot_Weapon" || to == "Slot_Weapon") && bag != EnumBag.Equipment) return;

            if ((from == "Slot_Ring" || to == "Slot_Ring") && bag != EnumBag.Property) return;

            if ((from == "Slot_Pet" || to == "Slot_Pet") && bag != EnumBag.Pet) return;

            if ((from == "Slot_Clothes" || to == "Slot_Clothes") && bag == EnumBag.Clothes)
            {
                dataBag.ExchangeClothes(from, to);
                return;
            }

            if ((from == "Slot_Weapon" || to == "Slot_Weapon") && bag == EnumBag.Equipment)
            {
                dataBag.ExchangeWeapon(from, to);
                return;
            }

            if ((from == "Slot_Ring" || to == "Slot_Ring") && bag == EnumBag.Property)
            {
                dataBag.ExchangeRing(from, to);
                return;
            }

            if ((from == "Slot_Pet" || to == "Slot_Pet") && bag == EnumBag.Pet)
            {
                dataBag.ExchangePet(from, to);
                return;
            }

            dataBag.ExchangeBagSlot(bag, from, to);
        }

    }

}
