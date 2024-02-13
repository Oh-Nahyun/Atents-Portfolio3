using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStandard : TurretBase
{
    //private void Awake()
    //{
    //    Transform child = transform.GetChild(2);
    //    fireTransform = child.GetChild(1);
    //}

    private void Start()
    {
        StartCoroutine(fireCoroutine);
    }
}
