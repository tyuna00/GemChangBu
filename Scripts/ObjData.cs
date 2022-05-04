using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjData : MonoBehaviour
{
    public enum Type { Npc, NoneNpc, Portal, MovingBox, Item, Enemy, InteractObj };

    public Type type;
    public int id;
}
