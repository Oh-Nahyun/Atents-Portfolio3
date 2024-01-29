using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    TurretBullet
}

public class Factory : Singleton<Factory>
{
    BulletPool bulletOld;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletOld = GetComponentInChildren<BulletPool>();
        if (bulletOld != null)
            bulletOld.Initialize();
    }

    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;

        switch (type)
        {
            case PoolObjectType.TurretBullet:
                result = bulletOld.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }

    public BulletOld GetBulletOld()
    {
        return bulletOld.GetObject();
    }

    public BulletOld GetBulletOld(Vector3 position, float angle = 0.0f)
    {
        return bulletOld.GetObject(position, angle * Vector3.forward);
    }
}

/// 실습_240129
/// 팩토리 만들기
/// - 이전 프로젝트의 팩토리를 무사히 이식하기
