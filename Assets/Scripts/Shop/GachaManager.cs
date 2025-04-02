using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    public List<ItemData> gachaPool;
    public int gachaCost = 300;
    public GameObject panel;
    public Image rollingImage;         // 아이콘 보여줄 이미지
    public float rollingTime = 3f;     // 롤링 시간
    public float interval = 0.05f;     // 바뀌는 속도

    private void Awake()
    {
        Instance = this;
        gachaPool = new List<ItemData>();
    }

    public void PullGacha()
    {
        if (!PlayerManager.Instance.Player.TrySpendGold(gachaCost))
        {
            UIManager.Instance.SetOpenInformationUI("골드 부족!");
            return;
        }
        StartCoroutine(RollEffectCoroutine());
    }

    private IEnumerator RollEffectCoroutine()
    {
        float timer = 0f;
        Item lastShown = null;

        while (timer < rollingTime)
        {
            lastShown = ResourceManager.Instance.GetRandomRarityItem();
            rollingImage.sprite = lastShown.Data.Icon;
            timer += interval;
            yield return new WaitForSecondsRealtime(interval);
        }

        // 뽑힌 아이템 지급
        PlayerInventory.Instance.AddItem(lastShown, 1);
        InventoryUI.Instance.Refresh();
        // 결과창 띄우기
        GachaResultUI.Instance.Show(lastShown);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
