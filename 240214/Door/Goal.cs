using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    /// 실습_240214
    /// 플레이어가 트리거 안에 들어오면 클리어

    /// 클리어 시
    /// 골대는 폭죽 모두 터트리기
    /// 플레이어는 수명 정지, 입력 정지
    /// GameClearPanel를 이용해서 클리어 화면 띄우기

    /// 게임오버 시 (플레이어가 죽었을 때)
    /// GameOverPanel를 이용해서 게임오버 화면 띄우기

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GameClear();
            Debug.Log("Game_Clear!");
        }
    }
}
