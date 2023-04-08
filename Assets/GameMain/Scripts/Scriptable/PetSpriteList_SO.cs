using StarForce.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PetSpriteList_SO", menuName = "Data/PetSpriteList_SO")]
public class PetSpriteList_SO : ScriptableObject
{
    public List<PetDetail> PetSpriteList;

    public PetDetail GetPetDetail(int PetId)
    {
        return PetSpriteList.Find(i => i.PetId == PetId);
    }
}

[Serializable]
public class PetDetail
{
    public int PetId;
    public Sprite PetIcon;
}
