using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClothesSpriteList_SO", menuName = "Data/ClothesSpriteList_SO")]
public class ClothesSpriteList_SO : ScriptableObject
{
    public List<ClothesDetail> ClothesSpriteList;

    public ClothesDetail GetClothesDetail(int ClothesId)
    {
        return ClothesSpriteList.Find(i => i.ClothesId == ClothesId);
    }
}

[Serializable]
public class ClothesDetail
{
    public int ClothesId;
    public Sprite ClothesIcon;
    public Sprite PlayerImage;
}
