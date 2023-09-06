using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public Transform objectManager;
    void Start()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<Transform>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderEffect")
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(objectManager);
        }
    }
}
