using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    // TurretStandard와 TurretTrace의 공통 부모

    /*
    /// <summary>
    /// 터렛이 발사할 총알 종류
    /// </summary>
    public PoolObjectType bulletType = PoolObjectType.Bullet;

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 1.0f;

    /// <summary>
    /// 총알 발사 위치 설정용 트랜스폼
    /// </summary>
    Transform fireTransform;

    IEnumerator PeriodFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            Factory.Instance.GetObject(bulletType, fireTransform.position, fireTransform.rotation.eulerAngles);
        }
    }
    */
}
