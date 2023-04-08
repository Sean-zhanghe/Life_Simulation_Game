using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodSpriteList_SO", menuName = "Data/FoodSpriteList_SO")]
public class FoodSpriteList_SO : ScriptableObject
{
    public List<FoodDetail> FoodSpriteList;

    public FoodDetail GetFoodDetail(int FoodId)
    {
        return FoodSpriteList.Find(i => i.FoodId == FoodId);
    }
}

[Serializable]
public class FoodDetail
{
    public int FoodId;
    public Sprite FoodIcon;
}
