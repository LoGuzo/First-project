using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinText : MonoBehaviour
{
    public Transform objectManager;
    public float moveSpeed;
    public float alphaSpeed;
    public float destroyTime;
    TextMeshPro text;
    Color alpha;
    public int val;

    void Start()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<Transform>();
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
    }
    void OnEnable()
    {
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
        text.text = "+ " + val.ToString() + "G";
     
    }
    void DestroyObject()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(objectManager);
        alpha.a = 1;
    }
}
