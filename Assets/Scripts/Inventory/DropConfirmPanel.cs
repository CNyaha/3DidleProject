using UnityEngine;
using UnityEngine.UI;

public class DropConfirmPanel : MonoBehaviour
{
    public static DropConfirmPanel Instance;

    public GameObject panel;
    public Button confirmButton;
    public Button cancelButton;

    private InventorySlotUI targetSlot;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);

        confirmButton.onClick.AddListener(() =>
        {
            targetSlot.slot.Clear();
            InventoryUI.Instance.Refresh();
            panel.SetActive(false);
        });

        cancelButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            InventoryUI.Instance.Refresh();
        });
    }

    public void Show(InventorySlotUI slot)
    {
        targetSlot = slot;
        panel.SetActive(true);
    }
}

