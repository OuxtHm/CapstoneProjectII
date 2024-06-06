using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private Transform[] backgrounds; // 두 개 이상의 배경을 배열로 관리
    [SerializeField]
    private float scrollAmount; // 배경의 너비
    [SerializeField]
    private Vector3 moveDirection; // 이동 방향

    private Transform target;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        foreach (Transform background in backgrounds)
        {
            // 플레이어가 배경을 앞쪽으로 지나갈 때
            if (target.position.x >= background.position.x + scrollAmount)
            {
                background.position += moveDirection * (backgrounds.Length * scrollAmount);
            }

            // 플레이어가 배경을 뒤쪽으로 지나갈 때
            if (target.position.x <= background.position.x - scrollAmount)
            {
                background.position -= moveDirection * (backgrounds.Length * scrollAmount);
            }
        }
    }
}