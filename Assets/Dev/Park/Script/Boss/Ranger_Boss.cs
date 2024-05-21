using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger_Boss : Boss
{
    public GameObject coinPrefab; // 코인 프리팹 참조를 위한 변수
    public GameObject potionPrefab; // 포션 프리팹 참조를 위한 변수
    public GameObject skillItemPrefab; // 스킬 아이템 프리팹 참조를 위한 변수

    public int numberOfCoins = 10; // 생성할 코인의 수
    public int numberOfPotions = 2; // 생성할 포션의 수
    public int numberOfSkillItems = 1; // 생성할 스킬 아이템의 수
    public float scatterRadius = 1.0f; // 아이템이 흩어지는 반경

    public override void BossInitSetting()
    {
        // 기존 설정 코드...
        boss_stage = 1;
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Speed = 3;
        boss_BumpPower = 5;
        boss_OnePattenPower = 10;   //근접 공격 패턴 대미지
        boss_TwoPattenPower = 20;   // 기본 활 패턴 대미지
        boss_ThreePattenPower = 20; // 화살비 패턴 대미지
        boss_FourPattenPower = 30;  // 레이져 패턴 대미지
    }

    // 보스가 죽었을 때 호출될 메서드
    public void OnDeath()
    {
        DropItems(coinPrefab, numberOfCoins);
        DropItems(potionPrefab, numberOfPotions);
        DropItems(skillItemPrefab, numberOfSkillItems);

        Debug.Log("아이템 떨구기");
    }

    void DropItems(GameObject itemPrefab, int numberOfItems)
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            Vector2 scatterOffset = Random.insideUnitCircle.normalized * scatterRadius * Random.Range(0.5f, 1.5f);
            Vector3 dropPosition = transform.position + new Vector3(scatterOffset.x, scatterOffset.y, 0);

            // 아이템을 인스턴스화
            GameObject droppedItem = Instantiate(itemPrefab, dropPosition, Quaternion.identity);

            // Rigidbody2D 컴포넌트를 동적으로 추가
            Rigidbody2D rb = droppedItem.AddComponent<Rigidbody2D>();
            // 필요하다면 다른 Rigidbody2D 설정을 조정할 수 있습니다
            // 예: rb.gravityScale = 1f;
        }
    }
}
