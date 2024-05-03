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
                // ������ ���ϱ�
                // ��ų ä������ �����ְ� 1�ʵ� â �ݱ�
                // ���� ��ų UI�� �����ϱ�
                // ���� â ���� �� ������Ʈ�� ���� ������ ���� �� �����ϱ�
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

    void MoneyUpdate()      // �÷��̾� ����ؽ�Ʈ ������Ʈ
    {
        money_txt.text = player.money.ToString();
    }

    public void OffWindow()     // â �ݱ�
    {
        shop.uiOpen = false;
        Destroy(this.gameObject);
    }

    public void Sell()      // ��ǰ �Ǹ�
    {
        player.money -= price;
        MoneyUpdate();
        skillSlotUi.SetActive(true);
    }
 
    public void Ask(int num)        // ���� ���θ� ���� UI ����
    {
        checkingObj.SetActive(true);
        selectSkillNum = num;
    }
}
