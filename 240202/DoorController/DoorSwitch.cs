using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 수동문을 열고 닫는 스위치
/// </summary>
public class DoorSwitch : MonoBehaviour, IInteracable
{
    /// <summary>
    /// 스위치의 상태
    /// </summary>
    enum State
    {
        Off = 0, // 스위치가 꺼진 상태
        On // 스위치가 켜진 상태
    }

    /// <summary>
    /// 스위치의 현재 상태
    /// </summary>
    State state = State.Off;

    /// <summary>
    /// 스위치가 조작할 문을 가지고 있는 게임 오브젝트
    /// </summary>
    //public GameObject target;

    //IInteracable useTarget;

    /// <summary>
    /// target이 가지고 있는 DoorManual
    /// </summary>
    public DoorManual targetDoor;

    //bool isUsing = false;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 애니메이터용 해시
    /// </summary>
    readonly int SwitchOnHash = Animator.StringToHash("SwitchOn");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        //if (target != null)
        //{
            //useTarget = target.GetComponent<IInteracable>();
        //    targetDoor = target.GetComponent<DoorManual>(); // target에서 문 찾기
        //}

        if (targetDoor == null)
        {
            Debug.LogWarning($"{gameObject.name}에게 사용할 문이 없습니다."); // 문이 없으면 경고 출력
        }
    }

    /// <summary>
    /// 스위치 사용
    /// </summary>
    public void Use()
    {
        if (targetDoor != null) // 조작할 문이 있어야 한다.
        {
            switch (state)
            {
                case State.Off:
                    // 스위치를 켜는 상황
                    //useTarget.Use();
                    targetDoor.Open(); // 문 열고
                    animator.SetBool(SwitchOnHash, true); // 스위치 애니메이션 재생
                    state = State.On; // 상태 변경
                    break;
                case State.On:
                    // 스위치를 끄려는 상황
                    //useTarget.Use();
                    targetDoor.Close(); // 문 닫고
                    animator.SetBool(SwitchOnHash, false); // 스위치 애니메이션 재생
                    state = State.Off; // 상태 변경
                    break;
            }

            //if (!isUsing)
            //{
            //    useTarget.Use();
            //    StartCoroutine(ResetSwitch());
            //}
        }
    }

    /*
    IEnumerator ResetSwitch()
    {
        isUsing = true;
        animator.SetBool("IsOpen", true);
        float aniTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(aniTime);
        animator.SetBool("IsOpen", false);
        isUsing = false;
    }
    */
}

/// 실습_240201
/// 1. DoorManual 새로 만들기
///     1.1. 열렸을 때 사용하면 닫힌다.
///     1.2. 닫혔을 때 사용하면 열린다.
/// 2. DoorSwitch 수정하기
///     2.1. 3개의 상태를 가진다. (idle, on, off)
///     2.2. 사용할 문은 무조건 Manual 계열의 문만 가능하다.
///     2.3. on이 될 때 문이 열려야 한다.
///     2.4. off가 될 때 문이 닫힌다. (autoClose 문은 시간이 되면 자동으로 닫힌다. 시간 다 되기 전에 off가 되면 즉시 닫힌다.)
