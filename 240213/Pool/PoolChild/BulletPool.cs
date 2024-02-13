using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : ObjectPool<Bullet>
{
    protected override void OnGetObject(Bullet component)
    {
        //component.SetVelocity();
    }
}
