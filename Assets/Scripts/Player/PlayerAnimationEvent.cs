using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [Header("공격 히트박스")]
    [SerializeField]
    private GameObject attackHitBox;

    private void Awake()
    {
        if (attackHitBox != null)
        {
            attackHitBox.SetActive(false);
        }
    }

    public void ResetAttackState()
    {
        PlayerManager.Instance.Player.controller.ResetAttackState();
    }

    public void EnableAttackHitBox() => attackHitBox.SetActive(true);
    public void DisableAttackHitBox() => attackHitBox.SetActive(false);



}
