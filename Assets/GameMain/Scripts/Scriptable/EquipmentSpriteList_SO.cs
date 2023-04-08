using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSpriteList_SO", menuName = "Data/EquipmentSpriteList_SO")]
public class EquipmentSpriteList_SO : ScriptableObject
{
    public List<EquipmentDetail> EquipmentSpriteList;

    public EquipmentDetail GetEquipmentDetail(int EquipmentId)
    {
        return EquipmentSpriteList.Find(i => i.EquipmentId == EquipmentId);
    }
}

[Serializable]
public class EquipmentDetail
{
    public int EquipmentId;
    public Sprite EquipmentIcon;
}
