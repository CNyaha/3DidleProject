using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerAutoCombat : MonoBehaviour
{
    public float attackInterval = 2f; //자동 기본 공격 주기
    public LayerMask enemyLayer;
    private NavMeshAgent agent;
    private Transform targetEnemy;

    public UIManager uimanager;


    private float attackTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        uimanager.AutoButton(false);
    }

    // 플레이어의 AutoMode가 켜져있으면 실행하며 적을 자동으로 탐색해 다가가서 적을 타격합니다.
    private void Update()
    {
         if (!PlayerManager.Instance.Player.controller.isAutoMode) return;

        attackTimer += Time.deltaTime;      // 공격 쿨타임 계산

        FindEnemy();                        // 가까운 적을 탐색

        if (targetEnemy != null)
        {

            Collider targetCollider = targetEnemy.GetComponent<Collider>();                     // 타겟(적)의 콜라이더
            Collider playerCollider = PlayerManager.Instance.PlayerTransform.GetComponent<Collider>();  // 플레이어의 콜라이더
            if (targetCollider == null) return;

            Vector3 enemyCloset = targetCollider.ClosestPoint(PlayerManager.Instance.PlayerTransform.position); 
            Vector3 playerCloset = playerCollider.ClosestPoint(transform.position);
            float distance = Vector3.Distance(playerCloset, enemyCloset);
            //플레이어와 적의 거리를 구합니다(중심점이 아닌 콜라이더 외곽 기준으로 가깝게 구합니다)

            if (distance > PlayerManager.Instance.Player.AttackRange)
            {
                agent.SetDestination(targetEnemy.position);
                PlayerManager.Instance.Player.controller.animationHandler?.SetMoveState(0.5f);
                // 거리가 공격범위보다 멀다면 이동
            }
            else
            {
                PlayerManager.Instance.Player.controller.animationHandler?.SetMoveState(0f);
                agent.ResetPath();      // 도착했으면 멈춘다.

                if (attackTimer >= attackInterval)
                {
                    attackTimer = 0;
                    Attack();
                }
                // 적이 공격범위 내에 있다면 캐릭터를 멈춘 후 공격쿨타임이 지나면 공격을 실행합니다.
            }
        }
        PlayerManager.Instance.Player.controller.skillManager.AutoUesSkill();

    }

    // 플레이어의 위치에서 가장 가까운 적의 위치를 찾습니다.
    private void FindEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 50f, enemyLayer);
        float minDistance = float.MaxValue;
        Transform nearest = null;

        foreach (Collider hit in hits)
        {
            if (!hit.gameObject.activeSelf) continue;

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null || enemy.isDead) continue;

            Vector3 closestPoint = hit.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = hit.transform;
            }
        }
        targetEnemy = nearest;

    }

    // 공격 함수
    private void Attack()
    {
        if (targetEnemy == null) return;
        PlayerManager.Instance.Player.controller.animationHandler?.PlayAttack();
        transform.LookAt(targetEnemy);
    }

    // 자동공격 모드 설정
    public void SetAutoMode(bool enable)
    {
        PlayerManager.Instance.Player.controller.isAutoMode = enable;
        PlayerManager.Instance.Player.controller.playerInputEnabled(!enable);


        if (!enable && agent.isOnNavMesh)
        {

            agent.ResetPath();
        }
        uimanager.AutoButton(enable);

        if (enable)
        {
            agent.enabled = true;
        }
        else
        {
            agent.enabled = false;
        }

        
    }

}
