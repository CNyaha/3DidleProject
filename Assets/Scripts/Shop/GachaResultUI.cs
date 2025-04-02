using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaResultUI : MonoBehaviour
{
    public static GachaResultUI Instance;

    public GameObject panel;
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text specText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(Item item)
    {
        icon.sprite = item.Data.Icon;
        nameText.text = item.Data.Name;
        descriptionText.text = item.Data.Description;

        // 스펙 출력 예시
        if (item.Data.Type == ItemType.EquipItem && item.Data.equipItemData.Length > 0)
        {
            var data = item.Data.equipItemData[0];
            specText.text = $"공격력: {data.AttackValue}\n방어력: {data.DefenceValue}";
        }
        else if (item.Data.Type == ItemType.UseItem && item.Data.useItemDatas.Length > 0)
        {
            var data = item.Data.useItemDatas[0];
            specText.text = $"회복량: {data.HealthValue}";
        }
        else
        {
            specText.text = "기타 아이템";
        }

        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
