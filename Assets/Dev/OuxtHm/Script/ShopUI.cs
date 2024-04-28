using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    Player player;
    Shop shop;
    public TextMeshProUGUI money_txt;
    public Button colleagueTabBtn;     // 동료 구매 탭 활성화 버튼
    public Button enhanceTabBtn;       // 강화 탭 활성화 버튼
    public GameObject colleagueTabPage;     // 동료 구매 화면
    public GameObject enhanceTabPage;       // 강화 구매 화면
    public Button[] btn = new Button[4];        // 동료 구매 버튼
    public Button[] btnEnhance = new Button[2]; // 동료 강화 버튼
    public GameObject checkingObj;

    public int testPrice;

    private void Awake()
    {
        shop = Shop.instance;
        colleagueTabPage = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
        enhanceTabPage = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
        money_txt = transform.GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        colleagueTabBtn = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
        enhanceTabBtn = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        checkingObj = transform.GetChild(1).gameObject;
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(i).GetComponentInChildren<Button>();
            btn[i].onClick.AddListener(() =>
            {
                checkingObj.SetActive(true);
            });
        }
        
        for(int i = 0; i < btnEnhance.Length; i++)
        {
            btnEnhance[i] = transform.GetChild(0).GetChild(0).GetChild(3).GetChild(i).GetChild(0).GetComponent<Button>();
        }
    }
    void Start()
    {
        player = Player.instance;
        colleagueTabBtn.onClick.AddListener(() =>
        {
            colleagueTabPage.SetActive(true);
            enhanceTabPage.SetActive(false);
        });
        enhanceTabBtn.onClick.AddListener(() =>
        {
            colleagueTabPage.SetActive(false);
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
        player.money -= testPrice;
        MoneyUpdate();
    }


}
