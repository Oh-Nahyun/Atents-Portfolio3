using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Door : TestBase
{
    public TextMeshPro text;
    public DoorBase door;
    public DoorSwitch testSwitch;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        float angle = Vector3.SignedAngle(door.transform.forward, cameraForward, Vector3.up);
        Debug.Log(angle);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        float angle = Vector3.Angle(door.transform.forward, cameraForward);
        Debug.Log(angle);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        IInteracable inter = door.GetComponent<IInteracable>();
        inter.Use();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        IInteracable inter = testSwitch.GetComponent<IInteracable>();
        inter.Use();
    }

    /// 실습_240131
    /// 1. 미닫이 문 프리팹 만들기 (Door_Sliding_Auto)
    /// 2. 한쪽 방향에서만 열리는 여닫이문 만들기 (프론트 쪽에 서있을 때만 열리기, 코드로 작성)
}
