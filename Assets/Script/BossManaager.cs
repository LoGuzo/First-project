using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManaager : MonoBehaviour
{
    public ObjectManager objectManager;
    public string enemyName;
    public int health;
    public Transform objectMTrans;
    public Transform playerTrans;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;
    public int speed=2;
    bool trigger;
    Vector2 DropPow;



    void Start()
    {
        
    }

    void Update()
    {
         //FaceMove();
        if (Input.GetKeyDown(KeyCode.G))
        {
            Think();
        }
    }
    private void FixedUpdate()
    {
    }
    void Awake()
    {
        objectMTrans = GameObject.Find("ObjectManager").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void FaceMove()
    {
        FaceTarget();
        float dir = playerTrans.position.x - transform.position.x; // 유도기능
        dir = (dir < 0) ? -1 : 1;
        transform.Translate(new Vector2(dir, 0) * speed * Time.deltaTime);
        
    }
    void FaceTarget()
    {
        if (playerTrans.position.x - transform.position.x < 0) // 타겟이 왼쪽에 있을 때
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else // 타겟이 오른쪽에 있을 때
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void OnEnable()
    {
        switch (enemyName)
        {
            case "Snow":
                health = 1;
                break;
        }
    }
    void Think()
    {
        patternIndex = patternIndex == 1 ? 0 : patternIndex + 1;
        curPatternCount = 0;
        switch (patternIndex)
        {
            case 0:
                SnowFoward();
                break;
            case 1:
                SnowAround();
                break;
        }
    }

    void SnowFoward()
    {
        float dir = playerTrans.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;
        for (int index = 0; index < 4; index++)
        {
            GameObject snow = objectManager.MakeObj("BossEffect");
            snow.transform.position = transform.position + new Vector3(dir,0,0) * 0.5f;

            Rigidbody2D rigid = snow.GetComponent<Rigidbody2D>();
            Vector2 vec = new Vector2(dir, -0.2f) + new Vector2(0, 1) * (index*0.1f);
            rigid.AddForce(vec.normalized*5, ForceMode2D.Impulse);
        }

        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("SnowFoward", 2);
        else
            Invoke("Think", 3);
    }

    void SnowAround()
    {
        int roundNumA = 10;
        for(int index = 0; index < roundNumA; index++)
        {
            GameObject snow = objectManager.MakeObj("BossEffect");
            snow.transform.position = transform.position;
            snow.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = snow.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNumA) 
                                        ,Mathf.Sin(Mathf.PI * 2 * index / roundNumA));
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }
        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("SnowAround", 0.7f);
        else
            Invoke("Think", 3);
    }
    public void OnDie()
    {
        gameObject.transform.SetParent(objectMTrans);
        this.gameObject.SetActive(false);
    }
    public void CoinDrop() // 코인 드랍
    {
        int random = Random.Range(1, 5);
        for (int i = 0; i < random; i++)
        {
            int updown = Random.Range(1, 5);
            int leftright = Random.Range(-2, 3);
            DropPow = new Vector2(leftright, updown);
            GameObject coin = objectManager.MakeObj("GoldCoin");
            Item itemLogic = coin.GetComponent<Item>();
            coin.transform.position = transform.position;
            itemLogic.objectManager = objectManager;
            Rigidbody2D rigid = coin.GetComponent<Rigidbody2D>();
            rigid.AddForce(DropPow, ForceMode2D.Impulse);
        }
    }
    public void EnemyHealthDown(int a)
    {
        if (health > 1)
            health-=a;
        else if(health<=1)
        {
            OnDie();
            CoinDrop();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
