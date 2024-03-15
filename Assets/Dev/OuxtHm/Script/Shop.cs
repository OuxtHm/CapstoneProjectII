using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Player player;
    public TextMeshProUGUI money_txt;
    public Button colleagueTabBtn;     // ���� ���� �� Ȱ��ȭ ��ư
    public Button enhanceTabBtn;       // ��ȭ �� Ȱ��ȭ ��ư
    public GameObject colleagueTabPage;     // ���� ���� ȭ��
    public GameObject enhanceTabPage;       // ��ȭ ���� ȭ��
    public Button[] btn = new Button[4];        // ���� ���� ��ư
    public Button[] btnEnhance = new Button[2]; // ���� ��ȭ ��ư

    private void Awake()
    {
        colleagueTabPage = transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
        enhanceTabPage = transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
        money_txt = transform.GetChild(0).GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        colleagueTabBtn = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
        enhanceTabBtn = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i] = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(i).GetComponentInChildren<Button>();
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

    void MoneyUpdate()      // �÷��̾� ����ؽ�Ʈ ������Ʈ
    {
        money_txt.text = player.money.ToString();
    }





}
