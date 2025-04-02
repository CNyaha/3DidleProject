using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    UseItem,        //사용 아이템
    EquipItem,      //장비 아이템
    Resource,       //자원 아이템
}

public enum EquipItemType
{
    Helmet,       //헬멧 
    Armor,        //갑바
    Shoes,        //신발
    Necklace,     //목걸이
    Ring,         //반지
    Weapon,       //무기
}

public enum ItemRarity
{
    Common,         //보통
    Rare,           //희귀
    SuperRare,      //엄청난희귀
    Epic,           //서사적인
    Legend,         //전설
}

public enum UseItemType
{
    HP,     //체력
    MP      //마나
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public int ID;                          //아이디
    public Sprite Icon;                     //아이콘
    public string Name;                     //이름
    public string Description;              //설명
    public int GradeCount;                  //업그레이드 횟수
    public int SellPrice;                   //판매 가격
    public ItemType Type;                   //타입
    public ItemRarity Rarity;               //희귀도
    public UseItemData[] useItemDatas;      //사용아이템 데이터
    public EquipItemData[] equipItemData;   //장비아이템 데이터

    public bool IsStack;                    //겹치는 아이템 가능
    public int MaxStack;                    //최대 겹칠 수 있는 수

    public bool AvailableInShop = false; // 상점에 등장 가능한 아이템인지 여부
    public int ShopPrice = 100;          // 상점에서 판매되는 가격

    public override bool Equals(object obj)
    {
        if (obj is ItemData other)
            return this.ID == other.ID;
        return false;
    }

    public override int GetHashCode()
    {
        return ID.GetHashCode();
    }
}


[Serializable]
public class UseItemData
{
    public UseItemType UseType;     //사용 타입
    public int HealthValue;         //회복할 양
}


[Serializable]
public class EquipItemData
{
    public EquipItemType EquipType;     //장비 아이템 타입
    public int UpgradePrice;            //장비 업그레이드 가격
    public int AttackValue;             //장비 공격력
    public int DefenceValue;            //장비 방어력
    public int UpgradeAttackValue;      //업그레이드 공격력 증가 수치
    public int DefenceAttackValue;      //업그레이드 방어력 증가 수치
}


