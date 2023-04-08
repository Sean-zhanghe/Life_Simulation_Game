using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropertySpriteList_SO", menuName = "Data/PropertySpriteList_SO")]
public class PropertySpriteList_SO : ScriptableObject
{
    public List<PropertyDetail> PropertySpriteList;

    public PropertyDetail GetPropertyDetail(int PropertyId)
    {
        return PropertySpriteList.Find(i => i.PropertyId == PropertyId);
    }
}

[Serializable]
public class PropertyDetail
{
    public int PropertyId;
    public Sprite PropertyIcon;
}
