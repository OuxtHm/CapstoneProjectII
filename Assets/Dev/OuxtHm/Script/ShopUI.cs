using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    Player player;
    Shop shop;
    RandomSkillShop randSkill;
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
    public int price;
    private int selectSkillNum;

    private void Awake()
    {

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
        shop = Shop.instance;
        randSkill = RandomSkillShop.instance;
        purchaseY.onClick.AddListener(() =>
        {
            Sell();
            checkingObj.SetActive(false);
        });
        for (int i = 0; i < btn.Length; i++)
        {
            int index = i;
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(i).GetComponentInChildren<Button>();
            btn[i].onClick.AddListener(() =>
            {
                price = randSkill.skillCon[index].price;
                Ask(index);
            });
        }

        for(int i = 0; i < slot.Length; i++)
        {
            int index = i;
            slot[i].onClick.AddListener(() =>
            {
                Debug.Log(index);
                Instantiate(randSkill.skillCon[selectSkillNum], slot[index].transform);
                // 사이즈 정하기
                // 스킬 채워진거 보여주고 1초뒤 창 닫기
                // 실제 스킬 UI에 적용하기
                // 상점 창 닫을 때 오브젝트만 끄고 상점맵 나갈 때 삭제하기
            });
        }

        for (int i = 0; i < btnEnhance.Length; i++)
        {
            btnEnhance[i] = transform.GetChild(0).GetChild(0).GetChild(3).GetChild(i).GetChild(0).GetComponent<Button>();
        }

        skillTabBtn.onClick.AddListener(() =>
        {
            skillTabPage.SetActive(true);
            enhanceTabPage.SetActive(false);
        });
        enhanceTabBtn.onClick.AddListener(() =>
        {
            skillTabPage.SetActive(false);
            enhanceTabPage.SetActive(true);
        });

        MoneyUpdate();
    }

    void MoneyUpdate()      // 플레이어 골드텍스트 업데이트
    {
        money_txt.text = player.money.ToString();
    }

    public void OffWindow()     // 창 닫기
    {
        shop.uiOpen = false;
        Destroy(this.gameObject);
    }

    public void Sell()      // 물품 판매
    {
        player.money -= price;
        MoneyUpdate();
        skillSlotUi.SetActive(true);
    }
 
    public void Ask(int num)        // 구매 여부를 묻는 UI 생성
    {
        checkingObj.SetActive(true);
        selectSkillNum = num;
    }
}
