using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroyObject : MonoBehaviour
{
    [SerializeField]
    private static GameObject Instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
