using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // 플레이어를 이동시킬 위치
    public Vector2 targetPosition = new Vector2(197.84f, 3.23f);

    // 트리거에 다른 오브젝트가 들어왔을 때 자동으로 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 들어온 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            // 플레이어의 위치를 targetPosition으로 변경
            other.transform.position = targetPosition;
        }
    }
}
