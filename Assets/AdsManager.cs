using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public GameObject adsPrefab;
    public static bool isInitialized = false;
    void Start()
    {


        if (!isInitialized)
        {
            isInitialized = true;
            GameObject ads = Instantiate(adsPrefab);
            DontDestroyOnLoad(ads);
        }

    }
}
