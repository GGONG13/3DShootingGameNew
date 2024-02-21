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

    void Start()
    {
        Init();
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
}
