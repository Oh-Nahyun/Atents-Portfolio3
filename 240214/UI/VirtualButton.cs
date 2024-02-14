using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualButton : MonoBehaviour, IPointerClickHandler
{
    /// 실습_240214
    /// 이미지를 누르면 점프를 한다.
    /// 점프 쿨타임이 표시된다.

    Image coolDown;

    public Action onClick;

    void Awake()
    {
        Transform child = transform.GetChild(1);
        coolDown = child.GetComponent<Image>();
        coolDown.fillAmount = 0.0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void RefreshCoolTime(float ratio)
    {
        coolDown.fillAmount = ratio;
    }

    public void Stop()
    {
        onClick = null;
        coolDown.fillAmount = 1.0f;
    }
}
