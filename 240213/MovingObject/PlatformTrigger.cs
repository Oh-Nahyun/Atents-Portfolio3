using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : PlatformBase
{
    /* // 내가 짠 코드_성공
    protected override void OnArrived()
    {
        Target = targetWaypoints.CurrentWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = targetWaypoints.GetNextWaypoint();
            OnMove();
        }
    }
    */

    /// <summary>
    /// 플랫폼이 움직일지 멈출지를 결정하는 변수
    /// </summary>
    bool isMoving = false;

    private void Start()
    {
        Target = targetWaypoints.GetNextWaypoint(); // 시작했을 때 플랫폼이 안움직이는 문제 해결용 (올라가자마자 도착하는 문제)
    }

    protected override void OnMove()
    {
        if (isMoving) // isMoving이 true일 때만 움직임
        {
            base.OnMove();
        }
    }

    protected override void OnArrived()
    {
        isMoving = false; // 도착하면 멈추기
        base.OnArrived();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMoving = true; // 플레이어가 올라오면 이동하기
        }
    }
}
