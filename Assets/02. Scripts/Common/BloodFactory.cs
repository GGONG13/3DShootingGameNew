using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFactory : MonoBehaviour
{
    public static BloodFactory Instance { get; private set; }
    public GameObject BloodPostb;
    public int BloodCount = 5;
    private List<GameObject> pool;
    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < BloodCount; i++) 
        {
            GameObject bloodObject = Instantiate(BloodPostb);
            bloodObject.SetActive(false);
            pool.Add(bloodObject);
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Make(Vector3 position, Vector3 normal)
    {
        GameObject bloodObject = null;
        for (int i = 0;i < BloodCount; i++) 
        {
           if (pool[i].activeInHierarchy == false)
            {
                bloodObject = pool[i];
                bloodObject.transform.position = position;
                bloodObject.transform.forward = normal;
                bloodObject.SetActive(true);
                break;
            }
        } 
    }
}
