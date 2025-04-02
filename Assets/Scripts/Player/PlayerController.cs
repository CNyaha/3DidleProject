using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 5f;       // 이동속도  
    public float jumpForce = 5f;        // 점프력

    [Header("카메라")]
    public Transform cameraTransform;

    public PlayerAnimationHandler animationHandler;
    public Rigidbody rb;
    private Vector2 moveInput;  // 입력받은 이동값
    private bool jumpInput;     //  점프 입력이 됐는지
    private bool isRunning;
    private bool isAttacking;

    [SerializeField]
    private PlayerInput playerInput;  //플레이어 입력 시스템
    public SkillManager skillManager;

    [HideInInspector]
    public bool isAutoMode = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();        
        animationHandler = GetComponent<PlayerAnimationHandler>();
        playerInput = GetComponent<PlayerInput>();
        skillManager = GetComponent<SkillManager>();
    }


    // 이동 입력
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if(moveInput != Vector2.zero && !isRunning)
        {
            SoundManager.Instance.StartAudioSFXMoveRun_PlayerWalk();
        }
        else if (moveInput == Vector2.zero)
        {
            SoundManager.Instance.StopCurrentSFXMoveRunSource();
        }
    }

    // 점프 입력
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
    }

    // 이동 입력
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunning = true;
            animationHandler?.SetRunState(true);
            SoundManager.Instance.StartAudioSFXMoveRun_PlayerRun();
        }
        else if (context.canceled)
        {
            isRunning = false;
            animationHandler?.SetRunState(false);
        }
    }

    // 공격 입력
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed || !isAttacking)
        {
            isAttacking = true;
            animationHandler?.PlayAttack();
        }
    }

    // 공격 상태 초기화
    public void ResetAttackState()
    {
        isAttacking = false;
    }


    private void FixedUpdate()
    {
        if (isAutoMode) return;

        Vector3 desireMoveDir = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = cameraTransform.right;
            camRight.y = 0;
            camRight.Normalize();


            desireMoveDir = camRight * moveInput.x + camForward * moveInput.y;
        }
        else
        {
            desireMoveDir = new Vector3(moveInput.x, 0, moveInput.y);
        }
        // 수평 속도 계산
        Vector3 currentVelocity = rb.velocity;
        Vector3 horizontalVelocity = desireMoveDir * (isRunning ? moveSpeed * 1.5f : moveSpeed);
        rb.velocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);


        // 플레이어 회전
        Vector3 moveDir = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        if (jumpInput && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            animationHandler?.TriggerJump();
            SoundManager.Instance.StartAudioSFX_PlayerJump();
        }

        if (!IsGrounded())
        {
            animationHandler?.SetJumpIdle(true);
        }
        else
        {
            animationHandler?.SetJumpIdle(false);
        }

        animationHandler?.SetGrounded(IsLadingSoon());

        float speed = moveInput.sqrMagnitude;
        animationHandler?.SetMoveState(speed);

        jumpInput = false;

    }

    // 플레이어가 땅에 닿았는지
    private bool IsGrounded()
    {
        float rayDistance = 1.1f;
        Debug.Log($"점프 상태는 {Physics.Raycast(transform.position, Vector3.down, rayDistance)} 입니다.");
        return Physics.Raycast(transform.position, Vector3.down, rayDistance);
    }

    // 플레이어가 땅에 닿을건지 착지 애니메이션을 미리 불러오기 위한 함수
    private bool IsLadingSoon()
    {
        float detectDistance = 1.8f;
        bool willLandSoon = Physics.Raycast(transform.position, Vector3.down, detectDistance);

        return willLandSoon;
    }

    // 플레이어 입력시스템 설정
    public void playerInputEnabled(bool isActive)
    {
        playerInput.enabled = isActive;
    }


}
