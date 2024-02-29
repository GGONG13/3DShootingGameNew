using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.Purchasing;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlayerGunFireAbility : MonoBehaviour, iHitalbe
{
    public Gun[] guns;
    public Gun CurrentGun; // 현재 들고 있는 총
    public int CurrentGunIndex;

    public ParticleSystem HitEffect;

    private Animator _animator;

    private const int DefaltFOV = 60;
    private const int ZoomFOV = 15;
    private bool _isZoomMode = false; // 줌 모드냐?

    public float _zoomTimer;
    private float zoomTransition = 0f;

    public float _CrrentTime;
    public int Health = 100;
    public Coroutine _reloadCoroutine;
    public UnityEngine.UI.Image gunsImage;
    public UnityEngine.UI.Image ZoomImage;
    public UnityEngine.UI.Image hairImage;

    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI reloadText;

    public GameObject[] muzzle;
    //   public List<GameObject> muzzleList; 이렇게 꺼도 됨
    private void Start()
    {
        CurrentGun = guns[0];
        RefreshGun();
        BulletUI();
        SetZoomMode(false);
        reloadText.enabled = false;
        ZoomImage.gameObject.SetActive(false);
        Camera.main.fieldOfView = DefaltFOV;
        // _reloadCoroutine = StartCoroutine(ReloadBullet(0));
        _animator = GetComponentInChildren<Animator>();
        for (int i = 0; i < muzzle.Length; i++)
        {
            muzzle[i].SetActive(false);
        }
/*        foreach (GameObject go in muzzle) 
        {
            go.SetActive(false);
        } 이렇게 해도 됨 */
    }
    void Update()
    {
        RefreshGun();
        _CrrentTime += Time.deltaTime;
        _zoomTimer += Time.deltaTime;
        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태 && 총알 갯수 > 0

        if (Input.GetMouseButtonDown(2) && CurrentGun.GType == GunType.Sniper)
        {
            
            BulletUI();
            SetZoomMode(!_isZoomMode);
            /*            if (isZoomMode)
                        {
                            isZoomMode = false;
                            Camera.main.fieldOfView = DefaltFOV;
                            ZoomImage.gameObject.SetActive(false);
                            bulletText.color = Color.black;
                        }
                        else
                        {
                            isZoomMode = true;
                            Camera.main.fieldOfView = ZoomFOV;
                            ZoomImage.gameObject.SetActive(true);
                            bulletText.color = Color.yellow;

                        }*/
        }

        if (GameManager.Instance.State == GameState.Start)
        {
            if (Input.GetMouseButton(0) && _CrrentTime >= CurrentGun._CoolTime && CurrentGun._bulletCount > 0)
            {

                _CrrentTime = 0;
                if (_reloadCoroutine != null)
                {
                    StopCoroutine(_reloadCoroutine);
                    _reloadCoroutine = null;
                    reloadText.enabled = false;
                }

                _animator.SetTrigger("Shot");

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
                        DamageInfo damage = new DamageInfo(DamageType.Normal, CurrentGun.damage);
                        damage.Position = hitInfo.point;
                        damage.Normal = hitInfo.normal;

                        if (UnityEngine.Random.Range(0, 2) == 0) 
                        {
                            Debug.Log("크리티컬!");
                            damage.DamageType = DamageType.Cirtical;
                            damage.Amount *= 2; 
                        }

                        hitObject.Hit(damage);
                    }

                    CurrentGun._bulletCount--;
                    CurrentGun._bulletCount = Mathf.Max(0, CurrentGun._bulletCount);
                    StartCoroutine(Muzzel_Coroutine());
                    // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 위치한다. 
                    HitEffect.gameObject.transform.position = hitInfo.point;
                    // 6. 이펙트가 쳐다보는 방향을 부딪힌 위치의 법선 벡터로 한다.
                    HitEffect.gameObject.transform.forward = hitInfo.normal;
                    HitEffect.Play(); // 쏠때마다 계속 재생될수 있도록 play를 달아줌


                    if (CurrentGun._bulletCount == 0)
                    {
                        HitEffect.gameObject.SetActive(false);
                    }
                    BulletUI();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && _reloadCoroutine == null)
        //실습 과제 16. R키 누르면 1.5초 후 재장전 (중간에 총 쏘는 행위를 하면 재장전 취소)
        {
            _reloadCoroutine = StartCoroutine(ReloadBullet(0));
            BulletUI();
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            CurrentGun = guns[0];
            SetZoomMode(false);
            BulletUI();


        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            CurrentGun = guns[1];
            BulletUI();
            SetZoomMode(false);

        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            CurrentGun = guns[2];
            BulletUI();
            SetZoomMode(false);

        }


        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            CurrentGunIndex--;
            if (CurrentGunIndex < 0) 
            {
                CurrentGunIndex = guns.Length - 1;
            }
            CurrentGun = guns[CurrentGunIndex];

            BulletUI();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            CurrentGunIndex++;
            if (CurrentGunIndex > guns.Length - 1)
            {
                CurrentGunIndex = 0;
            }
            CurrentGun = guns[CurrentGunIndex];

            BulletUI();
        }

        if (_isZoomMode && zoomTransition < 1)
        {
            zoomTransition += Time.deltaTime / 0.2f;
            Camera.main.fieldOfView = Mathf.Lerp(DefaltFOV, ZoomFOV, zoomTransition);
        }
        else if (!_isZoomMode && zoomTransition > 0) 
        {
            zoomTransition -= Time.deltaTime / 0.1f;
            Camera.main.fieldOfView = Mathf.Lerp(DefaltFOV, ZoomFOV, zoomTransition);
        }
    }

    private IEnumerator ReloadBullet(float delayTime)
    {

        reloadText.enabled = true;
        yield return new WaitForSeconds(1.5f);
            if (CurrentGun._bulletCount == 0)
        {
            CurrentGun._bulletCount = CurrentGun._bullet;
            CurrentGun._bulletMax -= CurrentGun._bullet;
            HitEffect.gameObject.SetActive(true);
        }
        else if (CurrentGun._bulletCount > 0)
        {
            CurrentGun._bulletMax -= CurrentGun._bulletCount;
            CurrentGun._bulletCount = CurrentGun._bullet;
        }
        BulletUI();
        reloadText.enabled = false;
    }
 

    void BulletUI()
    {
        bulletText.text = $"Bullet {CurrentGun._bulletCount:d2}/{CurrentGun._bulletMax}";
    }
    public void Hit(DamageInfo damageInfo)
    {
        Health -= damageInfo.Amount;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void RefreshGun()
    {
        foreach (Gun gun in guns) 
        {
            gun.gameObject.SetActive(gun == CurrentGun);
            gunsImage.sprite = CurrentGun.ProfileImage;
        }
               
    }

    private void SetZoomMode(bool set)
    {
        _isZoomMode = set;

        if (!_isZoomMode)
        {
             Camera.main.fieldOfView = DefaltFOV;
            // Camera.main.fieldOfView = Mathf.Lerp(ZoomFOV, DefaltFOV, _zoomTimer / 2f);
            ZoomImage.gameObject.SetActive(false);
            hairImage.enabled = true;
            bulletText.color = Color.black;
            
        }
        else
        {
             Camera.main.fieldOfView = ZoomFOV;
           //  Camera.main.fieldOfView = Mathf.Lerp(DefaltFOV, ZoomFOV, _zoomTimer / 1f);
            ZoomImage.gameObject.SetActive(true);
            hairImage.enabled = false;
            bulletText.color = Color.yellow;

        }
        RefreshGun();
    }

    public IEnumerator Muzzel_Coroutine() 
    {
        int random = UnityEngine.Random.Range(0, muzzle.Length);
        muzzle[random].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzle[random].SetActive(false);
    }
}
