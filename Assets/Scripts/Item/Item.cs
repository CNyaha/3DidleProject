using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public static int GradeMax = 20;            //최대 업그레이드 수
    public static float ProbabilityMin = 10f;   //최소 확률
    public ItemData Data;
    public Item(ItemData _itemData)
    {
        Data = _itemData;
    }


    //업그레이드 성공 시 실행할 메서드
    private void UpgradeItem()
    {
        if (Data.Type == ItemType.EquipItem ||
            Data.equipItemData != null)
        {
            EquipItemData data = Data.equipItemData[0];

            if(data.UpgradePrice > PlayerManager.Instance.player.Gold)
            {
                UIManager.Instance.SetOpenInformationUI("강화 실패\n플레이어 골드 부족");
                return;
            }
            PlayerManager.Instance.player.AddGold(-data.UpgradePrice);
            Data.GradeCount++;
            AddEquipItemValue(data, data.UpgradeAttackValue, data.DefenceAttackValue);
        }
    }


    //장비 데이터에 공격력, 방어력을 올리는 메서드
    private void AddEquipItemValue(EquipItemData data, int attackValue, int defenceValue)
    {
        data.AttackValue += attackValue;
        data.DefenceValue += defenceValue;
    }


    /// <summary>
    /// 업그레이드를 시도할 메서드
    /// </summary>
    public void TryUpgradeItem()
    {
        int GradeCount = Data.GradeCount;
        if (GradeCount < GradeMax)
        {
            SoundManager.Instance.StartAudioSFX_ItemUpgrade();
            float Probability = 100 - ((100 - ProbabilityMin) / GradeMax) * GradeCount;
            if (Util.ComputeProbability(Probability))
            {
                UpgradeItem();
                UIManager.Instance.SetOpenInformationUI("강화 성공");
            }
            else
            {
                UIManager.Instance.SetOpenInformationUI("강화 실패");
            }
        }
        else
        {
            UIManager.Instance.SetOpenInformationUI("강화 불가능\n최대 강화 횟수 도달");
        }
    }


    /// <summary>
    /// 아이템을 사용할 메서드
    /// </summary>
    public void UseUsableItem()
    {
        SoundManager.Instance.StartAudioSFX_UseItemUse();
        for (int i = 0; i < Data.useItemDatas.Length; i++)
        {
            UseItemData useItemData = Data.useItemDatas[i];
            switch (useItemData.UseType)
            {
                case UseItemType.HP:
                    PlayerManager.Instance.player.AddHealth(useItemData.HealthValue);
                    break;

                case UseItemType.MP:
                    PlayerManager.Instance.player.AddMana(useItemData.HealthValue);
                    break;
            }
        }
    }
}
