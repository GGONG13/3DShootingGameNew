using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGunFire : MonoBehaviour
{
    // 목표 : 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성 :
    // - 총알 튀는 이펙트 프리펩
    public ParticleSystem HitEffect;
    // 구현 순서 :
    // 1. 만약에 마우스 왼쪽 버튼을 누르면
    // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
    // 3. 레이를 발사한다.
    // 4. 레이가 부딪힌 대상의 정보를 받아온다.
    // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 생성한다. 

    public float _CoolTime = 0.5f;
    public float _CrrentTime;

    public int _bulletMax = 300;
    public int _bullet = 30;
    public int _bulletCount = 0;
    public int damage = 1;

    public Coroutine _reloadCoroutine;


    public Text bulletText;
    public Text reloadText;

    private void Start()
    {
        _bulletCount = _bullet;
        BulletUI();
        reloadText.enabled = false;
       // _reloadCoroutine = StartCoroutine(ReloadBullet(0));
    }
    void Update()
    {
        _CrrentTime += Time.deltaTime;
        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태 && 총알 갯수 > 0
        if (Input.GetMouseButton(0) && _CrrentTime >= _CoolTime && _bulletCount > 0)
        {
            
            _CrrentTime = 0;
            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);
                _reloadCoroutine = null;
                reloadText.enabled = false;
            }


            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딪힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit) 
            {
                // 실습과제 18. 총알 발사 (레이저)를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                /*                Monster monster = hitInfo.collider.gameObject.GetComponent<Monster>();
                                if (monster != null)
                                {
                                    monster.Hit(damage);
                                }*/

                iHitalbe hitObject = hitInfo.collider.GetComponent<iHitalbe>();
                if (hitObject != null) // 때릴 수 있는 친구인가?
                {
                    hitObject.Hit(damage);
                }

                _bulletCount--;
                _bulletCount = Mathf.Max(0, _bulletCount);
                // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 위치한다. 
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딪힌 위치의 법선 벡터로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play(); // 쏠때마다 계속 재생될수 있도록 play를 달아줌
                if (_bulletCount == 0)
                {
                    HitEffect.gameObject.SetActive(false);




                }
                BulletUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && _reloadCoroutine == null)
        //실습 과제 16. R키 누르면 1.5초 후 재장전 (중간에 총 쏘는 행위를 하면 재장전 취소)
        {
            _reloadCoroutine = StartCoroutine(ReloadBullet(0));
            BulletUI();
        }
    }

    private IEnumerator ReloadBullet(float delayTime)
    {

        reloadText.enabled = true;
        yield return new WaitForSeconds(1.5f);
            if (_bulletCount == 0)
        {
            _bulletCount = _bullet;
            _bulletMax -= _bullet;
            HitEffect.gameObject.SetActive(true);
        }
        else if (_bulletCount > 0)
        {
            _bulletMax -= _bulletCount;
            _bulletCount = _bullet;
        }
        BulletUI();
        reloadText.enabled = false;

    }

    void BulletUI()
    {
        bulletText.text = $"Bullet {_bulletCount:d2}/{_bulletMax}";
    }
}
