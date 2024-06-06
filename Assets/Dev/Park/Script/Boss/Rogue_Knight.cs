using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue_Knight : Boss
{
    private bool isDying = false; // 보스가 죽는 중인지 확인하는 플래그
    public override void BossInitSetting()
    {
        boss_stage = 2;
        boss_MaxHP = 100;
        boss_CurHP = 100;
        boss_Speed = 5;
        boss_BumpPower = 10;    //충돌 대미지
        boss_OnePattenPower = 20;   // 투명 순간이동 근접공격 패턴 대미지
        boss_TwoPattenPower = 30;   //  가로베기 패턴 대미지
        boss_ThreePattenPower = 20;    //   라이트 불꽃 패턴 대미지
        boss_FourPattenPower = 40;   //
    }

    void Update()
    {
        // boss_CurHP가 0 이하이고, 아직 죽는 중이 아니라면 Die 코루틴을 시작합니다.
        if (boss_CurHP <= 0 && !isDying)
        {
            StartCoroutine(pt2());
        }
    }//5/31 이경규 추가

    IEnumerator pt2()
    {
        isDying = true; // 죽는 중으로 상태 변경

        StageManager.instance.ActivatePortal2();

        yield return new WaitForSeconds(3.0f); // 필요에 따라 대기 시간 조정

        isDying = false; // 죽음 처리 완료
    }//5/31 이경규 추가
}
