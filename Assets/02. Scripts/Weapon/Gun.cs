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

    // ���ݷ�
    public int damage = 10;

    // �߻� ��Ÿ��
    public float _CoolTime = 0.5f;


    // �Ѿ� ����
    public int _bulletMax = 300;
    public int _bullet = 30;
    public int _bulletCount = 0;

    // ������ �ð�
    public float ReloadTime = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        // �Ѿ� ���� �ʱ�ȭ, �ѿ��� �ؾ� ��
        _bulletCount = _bullet;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
