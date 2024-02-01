using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorManualAutoClose : DoorBase, IInteracable
{
    TextMeshPro text; // 3D 글자 (UI 아님)

    public float autoCloseTime = 3.0f;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMeshPro>(true);
    }

    public void Use()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Camera.main.
            Vector3 cameraToDoor = transform.position - Camera.main.transform.position; // 카메라에서 문으로 향하는 방향 벡터
            float angle = Vector3.Angle(transform.forward, cameraToDoor);
            if (angle > 90.0f) // 사이각이 90도보다 크면 카메라가 문 앞에 있다.
            {
                text.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0); // 문의 회전에서 y축으로 반바퀴 더 돌리기
            }
            else
            {
                text.transform.rotation = transform.rotation; // 문의 회전 그대로 적용
            }

            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}

/// 실습_240201
/// 1. 일정 시간 이후에 자동으로 문이 닫히기
/// 2. 플레이어가 자신의 트리거 안에 들어오면 글자로 단축키 보여주기
