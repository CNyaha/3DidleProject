using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public static EquipmentUI Instance;

    public EquipSlotUI helmetSlot;
    public EquipSlotUI armorSlot;
    public EquipSlotUI shoesSlot;
    public EquipSlotUI ringSlot;
    public EquipSlotUI necklaceSlot;
    public EquipSlotUI weaponSlot;

    private void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        var eq = EquipmentManager.Instance;
        helmetSlot.Set(eq.GetEquipped(EquipItemType.Helmet));
        armorSlot.Set(eq.GetEquipped(EquipItemType.Armor));
        shoesSlot.Set(eq.GetEquipped(EquipItemType.Shoes));
        ringSlot.Set(eq.GetEquipped(EquipItemType.Ring));
        necklaceSlot.Set(eq.GetEquipped(EquipItemType.Necklace));
        weaponSlot.Set(eq.GetEquipped(EquipItemType.Weapon));
    }
}
