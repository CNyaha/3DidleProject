using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public Slider masterVol, bgmVol, sfxVol;
    public AudioMixer mainAudioMixer;

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVol", masterVol.value);
    }
    public void ChangeBgmVolume()
    {
        mainAudioMixer.SetFloat("BgmVol", bgmVol.value);
    }
    public void ChangeSfxVolume()
    {
        mainAudioMixer.SetFloat("SfxVol", sfxVol.value);
    }
}
