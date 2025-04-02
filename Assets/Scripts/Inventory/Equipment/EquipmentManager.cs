using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    private Dictionary<EquipItemType, Item> equippedItems = new Dictionary<EquipItemType, Item>();

    private void Awake()
    {
        Instance = this;
    }

    public void Equip(EquipItemType type, Item newItem)
    {
        // 기존 장비가 있으면 인벤토리로 되돌리기
        if (equippedItems.TryGetValue(type, out var oldItem))
        {
            // 기존 장비 해제 시 스탯 감소
            var oldEquipData = oldItem.Data.equipItemData[0];
            PlayerManager.Instance.Player.RemoveEquipStats(oldEquipData.AttackValue, oldEquipData.DefenceValue);

            // 기존 장비 인벤토리로 되돌리기
            InventoryUI.Instance.inventory.AddItem(oldItem, 1);
        }

        // 새 장비 장착
        equippedItems[type] = newItem;

        // 새 장비 능력치 적용
        var newEquipData = newItem.Data.equipItemData[0];
        PlayerManager.Instance.Player.AddEquipStats(newEquipData.AttackValue, newEquipData.DefenceValue);
        //소리재생
        SoundManager.Instance.StartAudioSFX_ItemEquipped();
        // UI 갱신
        EquipmentUI.Instance.Refresh();
    }

    public void Unequip(EquipItemType type)
    {
        if (equippedItems.TryGetValue(type, out Item item))
        {
            // 장비 해제 시 스탯 감소
            var equipData = item.Data.equipItemData[0];
            PlayerManager.Instance.Player.RemoveEquipStats(equipData.AttackValue, equipData.DefenceValue);

            // 장비 인벤토리로 이동
            InventoryUI.Instance.inventory.AddItem(item, 1);

            // 장비 목록에서 제거
            equippedItems.Remove(type);
        }

        // UI 갱신
        EquipmentUI.Instance.Refresh();
    }

    public Item GetEquipped(EquipItemType type)
    {
        equippedItems.TryGetValue(type, out Item item);
        return item;
    }

    public Dictionary<EquipItemType, Item> GetAllEquippedItems()
    {
        return equippedItems;
    }
}
