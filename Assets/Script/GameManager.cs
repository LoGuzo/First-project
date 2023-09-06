using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int health;
    public int reality;
    public Transform player;
    public Slider Health_Main;
    public Slider Health_Alpha;
    public ObjectManager objectManager;
    public string[] enemyObj;
    public int totalCoinVal = 0;
    bool isDelay;
    public float delayTime;
    // 하트용 public Image[] UIhealth; 
    // 하트용 public Image[] UIreality;

    void Start()
    {
        SpawnEnemy();
    }
    void Awake()
    {
        enemyObj =new string[]{"Snow"};
    }
    public void HpHeal()
    {
        if (health <= 3)// 하트용 UIhealth.Length
        {
            if (isDelay == false)
            {
                isDelay = true;
                StartCoroutine(Heal());
                Health_Main.value = health;
            }
        }
        else if(health < 7)
        {
            if (isDelay == false)
            {
                isDelay = true;
                StartCoroutine(Heal());
                Health_Alpha.value = health;
            }
        }
    }
    IEnumerator Heal()
    {
        yield return new WaitForSeconds(10.0f);
        // 하트용 UIhealth[health].color = new Color(1, 0, 0, 1);
        health++;
        isDelay = false;
    }
    // Start is called before the first frame update 

    // Update is called once per frame
    void Update()
    {
        HpHeal();
    }
    
    public void HealthDown()
    {
        if (health <= 3)
        {
            health--;
            Health_Main.value = health;
            // 하트용 UIhealth[health].color = new Color(1, 0, 0, 0);
        }
        else if (health < 7)
        {
            health--;
            Health_Alpha.value = health;
            // 하트용 UIhealth[0].color = new Color(1, 0, 0, 0);
        }
        
    }
    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (enemyObj.ToString())
        {
            case "Snow":
                enemyIndex = 0;
                break;
        }
        GameObject enemy = objectManager.MakeObj(enemyObj[enemyIndex]);
        enemy.transform.position = new Vector3(14f, -6, 0);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        BossManaager enemyLogic = enemy.GetComponent<BossManaager>();
        enemyLogic.playerTrans = player;
        enemyLogic.objectManager = objectManager;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthDown();
        }
    }
}
