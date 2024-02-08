using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인터페이스 : 어떤 클래스가 반드시 이런 기능(함수)를 가지고 있다고 명시해 놓는 것
// - 기본적으로 모든 멤버가 public
// - 인터페이스를 상속받은 클래스는 반드시 인터페이스의 멤버를 구현해야 한다.
// - 인터페이스는 멤버 변수는 없다. (const는 가능)
// - 인터페이스는 멤버 함수의 선언만 있다. (구현은 없다.)
// - 인터페이스는 상속 개수 제한이 없다.

public interface IInteracable
{
    bool CanUse // 사용 가능한지를 확인하는 프로퍼티
    {
        get;
    }

    void Use(); // 사용하는 기능이 있다고 선언해 놓은 것
}
