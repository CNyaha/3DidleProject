using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour, IMonsterBehaivor<EnemyController>
{
    private EnemyController enemyController;

    public void EnterBehaivor(EnemyController monsterController)
    {
        enemyController = monsterController;
    }


    public void ExitBehaivor(EnemyController monsterController)
    {
    }


    public void UpdateBehavior(EnemyController monsterController)
    {
        enemyController.agent.isStopped = false;
        NavMeshPath path = new NavMeshPath();
        if (enemyController.agent.CalculatePath(PlayerManager.Instance.PlayerTransform.position,path))
        {
            enemyController.agent.SetDestination(PlayerManager.Instance.PlayerTransform.position);
        }
    }
}
