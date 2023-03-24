using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCSpriteList_SO", menuName = "Data/NPCSpriteList_SO")]
public class NPCSpriteList_SO : ScriptableObject
{
    public List<NPCDetail> NPCSpriteList;

    public NPCDetail GetNPCDetail(int NPCId)
    {
        return NPCSpriteList.Find(i => i.NPCId == NPCId);
    }
}

[Serializable]
public class NPCDetail
{
    public int NPCId;
    public Sprite NPCIcon;
}
