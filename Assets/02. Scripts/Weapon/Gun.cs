using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GunType
{
    Rifle,
    Sniper,
    Pistol,
}


public class Gun : MonoBehaviour
{

    public GunType GType;

    public Sprite ProfileImage;

    // 공격력
    public int damage = 10;

    // 발사 쿨타임
    public float _CoolTime = 0.5f;


    // 총알 갯수
    public int _bulletMax = 300;
    public int _bullet = 30;
    public int _bulletCount = 0;

    // 재장전 시간
    public float ReloadTime = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        // 총알 갯수 초기화, 총에서 해야 함
        _bulletCount = _bullet;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
