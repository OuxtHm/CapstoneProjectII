using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger_Boss : Boss
{
    private bool isDying = false; // 보스가 죽는 중인지 확인하는 플래그
    public override void BossInitSetting()
    {
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
    void Update()
    {

        // boss_CurHP가 0 이하이고, 아직 죽는 중이 아니라면 Die 코루틴을 시작합니다.
        if (boss_CurHP <= 0 && !isDying)
        {
            StartCoroutine(pt1());
        }
        else
        {
            StageManager.instance.ExitPortal1();
        }
    }//5/31 이경규 추가

    IEnumerator pt1()
    {
        isDying = true; // 죽는 중으로 상태 변경

        StageManager.instance.ActivatePortal1();

        yield return new WaitForSeconds(3.0f); // 필요에 따라 대기 시간 조정

        isDying = false; // 죽음 처리 완료
    }//5/31 이경규 추가
}