using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed = 0.1f;
    public string type;
    public ObjectManager objectManager;
    GameManager gameManager;
    public GameObject val;
    private Transform trans;
    private Rigidbody2D rigid;
    private Transform itemTrans;
    private int coinVal;
    public Transform hudTrans;
    Transform objectMTrans;
    void Awake()
    {
        objectMTrans = GameObject.Find("ObjectManager").GetComponent<Transform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        itemTrans=GameObject.Find("ItemBox").GetComponent<Transform>();
        trans = transform;
        rigid = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
            if (gameObject.layer == 10) // 자석
            {
                Invoke("Magnet", 3);
            }
    }
    void OnEnable()
    {
        switch(type)
        {
            case "Coin":
                coinVal = Random.Range(10, 30);
                break;
        }
    }

    public void CoinVal(int coinVal)
    {
        GameObject text = objectManager.MakeObj("Val");
        text.transform.position = hudTrans.position;
        text.GetComponent<CoinText>().val = coinVal;
    }
    void Magnet() // 자석 함수
    {
        trans.position = Vector2.Lerp(gameObject.transform.position, itemTrans.transform.position, speed);
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == 10)
        {
            if (collision.gameObject.tag == "ItemBox")
            {
                gameObject.transform.SetParent(objectMTrans);
                gameManager.totalCoinVal += coinVal;
                gameObject.SetActive(false);
                CoinVal(coinVal);
            }
        }
    }
}
