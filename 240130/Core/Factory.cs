using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Bullet = 0,
}

public class Factory : Singleton<Factory>
{
    BulletPool bulletPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletPool = GetComponentInChildren<BulletPool>();
        if (bulletPool != null)
            bulletPool.Initialize();
    }

    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="euler">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;

        switch (type)
        {
            case PoolObjectType.Bullet:
                result = bulletPool.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }

    public Bullet GetBulletOld()
    {
        return bulletPool.GetObject();
    }

    public Bullet GetBulletOld(Vector3 position, float angle = 0.0f)
    {
        return bulletPool.GetObject(position, angle * Vector3.forward);
    }
}

/// 실습_240129
/// 팩토리 만들기
/// - 이전 프로젝트의 팩토리를 무사히 이식하기
