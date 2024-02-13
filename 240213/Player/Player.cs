using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour, IAlive
{
    /// 실습_240125
    /// 1. 플레이어는 WS키로 전진/후진을 한다.
    /// 2. 플레이어는 AD키로 좌회전/우회전을 한다.
    /// 3. 플레이어가 움직이면(전진/후진/좌회전/우회전) Player_Move 애니메이션이 재생된다.
    /// 4. 이동 입력이 없으면 Player_Idle 애니메이션이 재생된다.
    /// 5. Player_Move 애니메이션은 팔 다리가 앞뒤로 흔들린다.
    /// 6. Player_Idle 애니메이션은 머리가 살짝 앞뒤로 까딱거린다.

    PlayerInputActions inputActions;
    Rigidbody rigid;
    Animator animator;

    /// <summary>
    /// 이동 방향 (1 : 전진, -1 : 후진, 0 : 정지)
    /// </summary>
    float moveDirection = 0.0f;

    /// <summary>
    /// 이동 속도 (기준 속도)
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 현재 이동 속도
    /// </summary>
    float currentMoveSpeed = 5.0f;

    /// <summary>
    /// 회전 방향 (1 : 우회전, -1 : 좌회전, 0 : 정지)
    /// </summary>
    float rotateDirection = 0.0f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// 애니메이터용 해시값
    /// </summary>
    readonly int IsMoveHash = Animator.StringToHash("IsMove");
    readonly int UseHash = Animator.StringToHash("Use");
    readonly int DieHash = Animator.StringToHash("Die");

    /// <summary>
    /// 점프력
    /// </summary>
    public float jumpPower = 6.0f;

    /// <summary>
    /// 공중에 떠있는지 아닌지 나타내는 프로퍼티
    /// </summary>
    bool InAir
    {
        get => groundCount < 1;
    }

    /// <summary>
    /// 접촉하고 있는 "Ground" 태그 오브젝트의 개수 확인 및 설정용 프로퍼티
    /// </summary>
    int GroundCount
    {
        get => groundCount;
        set
        {
            if (groundCount < 0) // 0 이하이면 0에서 설정
            {
                groundCount = 0;
            }
            groundCount = value;
            if (groundCount < 0) // 설정한 값이 0 이하이면 0
            {
                groundCount = 0;
            }
        }
    }

    /// <summary>
    /// 접촉하고 있는 "Ground" 태그 오브젝트의 개수
    /// </summary>
    int groundCount = 0;

    /// <summary>
    /// 점프 쿨타임
    /// </summary>
    public float jumpCoolTime = 5.0f;

    /// <summary>
    /// 남아있는 쿨타임
    /// </summary>
    float jumpCoolRemains = -1.0f;

    /// <summary>
    /// 점프가 가능한지 확인하는 프로퍼티 (점프중이 아니고 쿨타임이 다 지났다.)
    /// </summary>
    bool IsJumpAvailable => !InAir && (jumpCoolRemains < 0.0f);

    /// <summary>
    /// 플레이어의 생존 여부
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 플레이어의 사망을 알리는 델리게이트
    /// </summary>
    public Action onDie;

    /// <summary>
    /// 시작했을 때의 플레이어 수명
    /// </summary>
    public float startLifeTime = 10.0f;

    /// <summary>
    /// 현재 플레이어의 수명
    /// </summary>
    float lifeTime = 0.0f;

    /// <summary>
    /// 플레이어의 수명을 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    float LifeTime
    {
        get => lifeTime;
        set
        {
            lifeTime = value;
            if (lifeTime < 0.0f)
            {
                lifeTime = 0.0f; // 수명이 다 되었으면 0.0으로 숫자 정리 및 사망 처리
                Die();
            }
            onLifeTimeChange?.Invoke(lifeTime / startLifeTime); // 현재 수명 비율을 알림
        }
    }

    /// <summary>
    /// 수명이 변경될 때 실행될 델리게이트
    /// </summary>
    public Action<float> onLifeTimeChange;

    private void Awake()
    {
        //inputActions = new PlayerInputActions(); // 인풋 액션 생성
        inputActions = new ();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        ItemUseChecker checker = GetComponentInChildren<ItemUseChecker>();
        checker.onItemUse += (interacable) => interacable.Use(); ///// 일회용이기에 람다식 사용
    }

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        LifeTime = startLifeTime;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable(); // 활성화될 때 Player 액션맵을 활성화
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Use.performed += OnUseInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable(); // Player 액션맵을 비활성화
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        //context.started;
        //context.performed;
        //context.canceled;
        SetInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJumpInput(InputAction.CallbackContext _)
    {
        Jump();
    }

    private void OnUseInput(InputAction.CallbackContext context)
    {
        animator.SetTrigger(UseHash);
    }

    private void Update()
    {
        jumpCoolRemains -= Time.deltaTime;
        //if (jumpCoolRemains < 0)
        //{
        //    Debug.Log("Jump Ready");
        //}

        LifeTime -= Time.deltaTime;
    }

    private void FixedUpdate() ///// Update를 사용하면 캐릭터가 떨리는 현상이 나타나므로, FixedUpdate 사용
    {
        //Debug.Log(Time.fixedDeltaTime);
        Move();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCount++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCount--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.onMove += OnRideMovingObject; // 플랫폼 트리거에 들어갔을 때 함수 연결
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.onMove -= OnRideMovingObject; // 플랫폼 트리거에서 나왔을 때 연결 해제
        }
    }

    /// <summary>
    /// 움직이는 물체에 탑승했을 때 연결될 함수 
    /// </summary>
    /// <param name="delta">움직인 양</param>
    void OnRideMovingObject(Vector3 delta)
    {
        rigid.MovePosition(rigid.position + delta);
    }

    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="input">입력된 방향</param>
    /// <param name="IsMove">이동 중이면 true, 이동 중이 아니면 false</param>
    void SetInput(Vector2 input, bool IsMove)
    {
        rotateDirection = input.x;
        moveDirection = input.y;

        animator.SetBool(IsMoveHash, IsMove);
    }

    /// <summary>
    /// 실제 이동 처리 함수 (FixedUpdate에서 사용)
    /// </summary>
    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDirection * transform.forward);
    }

    /// <summary>
    /// 실제 회전 처리 함수 (FixedUpdate에서 사용)
    /// </summary>
    void Rotate()
    {
        //transform.Rotate
        //rigid.MoveRotation
        //transform.rotation

        // 이번 FixedUpdate에서 추가로 회전할 회전 (delta)
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDirection, transform.up);

        // 현재 회전에서 rotate만큼 추가로 회전
        rigid.MoveRotation(rigid.rotation * rotate); // 현재 회전에서 rotate만큼 추가로 회전

        // 회전을 표현하는 클래스 : Quaternion
        // Quaternion.Euler() : x, y, z축으로 얼마만큼 회전시킬 것인지를 파라메터로 받아서 회전을 생성하는 함수
        // Quaternion.AngleAxis() : 특정 축을 기준으로 몇 도만큼 회전시킬지를 파라메터로 받아서 회전을 생성하는 함수
        // Quaternion.FromToRotation() : 시작 방향에서 도착 방향이 될 수 있는 회전을 생성하는 함수
        // Quaternion.Lerp() : 시작 회전에서 목표 회전으로 보간하는 함수
        // Quaternion.Slerp() : 시작 회전에서 목표 회전으로 보간하는 함수 (곡선으로 보간)
        // Quaternion.LookRotation() : 특정 방향을 바라보는 회전을 만들어주는 함수

        // Quaternion.identity; // 아무런 회전도 하지 않았다.
        // Quaternion.Inverse() : 역회전을 계산하는 함수

        // Quaternion.RotateTowards() : from에서 to로 회전시키는 함수. 한 번 실행될 때마다 maxDegreesDelta만큼 회전.

        //transform.rotation = Quaternion.identity;
        // transform.RotateAround : 특정 위치에서 특정 축을 기준으로 회전하기
    }

    /// <summary>
    /// 실제 점프 처리를 하는 함수
    /// </summary>
    void Jump()
    {
        if (IsJumpAvailable) // 점프가 가능할 때만 점프
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse); // 위쪽으로 jumpPower만큼 힘을 더하기
            jumpCoolRemains = jumpCoolTime; // 쿨타임 초기화
            //GroundCount = 0; // 모든 땅에서 떨어졌음
        }
    }

    /// <summary>
    /// 사망 처리용 함수
    /// </summary>
    public void Die()
    {
        if (isAlive)
        {
            Debug.Log("죽었음");

            /// 실습_240205
            /// 죽는 애니메이션이 나온다.
            /// 더 이상 조종이 안되어야 한다.
            /// 대굴대굴 구른다. (뒤로 넘어가면서 y축으로 스핀을 먹는다.)
            /// 죽었다고 신호 보내기 (onDie 델리게이트 실행)

            animator.SetTrigger(DieHash); ///// 죽는 애니메이션

            inputActions.Player.Disable(); // Player 액션맵을 비활성화 ///// 조종 불가 상태

            rigid.constraints = RigidbodyConstraints.None; // 물리 잠금을 전부 해제하기
            Transform head = transform.GetChild(0); ///// Player의 머리
            rigid.AddForceAtPosition(-transform.forward, head.position, ForceMode.Impulse); ///// 특정 위치에 힘을 가해준다.
            rigid.AddTorque(transform.up * 1.5f, ForceMode.Impulse);

            onDie?.Invoke(); ///// 델리게이트

            isAlive = false;
        }
    }

    /// <summary>
    /// 이동 속도 증감용 함수
    /// </summary>
    /// <param name="ratio">원본에서의 증감 비율</param>
    public void SetSpeedModifier(float ratio = 1.0f)
    {
        currentMoveSpeed = moveSpeed * ratio;
    }

    /// <summary>
    /// 원래 기준 속도로 복구하는 함수
    /// </summary>
    public void RestoreMoveSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }
}

/// 실습_240129
/// 1. 점프 중에 점프가 되지 않아야 한다.
/// 2. 점프 쿨타임이 있어야 한다.
