using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public Transform trans;
    public GameObject BossEffectPrefab;
    public GameObject GoldCoinPrefab;
    public GameObject BossSnowPrefab;
    public GameObject TextPrefab;


    GameObject[] goldCoin;
    GameObject[] bossEffect;
    GameObject[] targetPool;
    GameObject[] bossSnow;
    GameObject[] text;
    // Start is called before the first frame update

    // Update is called once per frame
    void Awake()
    {
        bossEffect = new GameObject[100];
        goldCoin = new GameObject[30];
        text = new GameObject[100];
        bossSnow = new GameObject[1];
        Generate();
    }
    void Generate()
    {
        for(int index = 0; index < bossEffect.Length; index++)
        {
            bossEffect[index] = Instantiate(BossEffectPrefab);
            bossEffect[index].transform.SetParent(trans);
            bossEffect[index].SetActive(false);
        }
        for (int index = 0; index < goldCoin.Length; index++)
        {
            goldCoin[index] = Instantiate(GoldCoinPrefab);
            goldCoin[index].transform.SetParent(trans);
            goldCoin[index].SetActive(false);
        }
        for(int index = 0; index < bossSnow.Length; index++)
        {
            bossSnow[index] = Instantiate(BossSnowPrefab);
            bossSnow[index].transform.SetParent(trans);
            bossSnow[index].SetActive(false);
        }
        for(int index = 0; index < text.Length; index++)
        {
            text[index] = Instantiate(TextPrefab);
            text[index].transform.SetParent(trans);
            text[index].SetActive(false);
        }

    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "GoldCoin":
                targetPool = goldCoin;
                break;
            case "BossEffect":
                targetPool = bossEffect;
                break;
            case "Snow":
                targetPool = bossSnow;
                break;
            case "Val":
                targetPool = text;
                break;
        }
        for(int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].transform.SetParent(null);
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }
        return null;
    }
}
