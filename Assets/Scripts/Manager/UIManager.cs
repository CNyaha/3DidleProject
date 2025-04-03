using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Stat")]
    [SerializeField] Player player;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI atk;
    [SerializeField] TextMeshProUGUI def;

    [SerializeField] TextMeshProUGUI stage;
    [SerializeField] TextMeshProUGUI money;

    [Header("Condition")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider mpSlider;
    [SerializeField] Slider expSlider;
    [SerializeField] Button activeButton;
    [SerializeField] Button autoButton;
    Coroutine glowCoroutine;

    [Header("Skill")]

    [Header("Settings")]
    [SerializeField] MenuController pauseMenu;
    [SerializeField] MenuController settingsMenu;
    [SerializeField] Image fade;

    [Header("InventoryTap")]
    [SerializeField] GameObject store;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject upgrade;

    [Header("InformationUI")]
    [SerializeField] TextMeshProUGUI textInformation;
    [SerializeField] GameObject informationUI;
    [SerializeField] Button btnExitInformation;
    [SerializeField] GameObject shadowUI;

    [Header("PlayerDeathPopUp")]
    [SerializeField] GameObject Yes;
    [SerializeField] GameObject No;

    [Header("OptionSliderBar")]
    [SerializeField] public RectTransform masterVolume;
    [SerializeField] public RectTransform sfxVolume;
    [SerializeField] public RectTransform bgmVolume;

    bool isFade;
    bool isSettings;
    bool isStopNow = false;

    protected override void Awake()
    {
        base.Awake();
        if (btnExitInformation != null)
            btnExitInformation.onClick.AddListener(SetCloseInformationUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSettings)
                MoveSettingsMenu();
            else
                MovePauseMenu();
        }
    }

    private void LateUpdate()
    {
        if (level == null) return;

        UpdateStat();
        UpdateCondition();
    }

    void UpdateStat()
    {
        level.text = $"Lv.{player.Level}";
        playerName.text = player.PlayerName;
        atk.text = player.Damage.ToString();
        def.text = player.Defence.ToString();

        stage.text = $"Stage{MonsterManager.Instance.curStage}";
        money.text = player.Gold.ToString();
    }

    void UpdateCondition()
    {
        hpSlider.value = player.CurrentHP / player.MaxHp;
        mpSlider.value = player.CurrentMp / player.MaxMp;
        expSlider.value = (float)player.CurrentExp / (float)player.MaxExp;
    }

    public void MovePauseMenu()
    {
        if (!isSettings)
        {
            StopCoroutine(FadeInOut(isFade));
            isFade = !isFade;
            fade.gameObject.SetActive(isFade);
            StartCoroutine(FadeInOut(isFade));
            pauseMenu.MoveToTarget();
        }
    }

    public void MoveSettingsMenu()
    {
        isSettings = !isSettings;
        settingsMenu.MoveToTarget();
    }

    IEnumerator FadeInOut(bool isFade)
    {
        if (isFade)
        {
            float fadeCount = 0;
            while (fadeCount < 0.7f)
            {
                fadeCount += 0.1f;
                yield return new WaitForSeconds(0.01f);
                fade.color = new Color(0, 0, 0, fadeCount);
            }
        }
        else
        {
            float fadeCount = 0.7f;
            while (fadeCount > 0)
            {
                fadeCount -= 0.1f;
                yield return new WaitForSeconds(0.01f);
                fade.color = new Color(0, 0, 0, fadeCount);
            }
        }
    }

    public void AutoButton(bool isAutoMode)
    {
        activeButton.gameObject.SetActive(!isAutoMode);
        autoButton.gameObject.SetActive(isAutoMode);

        if (glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
            glowCoroutine = null;
        }

        if (isAutoMode)
        {
            glowCoroutine = StartCoroutine(GlowEffectCoroutine(autoButton));
        }
        else if (glowCoroutine != null)
        {
            SetAlpha(autoButton, 1f);
        }

    }

    private IEnumerator GlowEffectCoroutine(Button target)
    {
        float duration = 1f;
        float timer = 0f;


        CanvasGroup canvasGroup = target.gameObject.AddComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = target.gameObject.AddComponent<CanvasGroup>();
        }

        while (true)
        {
            if (canvasGroup == null)
            {
                Debug.LogWarning("CanvasGroup이 존재하지 않음");
                yield break;
            }

            timer += Time.deltaTime;
            float alpha = Mathf.PingPong(timer, duration) / duration;
            canvasGroup.alpha = Mathf.Lerp(0.5f, 1f, alpha);
            yield return null;
        }
    }

    private void SetAlpha(Button button, float alpha)
    {
        CanvasGroup canvasGroup = button.gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    public void SetStore()
    {
        isStopNow = !isStopNow;
        if (isStopNow)
        {
            Time.timeScale = 0f;
            shadowUI.SetActive(true);
            store.SetActive(true);
        }
        else
        {
            SetActiveAllFalse();
        }
    }

    public void SetInventory()
    {
        isStopNow = !isStopNow;
        if (isStopNow)
        {
            Time.timeScale = 0f;
            shadowUI.SetActive(true);
            inventory.SetActive(true);
        }
        else
        {
            SetActiveAllFalse();
        }
    }

    public void SetUpgrade()
    {
        isStopNow = !isStopNow;
        if (isStopNow)
        {
            Time.timeScale = 0f;
            shadowUI.SetActive(true);
            upgrade.SetActive(true);
            inventory.SetActive(true);
        }
        else
        {
            SetActiveAllFalse();
        }
    }

    public void SetOpenInformationUI(string text)
    {
        textInformation.text = text;
        informationUI.SetActive(true);
    }

    private void SetCloseInformationUI()
    {
        informationUI.SetActive(false);
    }

    public void SetActiveAllFalse()
    {
        isStopNow = false;
        store.SetActive(false);
        shadowUI.SetActive(false);
        upgrade.SetActive(false);
        inventory.SetActive(false);
        Time.timeScale = 1f;
    }

    public void YesButton()
    {
        LoadingSceneController.LoadScene("Stage1");
    }

    public void NoButton()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
}
