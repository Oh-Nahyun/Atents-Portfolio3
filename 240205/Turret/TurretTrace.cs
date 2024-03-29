using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : TurretBase
{
    /// <summary>
    /// 시야 범위
    /// </summary>
    public float sightRange = 10.0f;

    /// <summary>
    /// 터렛의 머리가 돌아가는 속도
    /// </summary>
    public float turnSpeed = 2.0f;

    /// <summary>
    /// 터렛이 총알을 발사를 시작하는 좌우 발사각 (10이면, +-10도)
    /// </summary>
    public float fireAngle = 10.0f;

    /// <summary>
    /// 시야 범위 체크용 트리거
    /// </summary>
    SphereCollider sightTrigger;

    /// <summary>
    /// 내 시야 범위 안에 들어오는 플레이어
    /// </summary>
    Player target;

    /// <summary>
    /// 발사 중인지 아닌지 표시하는 변수 (true면 발사 중)
    /// </summary>
    bool isFiring = false;

#if UNITY_EDITOR
    /// <summary>
    /// 내 공격 영역 안에 플레이어가 있고 발사각 안에 플레이어가 있는 상태인지 아닌지 확인하기 위한 프로퍼티
    /// </summary>
    bool IsRedState => isFiring;

    /// <summary>
    /// 내 공격 영역 안에 플레이어가 있는 상태인지 아닌지 확인하기 위한 프로퍼티
    /// </summary>
    bool IsOrangeState => isTargetVisible;

    /// <summary>
    /// 플레이어가 보이는지 아닌지 표시해놓은 변수 (true면 무조건 target이 설정되어있다.)
    /// </summary>
    bool isTargetVisible = false;
#endif

    protected override void Awake()
    {
        base.Awake(); // base : 부모 클래스의 인스턴스에 접근하는 참조
        sightTrigger = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        sightTrigger.radius = sightRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            target = GameManager.Instance.Player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            target = null;
        }
    }

    private void Update()
    {
        LookTargetAndAttack();
    }

    private void LookTargetAndAttack()
    {
        bool isStartFire = false;
        if (target != null) // 플레이어가 내 트리거 안에 들어왔다.
        {
            Vector3 dir = target.transform.position - transform.position; ///// 방향 벡터 // 터렛에서 플레이어로 가는 방향 계산
            dir.y = 0.0f;

            if (IsVisibleTarget(dir)) // 장애물이 있는지 없는지 확인
            {
                // 장애물이 없이 플레이어가 보이면 그 쪽으로 방향을 돌린다.
                //transform.LookAt()
                //barrelBody.forward = dir; // 즉시 바라보기
                barrelBody.rotation = Quaternion.Slerp(barrelBody.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed); ///// 천천히 목표 지점 바라보기

                //Vector3.SignedAngle : 두 벡터의 사이각을 구하는데 방향을 고려하여 계산한다.
                float angle = Vector3.Angle(barrelBody.forward, dir); ///// 사잇각
                if (angle < fireAngle) ///// 발사각 안인지 밖인지 확인
                {
                    isStartFire = true; // 발사 결정
                }
            }
        }
#if UNITY_EDITOR ///// 전처리기. 성능에 아무 영향을 주지 않음.
        else
        {
            isTargetVisible = false;
        }
#endif

        if (isStartFire) // 발사해야 하는 상황인지 확인
        {
            StartFire(); // 발사 시작
        }
        else
        {
            StopFire(); // 발사 정지
        }
    }

    /// <summary>
    /// Target이 보이는지 확인하는 함수
    /// </summary>
    /// <param name="lookDirection">바라보는 방향</param>
    /// <returns>true면 target이 보인다. false면 target이 보이지 않는다.</returns>
    private bool IsVisibleTarget(Vector3 lookDirection)
    {
        bool result = false;

        Ray ray = new Ray(barrelBody.position, lookDirection);

        // out : 출력용 파라메터라고 알려주는 키워드. 무조건 함수가 실행되었을 때, 초기화 된다.
        //int layerMask = LayerMask.GetMask("Deufalt", "Player"); // RayCast를 할 때, 특정 레이어만 체크하고 싶을 때 사용

        if (Physics.Raycast(ray, out RaycastHit  hitInfo, sightRange)) ///// 마지막에 ', layerMask'를 붙이면 특정 레이어만 체크 가능
        {
            if (hitInfo.transform == target.transform)
            {
                result = true;
            }
        }

#if UNITY_EDITOR
        isTargetVisible = result;
#endif

        return result;
    }

    /// <summary>
    /// 총알을 발사하기 시작 (중복 실행 없음)
    /// </summary>
    void StartFire()
    {
        if (!isFiring)
        {
            StartCoroutine(fireCoroutine);
            isFiring = true;
        }
    }

    /// <summary>
    /// 총알 발사를 정지
    /// </summary>
    void StopFire()
    {
        if (isFiring)
        {
            StopCoroutine(fireCoroutine);
            isFiring = false;
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // sightRange 범위 그리기
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 3.0f);

        if (barrelBody == null)
        {
            barrelBody = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + barrelBody.forward * sightRange;

        // 중심선 그리기
        Handles.color = Color.yellow;
        Handles.DrawDottedLine(from, to, 2.0f);

        // 시야각 내부 그리기
        // 평소(녹색), 플레이어 쪽으로 머리 돌리는 중(주황색), 플레이어를 공격하는 상황(빨간색)
        // 녹색 : 내 공격 영역 안에 플레이어가 없는 상태
        // 주황색 : 내 공격 영역 안에 플레이어가 있는 상태
        // 빨간색 : 내 공격 영역 안에 플레이어가 있고 발사각 안에 플레이어가 있는 상태
        Handles.color = Color.green;
        if (IsRedState)
        {
            Handles.color = Color.red;
        }

        else if (IsOrangeState)
        {
            Handles.color = new Color(1.0f, 0.5f, 0.0f);
        }

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * barrelBody.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * barrelBody.forward;

        // 시야각의 가장자리 선 그리기
        to = transform.position + dir1 * sightRange;
        Handles.DrawLine(from, to, 3.0f);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from, to, 3.0f);

        // 시야각의 호 그리기
        Handles.DrawWireArc(from, transform.up, dir1, fireAngle * 2.0f, sightRange, 3.0f);
    }
#endif
}

/// 실습_240130
/// 추적용 터렛 만들기
/// 1. 플레이어가 터렛으로부터 일정 거리 안에 있으면 플레이어 쪽으로 BarrelBody가 회전한다. (플레이어를 바라보기, y축으로만 회전)
/// 2. 플레이어가 터렛의 발사각 안에 있으면 총알을 주기적으로 발사한다.
/// 3. 플레이어가 터렛의 발사각 안에 없으면 총알 발사를 정지한다.
/// 4. Gizmos를 통해 시야 범위와 발사각을 그린다. (Handles 추천)
