using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
