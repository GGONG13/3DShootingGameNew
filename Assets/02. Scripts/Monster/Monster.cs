using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Monster : MonoBehaviour, iHitalbe
{
    [Range(0f, 100f)]
    public int Health;
    public int MaxHealth = 100;
    [Header("몬스터 체력 슬라이더 UI")]
    public Slider MonsterSliderUI;
    [Header("아이템 프리펩")]
    public GameObject HealthPrefab;
    public GameObject StaminaPrefab;
    public GameObject BulletPrefab;
    void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Health <= 0)
        {
            DropItems();
            this.gameObject.SetActive(false);
        }

    }

    public void Hit(int damage)
    {
        Health -= damage;
        monsterUI();
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        Health = MaxHealth;
        monsterUI();
    }


    public void monsterUI()
    {
        MonsterSliderUI.value = (float)Health / (float)MaxHealth;
    }

    void DropItems() 
    {
        float random = Random.Range(0, 50);
        if (random < 20f)
        {
            Instantiate(HealthPrefab);
            HealthPrefab.SetActive(true);
            HealthPrefab.transform.position = transform.position;
        }
        else if (random < 20f && random < 40)
        {
            Instantiate(StaminaPrefab);
            StaminaPrefab.SetActive(true);
            StaminaPrefab.transform.position = transform.position;
        }
        else if (random < 50)
        {
            Instantiate(BulletPrefab);
            BulletPrefab.SetActive(true);
            BulletPrefab.transform.position = transform.position;
        }
    }
}
