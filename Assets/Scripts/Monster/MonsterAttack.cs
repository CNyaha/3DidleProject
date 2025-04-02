using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour, IMonsterBehaivor<EnemyController>
{
    private EnemyController enemyController;
    private Enemy enemy;
    public void EnterBehaivor(EnemyController monsterController)
    {
        enemyController = monsterController;
        enemy = GetComponent<Enemy>();
    }

    public void ExitBehaivor(EnemyController monsterController)
    {
        
    }
    /// <summary>
    /// 공격 모션
    /// </summary>
    /// <param name="monsterController"></param>
    public void UpdateBehavior(EnemyController monsterController)
    {
        enemyController.agent.isStopped = true;
        if (Time.time - enemyController.lastAttackTime > enemyController.attackRate)
        {
            enemyController.lastAttackTime = Time.time;
            if(PlayerManager.Instance.player.CurrentHP > 0)
                enemyController.animator.SetTrigger("Attack");
        }
    }


    /// <summary>
    /// 애니메이션이벤트에 추가
    /// </summary>
    public void OnAttack()
    {
        if (PlayerManager.Instance.player == null)
        {
            Debug.Log("플레이어는 null입니다.");
        }
        if (enemy == null)
        {
            Debug.Log("enemy는 null입니다.");
        }
        Debug.Log("플레이어 공격");
        PlayerManager.Instance.player.TakeDamage(enemy.Power);
    }
}
