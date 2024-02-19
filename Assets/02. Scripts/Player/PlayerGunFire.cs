using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGunFire : MonoBehaviour
{
    // ��ǥ : ���콺 ���� ��ư�� ������ �ü��� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�.
    // �ʿ� �Ӽ� :
    // - �Ѿ� Ƣ�� ����Ʈ ������
    public ParticleSystem HitEffect;
    // ���� ���� :
    // 1. ���࿡ ���콺 ���� ��ư�� ������
    // 2. ����(����)�� �����ϰ�, ��ġ�� ������ �����Ѵ�.
    // 3. ���̸� �߻��Ѵ�.
    // 4. ���̰� �ε��� ����� ������ �޾ƿ´�.
    // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��) ����Ʈ�� �����Ѵ�. 

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
        // 1. ���࿡ ���콺 ���� ��ư�� ���� ���� && ��Ÿ���� �� ���� ���� && �Ѿ� ���� > 0
        if (Input.GetMouseButton(0) && _CrrentTime >= _CoolTime && _bulletCount > 0)
        {
            
            _CrrentTime = 0;
            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);
                _reloadCoroutine = null;
                reloadText.enabled = false;
            }


            // 2. ����(����)�� �����ϰ�, ��ġ�� ������ �����Ѵ�.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. ���̸� �߻��Ѵ�.
            // 4. ���̰� �ε��� ����� ������ �޾ƿ´�.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit) 
            {
                // �ǽ����� 18. �Ѿ� �߻� (������)�� ���Ϳ��� ���� �� ���� ü�� ��� ��� ����
                /*                Monster monster = hitInfo.collider.gameObject.GetComponent<Monster>();
                                if (monster != null)
                                {
                                    monster.Hit(damage);
                                }*/

                iHitalbe hitObject = hitInfo.collider.GetComponent<iHitalbe>();
                if (hitObject != null) // ���� �� �ִ� ģ���ΰ�?
                {
                    hitObject.Hit(damage);
                }

                _bulletCount--;
                _bulletCount = Mathf.Max(0, _bulletCount);
                // 5. �ε��� ��ġ�� (�Ѿ��� Ƣ��) ����Ʈ�� ��ġ�Ѵ�. 
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. ����Ʈ�� �Ĵٺ��� ������ �ε��� ��ġ�� ���� ���ͷ� �Ѵ�.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play(); // �򶧸��� ��� ����ɼ� �ֵ��� play�� �޾���
                if (_bulletCount == 0)
                {
                    HitEffect.gameObject.SetActive(false);




                }
                BulletUI();
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && _reloadCoroutine == null)
        //�ǽ� ���� 16. RŰ ������ 1.5�� �� ������ (�߰��� �� ��� ������ �ϸ� ������ ���)
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
