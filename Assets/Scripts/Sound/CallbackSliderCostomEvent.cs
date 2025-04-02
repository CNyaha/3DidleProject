using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackSliderCostomEvent : MonoBehaviour
{
    public string playerPrefsName;
    public Action<float> UpdateMixerVolume;
    private RectTransform rectSlider;
    private float lastValue;

    private void Awake()
    {
        rectSlider = GetComponent<RectTransform>();
        if (playerPrefsName != null) lastValue = PlayerPrefs.GetFloat(playerPrefsName, 0.5f);
    }

    private void Update()
    {
        float currentValue = rectSlider.anchorMin.x;
        if(Mathf.Abs(currentValue - lastValue) >= 0.001f)
        {
            UpdateMixerVolume?.Invoke(currentValue);
            lastValue = currentValue;
            PlayerPrefs.SetFloat(playerPrefsName, currentValue);
        }
    }
}
