using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SoundManager : Singleton<SoundManager>
{
    public readonly string MasterGroupName = "Master";  //오디오 믹서 마스터 그룹 이름
    public readonly string SFXGroupName = "SFX";        //오디오 믹서 SFX 그룹 이름
    public readonly string BGMGroupName = "BGM";        //오디오 믹서 BGM 그룹 이름
    public readonly string SoundDataName = "SoundData"; //오디오 폴더 이름

    public GameObject BGM_SoundPool;        //BGM소리 오브젝트를 풀링할 오브젝트
    public GameObject SFX_SoundPool;        //SFX소리 오브젝트를 풀링할 오브젝트
    public GameObject SFX_MoveRun_SoundPool;//걷거나 뛰는 SFX소리 오브젝트를 풀링할 오브젝트

    public AudioSource CurrentBGMSource;     //현재 재생되고 있는 BGM소스
    public AudioSource CurrentSFXMoveRunSource; //현재 재생되고 있는 SFX MoveRun소스

    Dictionary<AudioClip, List<AudioSource>> audioPoolsSFX = new();     //SFX오디오 소스를 저장할 Dictionary
    Dictionary<AudioClip, AudioSource> audioPoolsBGM = new();           //BGM오디오 소스를 저장할 Dictionary
    Dictionary<AudioClip, AudioSource> audioPoolsSFXMoveRun = new();    //SFX MoveRun오디오 소스를 저장할 Dictionary

    public AudioMixer audioMixer;
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;

    #region 음악 경로 지정 관련 
    public AudioClip menuBGM;               //메인 메뉴 BGM
    public AudioClip battleBGM;             //베틀 메뉴 BGM

    public AudioClip playerOnDamageSFX;     //플레이어 피격 SFX
    public AudioClip playerWalkSFX;         //플레이어 걸음 SFX
    public AudioClip playerJumpSFX;         //플레이어 점프 SFX
    public AudioClip playerRunSFX;          //플레이어 뛰기 SFX
    public AudioClip playerDieSFX;          //플레이어 사망 SFX
    public AudioClip playerLevelUpSFX;      //플레이어 레벨 업 SFX
    public AudioClip enemyOnDamageSFX;      //몬스터 피격 SFX
    public AudioClip itemEquippedSFX;       //아이템 장착 SFX
    public AudioClip itemUpgradeSFX;        //아이템 강화 SFX
    public AudioClip useItemUseSFX;         //아이템 사용 SFX
    public AudioClip dungeonClearSFX;       //던전 클리어 SFX
    #endregion

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        CreatePoolObject();                 //풀링하는 오브젝트를 한꺼번에 저장할 오브젝트 생성
        initAudioClip();                    //Clip들의 Resource경로 설정
        initAudioMixer();                   //오디오 믹서들을 초기화
        initAudioEvent();                   //오디오 볼륨조절 이벤트 추가
    }


    //풀링하는 오브젝트를 한꺼번에 저장할 오브젝트 생성
    private void CreatePoolObject()
    {
        BGM_SoundPool = new GameObject("SoundPoolBGM");
        SFX_SoundPool = new GameObject("SoundPoolSFX");
        SFX_MoveRun_SoundPool = new GameObject("SoundPoolSFXMoveRun");
        BGM_SoundPool.transform.parent = transform;
        SFX_SoundPool.transform.parent = transform;
        SFX_MoveRun_SoundPool.transform.parent = transform;
    }


    //오디오 클립을 초기화
    private void initAudioClip()
    {
        menuBGM = Resources.Load<AudioClip>($"{SoundDataName}/MenuBGM");
        battleBGM = Resources.Load<AudioClip>($"{SoundDataName}/BattleBGM");
        playerOnDamageSFX = Resources.Load<AudioClip>($"{SoundDataName}/PlayerOnDamage");
        playerWalkSFX = Resources.Load<AudioClip>($"{SoundDataName}/Walk");
        playerJumpSFX = Resources.Load<AudioClip>($"{SoundDataName}/Jump");
        playerRunSFX = Resources.Load<AudioClip>($"{SoundDataName}/Run");
        playerDieSFX = Resources.Load<AudioClip>($"{SoundDataName}/PlayerDie");
        playerLevelUpSFX = Resources.Load<AudioClip>($"{SoundDataName}/PlayerLevelUp");
        enemyOnDamageSFX = Resources.Load<AudioClip>($"{SoundDataName}/EnemyOnDamage");
        itemEquippedSFX = Resources.Load<AudioClip>($"{SoundDataName}/ItemEquipped");
        itemUpgradeSFX = Resources.Load<AudioClip>($"{SoundDataName}/ItemUpgrade");
        useItemUseSFX = Resources.Load<AudioClip>($"{SoundDataName}/PlayerUseUseItem");
        dungeonClearSFX = Resources.Load<AudioClip>($"{SoundDataName}/DungeonClear");
    }


    //오디오 믹서를 초기화
    private void initAudioMixer()
    {
        audioMixer = Resources.Load<AudioMixer>("AudioMixer/MainAudioMixer");
        AudioMixerGroup[] bgmGroups = audioMixer.FindMatchingGroups($"{MasterGroupName}/{BGMGroupName}");
        AudioMixerGroup[] sfxGruips = audioMixer.FindMatchingGroups($"{MasterGroupName}/{SFXGroupName}");
        bgmGroup = bgmGroups[0];
        sfxGroup = sfxGruips[0];
    }


    //오디오 볼륨 조절 이벤트 등록
    private void initAudioEvent()
    {
        if (UIManager.Instance.masterVolume == null) return;

        UIManager.Instance.masterVolume.GetComponent<CallbackSliderCostomEvent>().UpdateMixerVolume += SetMasterVolume;
        UIManager.Instance.sfxVolume.GetComponent<CallbackSliderCostomEvent>().UpdateMixerVolume += SetSFXVolume;
        UIManager.Instance.bgmVolume.GetComponent<CallbackSliderCostomEvent>().UpdateMixerVolume += SetBGMVolume;
    }


    // 새로운 BGM오브젝트를 생성하며 AudioSource를 반환하는 메서드
    private AudioSource AddAudioBGMObject(AudioClip clip)
    {
        GameObject audioObject = new GameObject("AudioBGM");
        audioObject.transform.SetParent(BGM_SoundPool.transform);
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = bgmGroup;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.clip = clip;
        audioSource.Play();
        CurrentBGMSource = audioSource;
        return audioSource;
    }


    // 새로운 SFX오브젝트를 생성하며 AudioSource를 반환하는 메서드
    private AudioSource AddAudioSFXObject(AudioClip clip)
    {
        GameObject audioObject = new GameObject("AudioSFX");
        audioObject.transform.SetParent(SFX_SoundPool.transform);
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxGroup;
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.Play();
        return audioSource;
    }


    /// <summary>
    /// SFX효과음을 재생할 때 실행할 메서드
    /// </summary>
    /// <param name="clip">재생할 클립을 선언</param>
    private void StartAudioSFX(AudioClip clip)
    {
        ///딕셔너리에서 해당 오디오데이터에 대한 정보를 가져옴.
        ///해당 딕셔너리의 List<AudioSource>에 접근해서 해당 AudioSource에 요소의 맨 앞이 isPlay중인지 검사함.
        ///isPlay중이 아니라면 해당 오디오를 다시 재생시키고 리스트의 맨 뒤로 이동시킴.
        ///만일 맨 앞에 있는 AudioSource가 재생중이라면 다른 모든 AudioSource도 재생 중이라는 뜻이니
        ///해당 오디오 소스를 하나 새로 추가해서 재생 시킨 후 맨 뒤에 추가함.

        AudioSource tempAudio;
        if (audioPoolsSFX.ContainsKey(clip))
        {
            List<AudioSource> tempList = audioPoolsSFX[clip];
            if (tempList != null &&
              !(tempList[0].isPlaying))
            {
                tempAudio = tempList[0];
                tempAudio.Play();
                tempList.MoveFirstToLastList();
            }
            else
            {
                tempAudio = AddAudioSFXObject(clip);
                tempList.Add(tempAudio);
            }
        }
        else
        {
            List<AudioSource> tempList = new List<AudioSource>();
            tempAudio = AddAudioSFXObject(clip);
            tempList.Add(tempAudio);
            audioPoolsSFX.Add(clip, tempList);
        }

        if(tempAudio != null) SetAudioPositionForPlayer(tempAudio);
    }


    /// <summary>
    /// BGM을 재생할 때 사용하는 메서드
    /// </summary>
    /// <param name="clip">재생할 클립을 선언</param>
    private void StartAudioBGM(AudioClip clip)
    {
        ///BGM 오브젝트의 풀링은 SFX와 다르게 실행될 필요가 있음.
        ///BGM은 하나를 초과하는 오디오들이 실행될 경우 BGM이 겹치는 문제가 발생함.
        ///그러므로 BGM쪽은 따로 Dictionary<AudioClip, AudioSource>로 관리하는 쪽이 유리함.
        ///왜냐하면 어차피 Loof로 계속 돌게 될탠데 List로 추가할 필요없기 때문.
        ///BGM부분은 해당 Transform을 기준으로 정렬.
        ///BGM을 관리하는 게임오브젝트의 자식 오브젝트가 있는지 확인.
        ///있다면 0번째에 있는 오브젝트의 AudioSource를 가져와서 멈춰줌.
        ///특정 AudioSource를 가지고 있는 GameObject의 위치를 BGM을 관리하는 오브젝트의 0번째로 올림.
        ///이후 해당 AudioSource를 활성화 시켜둠.

        StopCurrentBGMSource();

        AudioSource tempAudio;
        if (audioPoolsBGM.ContainsKey(clip))
        {
            tempAudio = audioPoolsBGM[clip];
            tempAudio.Play();
        }
        else
        {
            tempAudio = AddAudioBGMObject(clip);
            audioPoolsBGM.Add(clip, tempAudio);
        }

        if (tempAudio != null)
        {
            CurrentBGMSource = tempAudio;
            SetAudioPositionForPlayer(tempAudio);
        }
    }


    /// <summary>
    /// SFX MoveRun을 재생할 때 사용하는 메서드
    /// </summary>
    /// <param name="clip">재생할 클립을 선언</param>
    private void StartAudioSFXMoveRun(AudioClip clip)
    {
        StopCurrentSFXMoveRunSource();

        AudioSource tempAudio;
        if (audioPoolsSFXMoveRun.ContainsKey(clip))
        {
            tempAudio = audioPoolsSFXMoveRun[clip];
            tempAudio.Play();
        }
        else
        {
            tempAudio = AddAudioBGMObject(clip);
            audioPoolsSFXMoveRun.Add(clip, tempAudio);
        }

        if (tempAudio != null)
        {
            CurrentSFXMoveRunSource = tempAudio;
            SetAudioPositionForPlayer(tempAudio);
        }
    }


    //딜레이로 소리를 출력할 때 실행할 코루틴
    private IEnumerator DelayStartAudio(Action action, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }


    #region BGM 출력 관련
    /// <summary>
    /// BGM MainMenu을 실행할 메서드
    /// </summary>
    public void StartAudioBGM_Mainmenu()
    {
        StartAudioBGM(menuBGM);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioBGM_Mainmenu(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioBGM_Mainmenu, delayTime));
    }


    /// <summary>
    /// BGM battle을 실행할 메서드
    /// </summary>
    public void StartAudioBGM_Battle()
    {
        StartAudioBGM(battleBGM);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioBGM_Battle(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioBGM_Battle, delayTime));
    }
    #endregion


    /// <summary>
    /// 현재 실행되고 있는 BGM소스를 멈추고 싶을 때 실행하는 메서드
    /// </summary>
    public void StopCurrentBGMSource()
    {
        if(CurrentBGMSource != null)
        {
            CurrentBGMSource.Stop();
            CurrentBGMSource = null;
        }
    }


    /// <summary>
    /// 플레이어가 움직임을 멈췄을 때 소리 출력을 멈출 메서드
    /// </summary>
    public void StopCurrentSFXMoveRunSource()
    {
        if (CurrentSFXMoveRunSource != null)
        {
            CurrentSFXMoveRunSource.Stop();
            CurrentSFXMoveRunSource = null;
        }
    }


    #region SFX 출력 관련
    /// <summary>
    /// SFX PlayerDie을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_PlayerDie()
    {
        StartAudioSFX(playerDieSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_PlayerDie(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_PlayerDie, delayTime));
    }


    /// <summary>
    /// SFX UseItemUse을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_UseItemUse()
    {
        StartAudioSFX(useItemUseSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_UseItemUse(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_UseItemUse, delayTime));
    }


    /// <summary>
    /// SFX DungeonClear을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_DungeonClear()
    {
        StartAudioSFX(dungeonClearSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_DungeonClear(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_DungeonClear, delayTime));
    }


    /// <summary>
    /// SFX ItemEquipped을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_ItemEquipped()
    {
        StartAudioSFX(itemEquippedSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_ItemEquipped(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_ItemEquipped, delayTime));
    }


    /// <summary>
    /// SFX EnemyOnDamage을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_EnemyOnDamage()
    {
        StartAudioSFX(enemyOnDamageSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_EnemyOnDamage(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_EnemyOnDamage, delayTime));
    }


    /// <summary>
    /// SFX ItemUpgrade을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_ItemUpgrade()
    {
        StartAudioSFX(itemUpgradeSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_ItemUpgrade(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_ItemUpgrade, delayTime));
    }


    /// <summary>
    /// SFX PlayerJump을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_PlayerJump()
    {
        StartAudioSFX(playerJumpSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_PlayerJump(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_PlayerJump, delayTime));
    }


    /// <summary>
    /// SFX PlayerLevelUp을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_PlayerLevelUp()
    {
        StartAudioSFX(playerLevelUpSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_PlayerLevelUp(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_PlayerLevelUp, delayTime));
    }


    /// <summary>
    /// SFX PlayerOnDamage을 실행할 메서드
    /// </summary>
    public void StartAudioSFX_PlayerOnDamage()
    {
        StartAudioSFX(playerOnDamageSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFX_PlayerOnDamage(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFX_PlayerOnDamage, delayTime));
    }


    /// <summary>
    /// SFX PlayerWalk을 실행할 메서드
    /// </summary>
    public void StartAudioSFXMoveRun_PlayerWalk()
    {
        StartAudioSFXMoveRun(playerWalkSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFXMoveRun_PlayerWalk(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFXMoveRun_PlayerWalk, delayTime));
    }


    /// <summary>
    /// SFX PlayerRun을 실행할 메서드
    /// </summary>
    public void StartAudioSFXMoveRun_PlayerRun()
    {
        StartAudioSFXMoveRun(playerRunSFX);
    }
    /// <param name="delayTime">스타트에 지연을 주고 싶은 시간을 입력</param>
    public void StartAudioSFXMoveRun_PlayerRun(float delayTime)
    {
        StartCoroutine(DelayStartAudio(StartAudioSFXMoveRun_PlayerRun, delayTime));
    }
    #endregion


    #region 볼륨 조절 관련
    /// <summary>
    /// Master 볼륨을 조절할 때 사용할 메서드
    /// </summary>
    /// <param name="sliderValue">0 ~ 1의 값을 가진 슬라이더 바</param>
    public void SetMasterVolume(float sliderValue)
    {
        if (sliderValue <= 0.0001f)
        {
            audioMixer.SetFloat(MasterGroupName, -80f);
        }
        else
        {
            float volume = Mathf.Log10(sliderValue) * 20f;
            audioMixer.SetFloat(MasterGroupName, volume);
        }
    }


    /// <summary>
    /// BGM 볼륨을 조절할 때 사용할 메서드
    /// </summary>
    /// <param name="sliderValue">0 ~ 1의 값을 가진 슬라이더 바</param>
    public void SetBGMVolume(float sliderValue)
    {
        if(sliderValue <= 0.0001f)
        {
            audioMixer.SetFloat(BGMGroupName, -80f);
        }
        else
        {
            float volume = Mathf.Log10(sliderValue) * 20f;
            audioMixer.SetFloat(BGMGroupName, volume);
        }

    }


    /// <summary>
    /// SFX 볼륨을 조절할 때 사용할 메서드
    /// </summary>
    /// <param name="sliderValue">0 ~ 1의 값을 가진 슬라이더 바</param>
    public void SetSFXVolume(float sliderValue)
    {
        if (sliderValue <= 0.0001f)
        {
            audioMixer.SetFloat(SFXGroupName, -80f);
        }
        else
        {
            float volume = Mathf.Log10(sliderValue) * 20f;
            audioMixer.SetFloat(SFXGroupName, volume);
        }
    }
    #endregion


    //TODO: 모든 SFX 출력 멈춤. 모든 List<AudioSource>를 0번부터 순회하여 IsPlay가 false인 녀석이 나온다면 빠져나감.

    //TODO: 실행되고 있는 모든 코루틴도 종료시켜야함.

    //실행되는 오디오의 Transform의 값을 플레이어 근처로 초기화시켜주는 메서드.
    private void SetAudioPositionForPlayer(AudioSource audioSource)
    {
        Transform playerTransform = PlayerManager.Instance.player?.transform;
        if(playerTransform != null)
        {
            audioSource.transform.position = playerTransform.position;
        }
        else
        {
            audioSource.transform.position = Vector3.zero;
        }
    }


    ///README
    ///사운드를 출력하고 싶을 경우
    ///BGM 출력 관련 및 SFX 출력 관련 메서드를 확인해서 출력하고 싶은 Clip이 들어있는 메서드를 확인.
    ///출력과 관련된 메서드들은 오버로드가 되어있는 부분이 있음.
    ///float delayTime에 매개변수를 넣을 경우 해당 시간 이후에 소리가 출력될 수 있도록 시간 조절 가능.
}
