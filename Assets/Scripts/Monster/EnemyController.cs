using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum MonsterBehavior
    {
        Move,
        Attack,
    }

    private IMonsterBehaivor<EnemyController> monsterBehaivor;

    public float enemySpeed = 3f;
    public GameObject character;

    private Dictionary<MonsterBehavior, IMonsterBehaivor<EnemyController>> curState = new Dictionary<MonsterBehavior, IMonsterBehaivor<EnemyController>>();
    private MonsterBehaivorMachine<EnemyController> stateMachine;
    public NavMeshAgent agent;
    public float lastAttackTime;
    public float attackRate = 0.5f;
    public Animator animator;
    public bool isBoss;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        IMonsterBehaivor<EnemyController> move = GetComponent<MonsterMove>();
        IMonsterBehaivor<EnemyController> attack = GetComponent <MonsterAttack>();
        agent.speed = enemySpeed;

        curState.Add(MonsterBehavior.Move, move);
        curState.Add(MonsterBehavior.Attack, attack);

        stateMachine = new MonsterBehaivorMachine<EnemyController>(this, curState[MonsterBehavior.Move]);
    }

    // Update is called once per frame
    void Update()
    {
        Collider playerCollider = PlayerManager.Instance.PlayerTransform.GetComponent<Collider>();  // 플레이어의 콜라이더 가져오기
        Collider myCollider = GetComponent<Collider>();                                             // my(적)의 콜라이더 가져오기
        if (playerCollider == null || myCollider == null) return;
        Vector3 closetPoint = playerCollider.ClosestPoint(transform.position);                      // 내(적) 위치에서 플레이어 콜라이더의 가장 가까운 지점을 계산
        Vector3 myColset = myCollider.ClosestPoint(PlayerManager.Instance.PlayerTransform.position);    // 플레이어 위치에서 나(적)의 콜라이더 표면의 가장 가까운 지점
        if (!isBoss) // 보스가 아니라 그냥 몬스터의 사거리
        {
            if (Vector3.Distance(myColset, closetPoint) <= 2f)
            {
                stateMachine.SetState(curState[MonsterBehavior.Attack]);
            }
            if (Vector3.Distance(myColset, closetPoint) > 2f)
            {
                stateMachine.SetState(curState[MonsterBehavior.Move]);
            }
        }
        else // 보스몬스터의 사거리
        {
            if (Vector3.Distance(myColset, closetPoint) <= 6f)
            {
                stateMachine.SetState(curState[MonsterBehavior.Attack]);
            }
            if (Vector3.Distance(myColset, closetPoint) > 6f)
            {
                stateMachine.SetState(curState[MonsterBehavior.Move]);
            }
        }
        stateMachine.UpdateState();
    }
}
