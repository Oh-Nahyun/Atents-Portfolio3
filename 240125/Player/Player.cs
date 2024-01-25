using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/////[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    /// 실습_240125
    /// 1. 플레이어는 WS키로 전진/후진을 한다.
    /// 2. 플레이어는 AD키로 좌회전/우회전을 한다.
    /// 3. 플레이어가 움직이면(전진/후진/좌회전/우회전) Player_Move 애니메이션이 재생된다.
    /// 4. 이동 입력이 없으면 Player_Idle 애니메이션이 재생된다.
    /// 5. Player_Move 애니메이션은 팔 다리가 앞뒤로 흔들린다.
    /// 6. Player_Idle 애니메이션은 머리가 살짝 앞뒤로 까딱거린다.

    /* // 내가 작성한 코드
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions(); // 인풋 액션 생성
    }

    private void OnEnable()
    {
        inputActions.Player.Enable(); // 활성화될 때 Player 액션맵을 활성화
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable(); // Player 액션맵을 비활성화
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("OnMove : 눌러짐");
        }

        if (context.canceled)
        {
            Debug.Log("OnMove : 떨어짐");
        }
    }
    */
}
