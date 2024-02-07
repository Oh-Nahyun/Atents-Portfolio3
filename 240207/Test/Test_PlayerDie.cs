using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerDie : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Time.timeScale = 0.1f; ///// 속도 늦추기
        GameManager.Instance.Player.Die();
    }
}
