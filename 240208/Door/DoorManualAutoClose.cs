using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorManualAutoClose : DoorManual
{
    public float autoCloseTime = 3.0f;

    public new void Use() // 함수에 new 키워드가 붙으면 부모쪽의 함수를 무시한다.
    {
        Open();
        StopAllCoroutines();
        StartCoroutine(AutoClose());
    }

    IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(autoCloseTime);
        Close();
    }
}

/// 실습_240201
/// 1. 일정 시간 이후에 자동으로 문이 닫히기
/// 2. 플레이어가 자신의 트리거 안에 들어오면 글자로 단축키 보여주기
