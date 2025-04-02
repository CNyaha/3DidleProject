using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int IsRun = Animator.StringToHash("IsRun");
    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int JumpIdle = Animator.StringToHash("JumpIdle");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int DeadTrigger = Animator.StringToHash("TriggerDead");
    private static readonly int HitTrigger = Animator.StringToHash("TriggerHit");



    protected Animator animator;
    public GameObject retryUI;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TriggerJump()
    {
        animator.SetTrigger(JumpTrigger);
    }

    public void SetGrounded(bool grounded)
    {
        animator.SetBool(IsGrounded, grounded);
    }

    public void SetJumpIdle(bool isAirborne)
    {
        animator.SetBool(JumpIdle, isAirborne);
    }

    
    public void SetMoveState(float speed)
    {
        animator.SetFloat(IsMove, speed);
        Debug.Log($"[애니메이션] IsMove: {speed}");
    }

    public void SetRunState(bool _isRun)
    {
        animator.SetBool(IsRun, _isRun);
    }

    public void PlayAttack()
    {
        animator.SetTrigger(Attack);
    }

    public void PlayDead()
    {
        animator.SetTrigger(DeadTrigger);
        retryUI.SetActive(true);
    }

    public void PlayerHit()
    {
        animator.SetTrigger(HitTrigger);
    }
}
