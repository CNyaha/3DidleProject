using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.StartAudioBGM_Mainmenu();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadingSceneController.LoadScene("Stage1");
        }
    }
}
