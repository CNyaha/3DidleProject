using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    public GameObject tooltipPanel;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    private Vector3 offset = new Vector3(20f, -20f, 0f);
    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(ItemData item)
    {
        tooltipPanel.SetActive(true);
        nameText.text = item.Name;
        descriptionText.text = item.Description;
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        if (tooltipPanel.activeSelf)
        {
            tooltipPanel.transform.position = Input.mousePosition + offset;
        }
    }
}
