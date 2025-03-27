using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    UseItem,        //��� ������
    EquipItem       //��� ������
}

public enum EquipItemType
{
    Helmet,       //���
    Armor,        //����
    Shoes,        //�Ź�
}

public enum UseItemType
{
    HP,
    MP
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public ItemType Type;
    public UseItemData[] useItemDatas;
    public EquipItemData[] equipItemData;

    public bool IsStack;
    public int MaxStack;
}


[Serializable]
public class UseItemData
{
    public UseItemType UseType;
    public int HealthValue;
}


[Serializable]
public class EquipItemData
{
    public EquipItemType EquipType;
    public int ArmorValue;
}
