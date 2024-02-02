using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    //public DoorBase target;

    public float rotateSpeed = 360.0f;

    Transform modelTransform;

    public Action onConsume;

    private void Awake()
    {
        modelTransform = transform.GetChild(0);
    }

    private void Update()
    {
        modelTransform.Rotate(Time.deltaTime * rotateSpeed * Vector3.up);
    }

    /*
    private void OnValidate()
    {
        ResetTarget(target);
    }

    void ResetTarget(DoorBase door)
    {
        if (door != null)
        {
            target = door; ///// 모든 문
            //target = door.GetComponent<DoorAuto>(); ///// 자동문만 사용 // 권장
            //target = door as DoorAuto; // door가 DoorAuto로 캐스팅 될 수 있으면 캐스팅하고 아니면 null

            // is : is 왼쪽에 있는 변수의 데이터 타입이 오른쪽에 있는 타입이나 그 타입을 상속받은 타입이면, true 아니면 false
            // as : as 왼쪽에 있는 변수의 데이터 타입이 오른쪽에 있는 타입이나 그 타입을 상속받은 타입이면,
            //      캐스팅을 해서 null이 아닌 값을 리턴하고, 아니면 null이다.
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnConsume();
        }
    }

    /// <summary>
    /// 이 열쇠를 획득했을 때 일어날 일을 처리하는 함수
    /// </summary>
    protected virtual void OnConsume()
    {
        //target.Open();
        onConsume?.Invoke();
        Destroy(this.gameObject);
    }

}

/// 실습_240202
/// 1. 문 열쇠 만들기
///     1.1. 이 오브젝트와 플레이어가 닿으면 target에 지정된 문이 열린다.
///     1.2. 이 오브젝트와 플레이어가 닿으면 이 오브젝트는 사라진다.
///     1.3. 이 오브젝트는 제자리에서 빙글빙글 돈다.
/// 
/// 2. 잠금해제용 문과 열쇠 만들기
///     2.1. DoorAutoLock : DoorAuto 상속 받은 클래스. 평소에는 잠겨있다가 이 문의 열쇠를 먹으면 문을 열 수 있다.
///     2.2. 잠금 상태와 해제 상태의 문 색상이 다르다.
///     2.3. UnlockDoorKey : 먹으면 연결된 DoorAutoLock의 잠금이 해제된다.
