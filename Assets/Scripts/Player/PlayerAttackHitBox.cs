using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitBox : MonoBehaviour
{
    // 플레이어 일반공격 할 때 활성화시킬 콜라이더입니다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적에게 피해를 입히기
            other.GetComponent<Enemy>().TakeDamage(PlayerManager.Instance.player.Damage);
            Debug.Log("적에게 공격");
        }
    }
}
