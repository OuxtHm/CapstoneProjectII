using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Boss : MonoBehaviour
{
    public static Boss Instance;
    Player player;
    BossHpBar bossHpBar;        // 2024-04-10 ������ �߰�
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform PbSpawn;  //������ ���� ��ġ ������Ʈ
    Transform AttackBox;    // ���� ���� ���� ������Ʈ
    BoxCollider2D BoxCollider2DSize;    //Attackbox ������Ʈ�� boxcollider2D

    bool ishurt = false;
    public bool bossMoving = false;
    bool isdie = false;
    int DirX;   //���Ͱ� �ٶ󺸴� ���Ⱚ
    public float playerLoc; // player�� X��ǥ
    public float bossLoc;  // boss�� X��ǥ
    public int atkPattern = 0; //boss ���� ����
    float distanceToTarget; //�÷��̾�� ���� ������ �Ÿ�
    public int turnPoint = 1;    // ���� ���� �� �̵� ��� ���� ����
    int totalDamage;    // ���� �������
    float randomTime;   //���̾Ʈ ���� �ð�
    float gatePos;

    [Header("���� ���� �ɷ�ġ")]
    public int boss_stage;  //������ �������� ����
    public float boss_MaxHP; //���� �ִ�ü��
    public float boss_CurHP; //���� ����ü��
    public int boss_Speed; //���� �̵��ӵ�
    public int boss_BumpPower;    //�浹 �����
    public int boss_OnePattenPower;   //ù��° ���� ���� �����
    public int boss_TwoPattenPower;   //�ι�° ���� ���� �����
    public int boss_ThreePattenPower;    //����° ���� ���� �����
    public int boss_FourPattenPower;   //�׹�° ���� ���� �����

    [Header("1�������� ���� ������")]
    public GameObject ArrowPb; // 1�������� ���� ȭ�� ������
    public GameObject ArrowrainPb; // 1�������� ���� ȭ��� ������
    public GameObject LaserPb; // 1�������� ���� ȭ��� ������
    public GameObject WarningPb;  //���� �� ����ǥ�� ������

    [Header("2�������� ���� ������")]
    public GameObject SwordEffectPb; // 2�������� ���� ���κ��� ������
    public GameObject FireEffectPb; // 2�������� ���� ���̾ ������

    [Header("3�������� ���� ������")]
    public GameObject FireBarrierPb;  //3 �������� ���� �Ҳ� �踮�� ������
    public GameObject FireBoltPb;  //3 �������� ���� ���̾Ʈ ������
    public GameObject FireGatePb;  //3 �������� ���� �극������Ʈ ������
    public GameObject FireBreathPb;  //3 �������� ���� ���̾�극�� ������
    public GameObject DemonBoss;    //3�������� ���� ������ ���� ������
    public GameObject Summoning;    //���� ������ ��ȯ�Ǵ� ȿ�� ������


    [Header("���� ����")]
    public GameObject coinPrefab; // ���� ������ ������ ���� ����
    public GameObject potionPrefab; // ���� ������ ������ ���� ����
    public GameObject skillItemPrefab; // ��ų ������ ������ ������ ���� ����
    private void Awake()
    {
        Instance = this;

        FireBarrierPb = Resources.Load<GameObject>("Prefabs/FireBarrier");
        FireBoltPb = Resources.Load<GameObject>("Prefabs/FireBolt");
        FireGatePb = Resources.Load<GameObject>("Prefabs/FireGate");
        FireBreathPb = Resources.Load<GameObject>("Prefabs/FireBreath");
        DemonBoss = Resources.Load<GameObject>("Prefabs/Demon_Boss");
        Summoning = Resources.Load<GameObject>("Prefabs/Summoning");

        SwordEffectPb = Resources.Load<GameObject>("Prefabs/SwordEffectPb");
        FireEffectPb = Resources.Load<GameObject>("Prefabs/FirePb");

        ArrowPb = Resources.Load<GameObject>("Prefabs/Arrow");
        ArrowrainPb = Resources.Load<GameObject>("Prefabs/ArrowRain");
        LaserPb = Resources.Load<GameObject>("Prefabs/Laser");
        WarningPb = Resources.Load<GameObject>("Prefabs/Warning");
    }

    private void Start()
    {
        player = Player.instance.GetComponent<Player>();
        bossHpBar = BossHpBar.instance;     // 2024-04-10 ������ �߰�
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        PbSpawn = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        rigid = this.GetComponent<Rigidbody2D>();
        randomAtk();
        OneTime();
    }

    void OneTime()
    {
        Invoke("MoveOn",2f);
        atkPattern = 0;
        distanceToTarget = 0;
        randomTime = 10;
    }

    public virtual void BossUpdate(Transform target)  // boss�� Update��
    {
        playerLoc = target.position.x;
        bossLoc = this.gameObject.transform.position.x;
        distanceToTarget = Mathf.Abs(target.position.x - transform.position.x); // �Ÿ��� ���̸� ���밪���� ����
        bossMove(target);
        bossAttack();
    }

    public void bossMove(Transform target)  // boss�� �����̵��� �ϴ� �Լ�
    {
        if (boss_stage == 1)   //���� ���� �� �÷��̾������� �̵�
        {
            if (bossMoving && !isdie && !ishurt)
            {
                if (turnPoint == 1)
                {
                    gameObject.transform.Translate(new Vector2(-DirX, 0) * Time.deltaTime * boss_Speed);
                    if (DirX == 1)
                        spriteRenderer.flipX = true;
                    else
                        spriteRenderer.flipX = false;
                }
                else if (turnPoint == -1)
                    gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);

                anim.SetBool("Move", true);
            }
            else
            {
                anim.SetBool("Move", false);
            }
        }
        else if (boss_stage == 2)
        {
            if (bossMoving && !isdie && !ishurt)
            {
                if (playerLoc < bossLoc)
                {
                    spriteRenderer.flipX = true;
                    DirX = -1;
                }
                else
                {
                    spriteRenderer.flipX = false;
                    DirX = 1;
                }
            }
        }
        else if (boss_stage == 3)
        {
            if (bossMoving && !isdie && !ishurt)
            {
                gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);
                anim.SetBool("Move", true);
            }
            else
                anim.SetBool("Move", false);
        }
        else
        {
            if (bossMoving && !isdie && !ishurt)
            {
                if (playerLoc < bossLoc)
                {
                    spriteRenderer.flipX = true;
                    DirX = -1;
                }
                else
                {
                    spriteRenderer.flipX = false;
                    DirX = 1;
                }
                Vector2 directionToTarget = target.position - transform.position;
                Vector2 normalizedDirection = directionToTarget.normalized;
                gameObject.transform.Translate(normalizedDirection * Time.deltaTime * boss_Speed);
                anim.SetBool("Move", true);
            }
        }
    }
    public void bossAttack()
    {
        if(!isdie)
        {
            if (playerLoc < bossLoc && !bossMoving) //�÷��̾ ���ʿ� ���� ��
            {
                spriteRenderer.flipX = true;
                DirX = -1;
                if(boss_stage == 1)
                    AttackBox.position = new Vector2(transform.position.x - 1.6f, transform.position.y);
                else if(boss_stage == 2)
                    AttackBox.position = new Vector2(transform.position.x - 1.5f, transform.position.y);
                else
                    AttackBox.position = new Vector2(transform.position.x - 5f, transform.position.y - 1);
            }
            else if (playerLoc > bossLoc && !bossMoving) //�÷��̾ �����ʿ� ���� ��
            {
                spriteRenderer.flipX = false;
                DirX = 1;
                if (boss_stage == 1)
                    AttackBox.position = new Vector2(transform.position.x + 1.6f, transform.position.y);
                else if (boss_stage == 2)
                    AttackBox.position = new Vector2(transform.position.x + 1.5f, transform.position.y);
                else
                    AttackBox.position = new Vector2(transform.position.x + 5f, transform.position.y - 1);
            }
            if(boss_stage == 1)
            {
                switch (atkPattern)
                {
                    case -1: //1�� �������� �������� ����� ���� ����
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 1);
                        totalDamage = boss_OnePattenPower;
                        atkPattern = 0;
                        break;

                    case 1:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 2);
                        //Ranger_Arrowattack(); �ִϸ��̼ǿ��� ����ǰ� ������
                        atkPattern = 0;
                        break;

                    case 2:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 3);
                        StartCoroutine(Ranger_Arrowrain());
                        atkPattern = 0;
                        break;

                    case 3:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 4);
                        StartCoroutine(Ranger_Laserattack());
                        atkPattern = 0;
                        break;
                }
            }
            if (boss_stage == 2)
            {
                switch (atkPattern)
                {
                    case 1:
                        bossMoving = false;
                        anim.SetTrigger("Move");
                        Invoke("MoveOn", 3f);
                        atkPattern = 0;
                        break;
                    case 2:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 2);
                        atkPattern = 0;
                        break;

                    case 3:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 3);
                        atkPattern = 0;
                        break;
                }
            }
            if (boss_stage == 3)
            {
                switch (atkPattern)
                {
                    case -1:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 1);
                        Invoke("MoveOn", 3f);
                        totalDamage = boss_OnePattenPower;
                        atkPattern = 0;
                        break;
                    case 1:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 2);
                        atkPattern = 0;
                        break;

                    case 2:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 3);
                        Invoke("MoveOn", 2f);
                        atkPattern = 0;
                        break;

                    case 3:
                        //bossMoving = false;
                        atkPattern = 0;
                        break;
                }

                //3stage ���̾Ʈ ���� �ð����� ����
                if (randomTime <= 0)
                {
                    randomTime = Random.Range(10, 41);
                    Demon_FireBolt();
                    Debug.Log("���̾Ʈ ����");
                }
                else
                    randomTime -= Time.deltaTime;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player")) //�浹 ����� ó��
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null && boss_CurHP > 0)
            {
                player.Playerhurt(boss_BumpPower);
            }
        }

        if (collision != null && collision.gameObject.CompareTag("wall"))   //�� �浹 ó��
        {
            turnPoint *= -1;
            DirX *= -1;
        }
    }
    
    public void randomAtk() // ���� ���� �������� ���ϱ�
    {
        atkPattern = Random.Range(1, 4);    // 1 ~ 3 ������ ���� �������� ����
        if (boss_stage == 1)    //������ �÷��̾��� �Ÿ��� 4��ŭ �̳��� ������ ���� ���� Ȯ��
            if(DirX == 1 && (playerLoc - bossLoc) <= 5f || (DirX == -1 && (playerLoc - bossLoc) >= -5f))
                atkPattern = -1;
        if(boss_stage == 3)
            if (DirX == 1 && (playerLoc - bossLoc) <= 7f || (DirX == -1 && (playerLoc - bossLoc) >= -7f))
                atkPattern = (Random.Range(0, 2) == 0) ? -1 : 1;
        Invoke("randomAtk", 4f);
    }

    void Ranger_Arrowattack()   //1stage Ȱ ��� ����
    {
        EffectPb APb = ArrowPb.GetComponent<EffectPb>();
        APb.Power = boss_TwoPattenPower;
        APb.dir = DirX;
        APb.DelTime = 3f;
        APb.movecheck = 1;
        APb.speed = 20;
        APb.playerpos = player.transform;

        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);

        Invoke("MoveOn", 3f);
    }

    IEnumerator Ranger_Arrowrain()  //1stage ȭ��� ����
    {
        EffectPb ArPb = ArrowrainPb.GetComponent<EffectPb>();
        Vector2 Targetpos = new Vector2(player.transform.position.x, PbSpawn.position.y + 1.1f);  //���� �ִ� Pbspawn��ġ���� �����ؼ� ���ο� ��ġ ����
        Vector2 Warningpos = new Vector2(player.transform.position.x, PbSpawn.position.y - 2.1f);  //���� ǥ�� ���� ��ġ
        ArPb.Power = boss_ThreePattenPower;
        ArPb.dir = DirX;
        ArPb.DelTime = 1.1f;
        ArPb.movecheck = 0;
        ArPb.playerpos = player.transform;

        GameObject Warring = Instantiate(WarningPb, Warningpos, PbSpawn.rotation);  //���� ǥ�� ����
        yield return new WaitForSeconds(1.5f);
        GameObject arrowrain = Instantiate(ArrowrainPb, Targetpos, PbSpawn.rotation);//ȭ��� ���� ����

        Destroy(Warring);
        Invoke("MoveOn", 4.5f);
    }

    IEnumerator Ranger_Laserattack()    //1stage ������ ����
    {
        Vector2 newPosition = new Vector2(this.transform.position.x + (DirX * 9.8f), this.transform.position.y + 0.45f);  //���� �ִ� Pbspawn��ġ���� �����ؼ� ���ο� ��ġ ����
        EffectPb LrPb = LaserPb.GetComponent<EffectPb>();
        LrPb.Power = boss_FourPattenPower;
        LrPb.dir = DirX;
        LrPb.DelTime = 0.3f;
        LrPb.movecheck = 0;
        LrPb.playerpos = player.transform;

        yield return new WaitForSeconds(0.9f);
        GameObject arrowlaser = Instantiate(LaserPb, newPosition, PbSpawn.rotation);

        Invoke("MoveOn", 5f);
    }

    void giveDamage()   //�ִϸ��̼ǿ��� ����Ǵ� ������� �Ѱ��ִ� �Լ�
    {
        BoxCollider2DSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxCollider2DSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player = collider.GetComponent<Player>();
                player.Playerhurt(totalDamage);
            }
        }
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator Knight_Transparent()  //2stage ���� ���� , SwordAttack 1
    {
        float fadeDuration = 0.8f;  // ��ȭ�� �ɸ��� �ð�
        float elapsedTime = 0f;
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        while (elapsedTime < fadeDuration)
        {
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 0f, elapsedTime / fadeDuration));  // ���İ� ������ ����
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.gameObject.transform.position = new Vector2(player.transform.position.x + (DirX > 0 ? -2 : 2), player.transform.position.y);
        yield return new WaitForSeconds(2f);
        this.gameObject.layer = LayerMask.NameToLayer("Boss");
        spriteRenderer.color = new Color(1, 1, 1, 1);
        anim.SetTrigger("Attack");
        anim.SetFloat("Attackpatten", 1);
        totalDamage = boss_OnePattenPower;
    }

    IEnumerator Knight_SwordAttack2()  //2stage ���κ��� ����, SwordAttack 2
    {
        Vector2 Spownpos = new Vector2(this.transform.position.x , this.PbSpawn.position.y);
        this.gameObject.transform.position = new Vector2(transform.position.x + (DirX > 0 ? 10 : -10), transform.position.y);

        yield return new WaitForSeconds(0.4f);
        EffectPb SEfPb = SwordEffectPb.GetComponent<EffectPb>();
        SEfPb.dir = DirX;
        SEfPb.Power = boss_TwoPattenPower;
        SEfPb.DelTime = 0.6f;
        SEfPb.movecheck = 0;
        SEfPb.playerpos = player.transform;

        GameObject effect = Instantiate(SwordEffectPb, Spownpos, PbSpawn.rotation);
        Invoke("MoveOn", 3.5f);
    }
    IEnumerator Knight_LightAttack()    //2stage �Ҳ� ����ü ����
    {
        Vector2 Spownpos = new Vector2(this.transform.position.x + DirX, this.PbSpawn.position.y);

        yield return new WaitForSeconds(0.1f);
        EffectPb FirePb = FireEffectPb.GetComponent<EffectPb>();
        FirePb.dir = DirX;
        FirePb.Power = boss_ThreePattenPower;
        FirePb.DelTime = 1f;
        FirePb.movecheck = 1;
        FirePb.speed = 15;
        FirePb.playerpos = player.transform;

        GameObject effect = Instantiate(FireEffectPb, Spownpos, PbSpawn.rotation);
        Invoke("MoveOn", 3.5f);
    }

    void Demon_FireBarrier()   //3stage �Ҳ� �踮�� ����
    {
        EffectPb FBPb = FireBarrierPb.GetComponent<EffectPb>();
        FBPb.Power = boss_TwoPattenPower;
        FBPb.dir = DirX;
        FBPb.DelTime = 1.5f;
        FBPb.movecheck = 0;
        FBPb.playerpos = player.transform;

        Vector2 Spownpos = new Vector2(this.transform.position.x, this.PbSpawn.position.y + 1);
        GameObject FireBarrier = Instantiate(FireBarrierPb, Spownpos, transform.rotation);
        Invoke("MoveOn", 4f);
    }
    void Demon_FireBolt()   //3stage ���̾Ʈ ����
    {
        EffectPb FTPb = FireBoltPb.GetComponent<EffectPb>();
        if(Mathf.Abs(playerLoc - bossLoc) > 0)
            FTPb.dir = DirX;
        else
            FTPb.dir = -DirX;

        FTPb.Power = boss_ThreePattenPower;
        FTPb.DelTime = 2f;
        FTPb.movecheck = 2;
        FTPb.speed = 10;
        FTPb.playerpos = player.transform;

        Vector2 Spownpos1 = new Vector2(this.transform.position.x - 2, this.transform.position.y + 3);
        Vector2 Spownpos2 = new Vector2(this.transform.position.x + 2, this.transform.position.y + 3);
        GameObject FireBolt1 = Instantiate(FireBoltPb, Spownpos1, transform.rotation);
        GameObject FireBolt2 = Instantiate(FireBoltPb, Spownpos2, transform.rotation);
    }
    void Demon_BreathGate()   //3stage �Ҳ� �극�� ���� ����Ʈ
    {
        EffectPb BGPb = FireGatePb.GetComponent<EffectPb>();
        BGPb.dir = -DirX;
        BGPb.DelTime = 2f;
        BGPb.movecheck = 0;
        BGPb.playerpos = player.transform;

        Vector2 Spownpos = new Vector2(playerLoc + (DirX > 0 ? 5 : -5), this.transform.position.y - 1f);
        GameObject FireBreath = Instantiate(FireGatePb, Spownpos, transform.rotation);
        gatePos = playerLoc + (DirX > 0 ? 5 : -5);
        Invoke("MoveOn", 3f);
        Invoke("Demon_FireBreath", 1f);
    }

    void Demon_FireBreath()   //3stage �Ҳ� �극�� ����
    {
        EffectPb FHPb = FireBreathPb.GetComponent<EffectPb>();
        FHPb.Power = boss_FourPattenPower;
        FHPb.dir = -DirX;
        FHPb.DelTime = 1f;
        FHPb.movecheck = 0;
        FHPb.playerpos = player.transform;

        Vector2 Spownpos = new Vector2(gatePos + (DirX > 0 ? -3 : 3), this.transform.position.y - 1f);
        GameObject FireBreath = Instantiate(FireBreathPb, Spownpos, transform.rotation);
    }

    public IEnumerator Hurt(Transform target, float Damage)  //�÷��̾�� �ǰ� �޾��� �� ����
    {
        yield return new WaitForSeconds(0);
        if (boss_CurHP > 0 && !ishurt)
        {
            ishurt = true;
            boss_CurHP = boss_CurHP - Damage;
            //StartCoroutine(bossHpBar.FrontHpUpdate());      // 2024-04-10 ������ �߰�
            //bossHpBar.anim.SetTrigger("Damage");
            StartCoroutine(Blink());

            if (boss_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                //StartCoroutine(bossHpBar.FrontHpUpdate());      // 2024-04-10 ������ �߰�
                //bossHpBar.anim.SetTrigger("Remove");
                StartCoroutine(Die());
            }
        }
        ishurt = false;
    }
    IEnumerator Blink() // �ǰ� ȿ��
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        DirX = 0;
        bossMoving = false;
        anim.SetTrigger("Die");
        anim.SetBool("Move", false);
        yield return new WaitForSeconds(2f);

        if(boss_stage == 4) //���� �������� ���� �� ���� ������ ��ȯ
        {
            Instantiate(Summoning, transform.position, transform.rotation);
            yield return new WaitForSeconds(2f);
            Instantiate(DemonBoss, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            // ������ ���� �ڵ� �߰�
            SpawnItems(coinPrefab, 3); // 3���� ���� ����
            SpawnItems(potionPrefab, 2); // 2���� ���� ����
            SpawnItems(skillItemPrefab, 1); // 1���� ��ų ������ ����
            Destroy(gameObject);
        }
            
        
    }

    void SpawnItems(GameObject itemPrefab, int itemCount)
    {
        List<Vector3> spawnPositions = new List<Vector3>();
        int maxAttempts = 10; // �ִ� �õ� Ƚ��

        for (int i = 0; i < itemCount; i++)
        {
            Vector3 spawnPosition;
            int attempts = 0;

            // �������� ��ġ�� �ʰ� ��ȿ�� ��ġ�� �����ǵ��� ����
            do
            {
                spawnPosition = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
                attempts++;
            }
            while ((IsPositionOverlap(spawnPosition, spawnPositions, 0.5f) || IsPositionBlocked(spawnPosition)) && attempts < maxAttempts); // �ּ� �Ÿ� 0.5f �� �浹 ����

            if (attempts < maxAttempts)
            {
                spawnPositions.Add(spawnPosition);
                GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
                StartCoroutine(RotateItem(spawnedItem));
            }
            else
            {
                Debug.LogWarning("��ȿ�� ��ġ�� ã�� ���� ������ ���� ����");
            }
        }
    }// 5.22 �̰���߰�

    bool IsPositionOverlap(Vector3 position, List<Vector3> positions, float minDistance)
    {
        foreach (var pos in positions)
        {
            if (Vector3.Distance(position, pos) < minDistance)
            {
                return true;
            }
        }
        return false;
    }// 5.22 �̰���߰�

    bool IsPositionBlocked(Vector3 position)
    {
        // Ư�� ���̾��� �浹ü�� �ִ��� Ȯ�� (��: ���̳� �ٴ� ���̾�)
        Collider2D hitCollider = Physics2D.OverlapCircle(position, 0.5f, LayerMask.GetMask("Wall", "Ground"));
        return hitCollider != null;
    }

    IEnumerator RotateItem(GameObject item)
    {
        while (item != null)
        {
            item.transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime); // Z�� �������� 45�� ȸ��
            yield return null;
        }
    }                  // 5.22 �̰���߰�


    void MoveOn()
    {
        if(!isdie)
            bossMoving = true;
    }

    public abstract void BossInitSetting(); // ���� �⺻ ������ �����ϴ� �Լ�(�߻�)


}
