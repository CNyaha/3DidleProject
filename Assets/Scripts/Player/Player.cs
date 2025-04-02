using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public string PlayerName { get; private set; }
    public int Level { get; private set; } = 1;
    public int MaxExp { get; private set; }
    public int CurrentExp { get; private set; }
    public float MaxHp { get; private set; }
    public float CurrentHP { get; private set; }
    public float MaxMp { get; private set; }
    public float CurrentMp { get; private set; }
    public float Damage { get; private set; }
    public float AttackRange { get; private set; }
    public float Defence { get; private set; }

    public float CriticalChance { get; private set; }
    public float CriticalDamage { get; private set; }

    public int Gold { get; private set; } = 1000;

    public bool isWeaponEquip { get; private set; } = false;
    public int weaponIndex { get; private set; } = -1;
    public bool isHelmetEquip { get; private set; } = false;
    public int helmetIndex { get; private set; } = -1;

    public bool isInvincible { get; private set; } = false; //무적판정 

    private int expUp = 1;
    private bool isDead = false;


    private Coroutine hitCoroutine;

    public PlayerController controller;
    

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        PlayerManager.Instance.PlayerTransform = this.transform;
        controller = GetComponent<PlayerController>();
        Init("기사", 100f, 20f, 15f);
    }

    // 플레이어 기본설정
    public void Init(string playerName, float maxHp, float damage, float defence)
    {
        PlayerName = playerName;
        MaxHp = maxHp;
        CurrentHP = maxHp;
        Damage = damage;
        Defence = defence;
        MaxMp = 100;
        CurrentMp = MaxMp;
        CriticalChance = 10;
        CriticalDamage = 150;
        AttackRange = 2;
        Gold = 1000;
        CurrentExp = 0;
        MaxExp = 50;
        isDead = false;
    }

    // damage만큼의 피해를 입습니다.
    public void TakeDamage(float damage)
    {
        if (CurrentHP <= 0)
        {
            PlayerDeath();
            return;
        }
        if (isInvincible) return;
        SubstractHelath(damage);
        controller.animationHandler?.PlayerHit();

        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        hitCoroutine = StartCoroutine(HitCorourtine());

        SoundManager.Instance.StartAudioSFX_PlayerOnDamage();
        
    }

    // 플레이어가 피해를 받으면 일정시간동안 무적이 되게 설정
    private IEnumerator HitCorourtine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(1.2f);
        isInvincible = false;

    }
    // 체력 회복
    public void AddHealth(float value)
    {
        CurrentHP = Mathf.Min(CurrentHP + value, MaxHp);
    }
    // 체력 감소
    public void SubstractHelath(float value)
    {
        if (CurrentHP - value >= 0)
        {
            Debug.Log("HP가 없습니다.");
        }
        CurrentHP = Mathf.Max(CurrentHP - value, 0);
    }
    // 마나 회복
    public void AddMana(float value)
    {
        CurrentMp = Mathf.Min(CurrentMp + value, MaxMp);
    }
    // 마나 감소
    public void SubstractMana(float value)
    {
        if (CurrentMp - value >= 0)
        {
            Debug.Log("MP가 없습니다.");
        }
        CurrentMp = Mathf.Max(CurrentMp - value, 0);
    }
    // 레벨업
    public void LevelUp()
    {
        Level++;
        CurrentExp -= MaxExp;
        if (Level % 10 == 0)
        {
            expUp++;
        }
        MaxExp = expUp * Level * 50;

        Damage++;
        Defence += 0.5f;
        MaxHp += 5f;
        MaxMp += 1f;
        AddHealth(MaxHp);
        AddMana(MaxMp);
        SoundManager.Instance.StartAudioSFX_PlayerLevelUp();
    }
    // 경험치 추가
    public void AddExp(int value)
    {
        CurrentExp += value;
        if (CurrentExp > MaxExp)
        {
            LevelUp();
        }
    }

    // 골드 설정
    public void  SetGold(int amount)
    {
        Gold = Mathf.Max(0, amount);
    }

    // amount만큼의 골드 감소
    public bool TrySpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            return true;
        }
        return false;
    }

    // 골드 증가
    public void AddGold(int amount)
    {
        Gold += amount;
    }

    // 플레이어 사망
    public void PlayerDeath()
    {
        if (CurrentHP <= 0 && !isDead)
        {
            isDead = true;

            controller.playerInputEnabled(false);
            controller.enabled = false;
            controller.rb.velocity = Vector3.zero;
            controller.isAutoMode = false;

            GetComponent<PlayerAutoCombat>().SetAutoMode(false);
            GetComponent<PlayerAutoCombat>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;

            controller.animationHandler?.PlayDead();
            SoundManager.Instance.StartAudioSFX_PlayerDie();

        }

    }
    //장착적용
    public void AddEquipStats(int atk, int def)
    {
        Damage += atk;
        Defence += def;
    }
    //장착해제
    public void RemoveEquipStats(int atk, int def)
    {
        Damage -= atk;
        Defence -= def;
    }

}
