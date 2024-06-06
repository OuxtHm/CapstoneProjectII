using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;
    SoundManager sm;
    Player player;
    Shop shop;
    RandomSkillShop randSkill;
    SkillUI skillUI;
    public TextMeshProUGUI money_txt;
    public GameObject grandObject;
    public Button skillTabBtn;     // 스킬 구매 탭 활성화 버튼
    public Button enhanceTabBtn;       // 강화 탭 활성화 버튼
    public GameObject skillTabPage;     // 스킬 구매 화면
    public GameObject enhanceTabPage;       // 강화 구매 화면
    public Button[] btn = new Button[4];        // 스킬 구매 버튼
    public Button[] btnEnhance = new Button[2]; // 스킬 강화 버튼
    public GameObject checkingObj;
    public GameObject skillSlotUi;      // 스킬 슬롯 UI
    public Button purchaseY;
    public Button[] slot = new Button[2];
    Transform[] playerSkillSlot = new Transform[2];
    public int price;
    private int selectSkillNum;
    public AudioClip clickSounds;      // 버튼 클릭 사운드
    private List<int> purchasedSkills = new List<int>(); // 구매한 스킬 인덱스 저장
    private void Awake()
    {
        instance = this;
        skillTabPage = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
        enhanceTabPage = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
        money_txt = transform.GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        skillTabBtn = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
        enhanceTabBtn = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        checkingObj = transform.GetChild(1).gameObject;
        skillSlotUi = transform.GetChild(2).gameObject;
        purchaseY = checkingObj.transform.GetChild(0).GetComponent<Button>();
        for(int i = 0; i < 2; i++)
        {
            slot[i] = skillSlotUi.transform.GetChild(i).GetComponentInChildren<Button>();
        }
    }
    void Start()
    {
        player = Player.instance;
        sm = SoundManager.instance;
        shop = Shop.instance;
        randSkill = RandomSkillShop.instance;
        skillUI = SkillUI.instance;
        for (int i = 0; i < 2; i++)
        {
            playerSkillSlot[i] = skillUI.transform.GetChild(1).GetChild(i + 1).GetComponent<Transform>();
        }

        purchaseY.onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            Sell();
            checkingObj.SetActive(false);
        });
        for (int i = 0; i < btn.Length; i++)
        {
            int index = i;
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(i).GetComponentInChildren<Button>();

            // 상점 스킬 목록 버튼 기능
            btn[i].onClick.AddListener(() =>
            {
                if (purchasedSkills.Contains(index)) // 이미 구매한 스킬인지 확인
                {
                    Debug.Log("이미 구매한 스킬입니다."); // 또는 사용자에게 알림을 표시
                    return; // 여기서 함수를 종료하여 더 이상 진행하지 않음
                }

                sm.SFXPlay(clickSounds);
                price = randSkill.skillCon[index].price;
                Ask(index);
            });
        }

        for (int i = 0; i < slot.Length; i++)
        {
            int index = i;
            
            // 구매한 스킬을 넣을 슬롯 정하는 버튼 기능
            slot[i].onClick.AddListener(() =>
            {
                sm.SFXPlay(clickSounds);
                GameObject skillIcon = Instantiate(randSkill.skillCon[selectSkillNum].gameObject, slot[index].transform);
                RectTransform iconRect = skillIcon.GetComponent<RectTransform>();
                iconRect.sizeDelta = new Vector2(80, 80);
                skillSlotUi.gameObject.SetActive(false);
                CreateSkillUI(index, skillIcon);
            });
        }

        for (int i = 0; i < btnEnhance.Length; i++)
        {
            btnEnhance[i] = transform.GetChild(0).GetChild(0).GetChild(3).GetChild(i).GetChild(0).GetComponent<Button>();
        }

        // 스킬 구매 창으로 이동하는 버튼 기능 
        skillTabBtn.onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            skillTabPage.SetActive(true);
            enhanceTabPage.SetActive(false);
        });

        // 스킬 강화 창으로 이동하는 버튼 기능
        enhanceTabBtn.onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            skillTabPage.SetActive(false);
            enhanceTabPage.SetActive(true);
        });

        MoneyUpdate();
    }
    void CreateSkillUI(int num, GameObject icon)        // 실제 Skill UI에 아이콘 적용하기 
    {
        GameObject orignSkill = playerSkillSlot[num].GetChild(0).gameObject;
        Destroy(orignSkill);
        GameObject newSkill = Instantiate(icon, playerSkillSlot[num]);
        newSkill.transform.SetSiblingIndex(0);
        if(num == 0)
        {
            skillUI.GetSkillComponent();
        }
        else
        {
            skillUI.GetUltComponent();
        }
        
    }
    void MoneyUpdate()      // 플레이어 골드텍스트 업데이트
    {
        money_txt.text = player.money.ToString();
    }

    public void OffWindow()     // 창 닫기
    {
        shop.uiOpen = false;
        this.gameObject.SetActive(false);
    }

    public void Sell()      // 물품 판매
    {
        player.money -= price;
        MoneyUpdate();
        skillSlotUi.SetActive(true);
        purchasedSkills.Add(selectSkillNum); // 구매한 스킬 인덱스를 리스트에 추가
    }
    public void Ask(int num)        // 구매 여부를 묻는 UI 생성
    {
        checkingObj.SetActive(true);
        selectSkillNum = num;
    }
}
