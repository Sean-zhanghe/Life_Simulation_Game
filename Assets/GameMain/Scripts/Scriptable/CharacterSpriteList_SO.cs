using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSpriteList_SO", menuName = "Data/CharacterSpriteList_SO")]
public class CharacterSpriteList_SO : ScriptableObject
{
    public List<CharacterDetail> characterSpriteList;

    public CharacterDetail GetCharacterDetail(int characterId)
    {
        return characterSpriteList.Find(i => i.characterId == characterId);
    }
}

[Serializable]
public class CharacterDetail
{
    public int characterId;
    public Sprite characterSprite;
}
