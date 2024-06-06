using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    StageManager stageManager;
    SoundManager soundManager;
    MainCam maincam;
    public static Teleport Instance;  
    DataManager dm;
    public GameObject targetObj;
    public pade fadeScript; // 'pade'�� �ùٸ� Ŭ���� �̸����� Ȯ���ϼ���. �Ϲ������� Ŭ���� �̸��� �빮�ڷ� �����մϴ�.
    public GameObject toObj;
    StageUI stageUi;
    Animator animator;
    public int stageNumber; // ��Ż�� ���� �������� ��ȣ
    public bool isActive = false; // ��Ż�� Ȱ��ȭ �������� �����ϴ� �ʵ�
    public bool isTelepo = false;
    GameObject[] bossScene = new GameObject[3];
    public GameObject keyX;
    bool isbgm = false; //bgm�� �����ϰ� �ִ��� Ȯ��

    private void Awake()
    {
        Instance = this;
        keyX = this.transform.GetChild(0).gameObject;

        bossScene[0] = Resources.Load<GameObject>("Prefabs/LeafBossShow_canvas");
        bossScene[1] = Resources.Load<GameObject>("Prefabs/ShadowBossShow_canvas");
        bossScene[1] = Resources.Load<GameObject>("Prefabs/DevilBossShow_canvas");
    }
    private void Start()
    {
        stageUi = StageUI.instance;
        stageManager = StageManager.instance;
        soundManager = SoundManager.instance;
        animator = GetComponent<Animator>();
        stageBGM();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (animator != null) // Animator ������Ʈ�� �����ϴ��� Ȯ��
            {
                animator.SetBool("gate", true);
            }
            keyX.SetActive(true);
            targetObj = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // isTelepo�� false�� ���� ��Ż ��� ����
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.X) && !isTelepo)
        {
            maincam = MainCam.instance.GetComponent<MainCam>();
            StartCoroutine(TelepotyRoutine());
        }
    }



    IEnumerator TelepotyRoutine()
    {
        DestroyShopUi();

        isTelepo = true;
        fadeScript.gameObject.SetActive(true);
        fadeScript.Fade(); // ���̵��� ����

        yield return new WaitForSeconds(1f); // ���̵��� �Ϸ� ���
        // ���� �������� ������ 5�� ���, ���� ���������� �̵�
        if (stageManager.nowStageLv == 5)
        {
            // ���� �������� ��ȣ ���
            int nextStage = stageManager.nowStage + 1;
            // �������� �迭 ������ �ʰ����� �ʴ� ���, �������� ����
            if (nextStage - 1 < stageManager.stage.Length)
            {
                stageManager.ChangeStage(nextStage);
                stageManager.nowStageLv = 1; // ���� ���������� ������ 1�� ����
            }
            else
            {
                // �������� �迭 ������ �ʰ��ϴ� ���, ù ��° ���������� ���ư�
                stageManager.ChangeStage(1);
            }

            isbgm = false;
        }
        else
        {
            // ���� �������� ������ 5�� �ƴ� ���, ������ 1 ����
            stageManager.nowStageLv++;
            isbgm = true;
            if (stageManager.nowStageLv == 5)        
            {
                Debug.Log("���� UI �����");
                ShopUI shopUi = ShopUI.instance;
                Destroy(shopUi.gameObject);
            }
        }
        dm.playerData.nowStage = stageManager.nowStage;
        dm.playerData.nowStageLV = stageManager.nowStageLv;
        maincam.CameraPosition();   //������ �߰� 05.29 - ī�޶� ��ġ �̵�
        stageBGM(); //������ �߰� 6.4 - bgm����

        // ���� ���������� ���� ���
        stageUi.PrintStage(stageManager.nowStage, stageManager.nowStageLv);
        // �÷��̾� ��ġ �̵�
        targetObj.transform.position = toObj.transform.position;
        dm.playerData.nowPosition = toObj.transform.position;
        yield return new WaitForSeconds(1f); // ��Ż ��� �� ���� �ð� ���
        isTelepo = false;
        dm.SaveData();
        fadeScript.gameObject.SetActive(false);
    }
    IEnumerator BossSceneShowDestroy(GameObject destroyObject)
    {
        yield return new WaitForSeconds(2f);
        Destroy(destroyObject);
    }
    void DestroyShopUi()        // ���� UI�� �ִٸ� �����ϴ� �Լ�
    {
        if (stageManager.nowStageLv == 4)
        {
            ShopUI shopUi = ShopUI.instance;
            if (shopUi != null)
            {
                Destroy(shopUi.gameObject);
            }
        }

    }

    void stageBGM() //�������� ����� bgm ����
    {
        if (stageManager.nowStageLv == 5)   //���� ���������� �� ����Ǵ� bgm
        {
            GameObject bossScnenShow;
            if (stageManager.nowStage == 1)
            {
                soundManager.BGMPlay(soundManager.boss_stage1);
                bossScnenShow = Instantiate(bossScene[0]);
                Debug.Log("1�������� ���� bgm����");
            }
            else if (stageManager.nowStage == 2)
            {
                soundManager.BGMPlay(soundManager.boss_stage2);
                bossScnenShow = Instantiate(bossScene[1]);
                Debug.Log("2�������� ���� bgm����");
            }
            else
            {
                soundManager.BGMPlay(soundManager.boss_stage3);
                bossScnenShow = Instantiate(bossScene[2]);
                Debug.Log("3�������� ���� bgm����");
            }
            StartCoroutine(BossSceneShowDestroy(bossScnenShow));
        }
        else   //�Ϲ� ���������� �� ����Ǵ� bgm
        {
            if (!isbgm) // nowStage�� ����� ���� ����ǰ� ���ǹ� �߰�
            {
                if (stageManager.nowStage == 1 || stageManager.nowStage == 0)
                {
                    soundManager.BGMPlay(soundManager.nomal_stage1);
                    Debug.Log("1�������� �Ϲ� bgm����");
                }
                else if (stageManager.nowStage == 2)
                {
                    soundManager.BGMPlay(soundManager.nomal_stage2);
                    Debug.Log("2�������� �Ϲ� bgm����");
                }
                else
                {
                    soundManager.BGMPlay(soundManager.nomal_stage3);
                    Debug.Log("3�������� �Ϲ� bgm����");
                }
            }
        }
    }
}
