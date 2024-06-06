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
    public Button skillTabBtn;     // ��ų ���� �� Ȱ��ȭ ��ư
    public Button enhanceTabBtn;       // ��ȭ �� Ȱ��ȭ ��ư
    public GameObject skillTabPage;     // ��ų ���� ȭ��
    public GameObject enhanceTabPage;       // ��ȭ ���� ȭ��
    public Button[] btn = new Button[4];        // ��ų ���� ��ư
    public Button[] btnEnhance = new Button[2]; // ��ų ��ȭ ��ư
    public GameObject checkingObj;
    public GameObject skillSlotUi;      // ��ų ���� UI
    public Button purchaseY;
    public Button[] slot = new Button[2];
    Transform[] playerSkillSlot = new Transform[2];
    public int price;
    private int selectSkillNum;
    public AudioClip clickSounds;      // ��ư Ŭ�� ����
    private List<int> purchasedSkills = new List<int>(); // ������ ��ų �ε��� ����
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

            // ���� ��ų ��� ��ư ���
            btn[i].onClick.AddListener(() =>
            {
                if (purchasedSkills.Contains(index)) // �̹� ������ ��ų���� Ȯ��
                {
                    Debug.Log("�̹� ������ ��ų�Դϴ�."); // �Ǵ� ����ڿ��� �˸��� ǥ��
                    return; // ���⼭ �Լ��� �����Ͽ� �� �̻� �������� ����
                }

                sm.SFXPlay(clickSounds);
                price = randSkill.skillCon[index].price;
                Ask(index);
            });
        }

        for (int i = 0; i < slot.Length; i++)
        {
            int index = i;
            
            // ������ ��ų�� ���� ���� ���ϴ� ��ư ���
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

        // ��ų ���� â���� �̵��ϴ� ��ư ��� 
        skillTabBtn.onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            skillTabPage.SetActive(true);
            enhanceTabPage.SetActive(false);
        });

        // ��ų ��ȭ â���� �̵��ϴ� ��ư ���
        enhanceTabBtn.onClick.AddListener(() =>
        {
            sm.SFXPlay(clickSounds);
            skillTabPage.SetActive(false);
            enhanceTabPage.SetActive(true);
        });

        MoneyUpdate();
    }
    void CreateSkillUI(int num, GameObject icon)        // ���� Skill UI�� ������ �����ϱ� 
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
    void MoneyUpdate()      // �÷��̾� ����ؽ�Ʈ ������Ʈ
    {
        money_txt.text = player.money.ToString();
    }

    public void OffWindow()     // â �ݱ�
    {
        shop.uiOpen = false;
        this.gameObject.SetActive(false);
    }

    public void Sell()      // ��ǰ �Ǹ�
    {
        player.money -= price;
        MoneyUpdate();
        skillSlotUi.SetActive(true);
        purchasedSkills.Add(selectSkillNum); // ������ ��ų �ε����� ����Ʈ�� �߰�
    }
    public void Ask(int num)        // ���� ���θ� ���� UI ����
    {
        checkingObj.SetActive(true);
        selectSkillNum = num;
    }
}
