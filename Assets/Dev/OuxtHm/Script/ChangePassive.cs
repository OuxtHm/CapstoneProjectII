using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePassive : MonoBehaviour
{
    public static ChangePassive instance;
    DataManager dm;
    SkillManager sm;
    public Image changeCoolTime;

    public Image passive_1;  // 패시브 오브젝트
    public Image passive_2;  // 패시브 오브젝트

    public Transform nowPassive;     // 사용 중인 패시브
    public Transform readyPassive;   // 대기 중인 패시브

    public SkillControler nowPassiveSkilltroler;
    public SkillControler readyPassiveSkilltroler;

    public bool change;     // 패시브 변경 했는지 확인하는 변수
    private void Awake()
    {
        instance = this;
        changeCoolTime = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        nowPassive = transform.GetChild(0).GetComponent<Transform>();     // 현재 사용중인 패시브 Transform 설정
        readyPassive = transform.GetChild(1).GetComponent<Transform>();   // 대기중인 패시브 Transform 설정
        change = false;
    }
    private void Start()
    {
        dm = DataManager.instance;
        sm = SkillManager.instance;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !change)
        {
            SwapPassive();
            StartCoroutine(ChangePassiveCoolTime(7f));
        }
    }

    public IEnumerator ChangePassiveCoolTime(float duration)      // 패시브 변경 쿨타임 적용
    {
        change = true;
        float elapsedTime = 0f; // 경과 시간
        changeCoolTime.fillAmount = 1;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            changeCoolTime.fillAmount = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        changeCoolTime.fillAmount = 0f;
        change = false;
    }

    public void SwapPassive()     // 패시브 이미지 위치 교환
    {
        nowPassive = transform.GetChild(0).GetComponent<Transform>();     // 현재 사용중인 패시브 Transform 설정
        readyPassive = transform.GetChild(1).GetComponent<Transform>();   // 대기중인 패시브 Transform 설정

        passive_1 = nowPassive.GetChild(0).GetComponent<Image>();       // 사용중인 패시브 Image 설정
        passive_2 = readyPassive.GetChild(0).GetComponent<Image>();     // 대기중인 패시브 Transform 설정

        passive_1.transform.SetParent(readyPassive.transform, false);   // 대기중인 패시브 사용중 슬롯으로 이동
        passive_1.transform.SetAsFirstSibling();                          // 부모 오브젝트의 첫번째 자식으로 설정

        passive_2.transform.SetParent(nowPassive.transform, false);     // 사용중인 패시브 대기중 슬롯으로 이동
        passive_2.transform.SetAsFirstSibling();                          // 부모 오브젝트의 첫번째 자식으로 설정

        GetPassiveComponent();
    }   
    public void GetPassiveComponent()
    {
        readyPassiveSkilltroler = passive_1.GetComponentInChildren<SkillControler>();
        nowPassiveSkilltroler = passive_2.GetComponentInChildren<SkillControler>();
        dm.skillData.nowPassive = nowPassiveSkilltroler.num;
        dm.skillData.readySkill = readyPassiveSkilltroler.num;

    }
}
