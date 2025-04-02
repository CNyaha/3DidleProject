using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float power;
    [SerializeField] private float defence;
    public bool isDead = false;
    private float maxHealth;
    private float maxPower;
    private float maxDefence;

    public float Power 
    {
        get {  return power; } private set { power = value; } 
    }

    private MonsterManager monsterManager;
    private EnemyController enemyController;

    private void Awake()
    {
        monsterManager = MonsterManager.Instance;
        enemyController = GetComponent<EnemyController>();
        maxHealth = health;
        maxPower = power;
        maxDefence = defence;
    }

    public void Init()
    {
        health = maxHealth;
        power = maxPower;
        maxDefence = defence;
    }
    
    /// <summary>
    /// 보스 혹은 몬스터에게 데미지
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (damage <= 0)
            return;
        
        health -= (damage-defence);
        SoundManager.Instance.StartAudioSFX_EnemyOnDamage();
        if (health <= 0)
        {
            isDead = true;
            this.gameObject.SetActive(false);
            if(enemyController.isBoss)
            {
                PlayerManager.Instance.player.AddExp(Random.Range(80, 101));
                monsterManager.deathCount = 0;
                PlayerManager.Instance.player.AddGold(Random.Range(100,201));
                monsterManager.curStage++;
            }
            else
            {
                PlayerManager.Instance.player.AddExp(Random.Range(10, 21));
                monsterManager.deathCount++;
                PlayerManager.Instance.player.AddGold(Random.Range(10, 51));
            }
        }
    }

    /// <summary>
    /// 몬스터 활성화 시 스텟 스테이지에 맞게 추가설정
    /// </summary>
    /// <param name="stage"></param>
    public void SetStatus(int stage)
    {
        health += (stage*0.1f)*health;
        power += (stage * 0.1f) * power;
        defence += (stage * 0.1f) * defence;
    }
}
