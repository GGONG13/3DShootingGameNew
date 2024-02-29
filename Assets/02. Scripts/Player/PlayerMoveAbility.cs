using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMoveAbility : MonoBehaviour, iHitalbe
{
    // 목표: 키보드 방향키(wasd)를 누르면 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다. 
    // 속성:
    // - 이동속도
    public float MoveSpeed = 5;     // 일반 속도
    public float RunSpeed = 10;    // 뛰는 속도

    public float Stamina = 100;             // 스태미나
    public const float MaxStamina = 100;    // 스태미나 최대량
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50;  // 초당 스태미나 충전량

    [Header("스태미나 슬라이더 UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;
    private Animator _animator;

    // 목표: 스페이스바를 누르면 캐릭터를 점프하고 싶다.
    // 필요 속성:
    // - 점프 파워 값
    public float JumpPower = 3f;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // 구현 순서:
    // 1. 만약에 [Spacebar] 버튼을 누르면..
    // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다

    [Header("플레이어 체력 슬라이더 UI")]
    public int Health;
    public int MaxHealth = 100;
    public Slider PlayerSliderUI;

    public float speed;

    // 목표: 캐릭터에 중력을 적용하고 싶다.
    // 필요 속성:


    private float _gravity = -20;
    // - 누적할 중력 변수: y축 속도
    public float _yVelocity = 0f;
    // 구현 순서:
    // 1. 중력 가속도가 누적된다.
    // 2. 플레이어에게 y축에 있어 중력을 적용한다.

    // 02.16 목표 : 벽에 닿아있는 상태에서 스페이스 바를 누르면 벽타기를 하고 싶다.
    // 필요 속성 :
    // - 벽타기 파워
    public float ClimingPower = 7f;
    // - 벽타기 상태
    private bool _isCliming = false;
    // - 벽타기 스태미너 소모량 팩터
    public float ClimingStaminaCosumeFactor = 1.5f;
    // 구현 순서
    // 1. 만약에 벽에 닿아 있는데
    // 2. [SpaceBar] 버튼을 누르고 있으면
    // 3. 벽을 타겠다.
   

    public Image HitEffectImageUI;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        HitEffectImageUI.enabled = false;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Stamina = MaxStamina;
        Health = MaxHealth;
        PlayerHealthUI();
    }

    // 구현 순서
    // 1. 키 입력 받기
    // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
    // 3. 이동하기

    void Update()
    {
        // 구현 순서
        // 1. 만약에 벽에 닿아 있는데
        if (_characterController.collisionFlags == CollisionFlags.Sides)
        // 캐릭터 관련해서 제어 컨트롤러
        {

            // 2. [SpaceBar] 버튼을 누르고 있으면
            if (Stamina > 0 && Input.GetKey(KeyCode.Space))
            {
                // 3. 벽을 타겠다.
                _isCliming = true;
                _yVelocity = ClimingPower;
                Stamina -= StaminaConsumeSpeed * 1.5f * Time.deltaTime; // 초당 33씩 소모

                if (Stamina > 0)
                {
                    _isCliming = false;
                    _yVelocity = _gravity;
                    _isJumping = false;
                }
            }

        }
        else
        {
            _isCliming = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            // FPS 카메라 모드로 전환
            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // TPS 카메라 모드로 전환
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }
        if (GameManager.Instance.State == GameState.Start)
        {            // 1. 키 입력 받기
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // 2. '캐릭터가 바라보는 방향'을 기준으로 방향구하기
            Vector3 dir = new Vector3(h, 0, v);             // 로컬 좌표꼐 (나만의 동서남북) 
            dir.Normalize();
            // Transforms direction from local space to world space.
            dir = Camera.main.transform.TransformDirection(dir); // 글로벌 좌표계 (세상의 동서남북)

            // 3-1. 중력 적용
            // 1. 중력 가속도가 누적된다.
            _yVelocity += _gravity * Time.deltaTime;

            // 2. 플레이어에게 y축에 있어 중력을 적용한다.

            dir.y = _yVelocity;

            // 3-2. 이동하기
            //transform.position += speed * dir * Time.deltaTime;
            _characterController.Move(dir * MoveSpeed * Time.deltaTime);
            _animator.SetFloat("Move", dir.magnitude);

        }

        // 실습 과제 1. Shift 누르고 있으면 빨리 뛰기
        float speed = MoveSpeed; // 5
        if (_isCliming || Input.GetKey(KeyCode.LeftShift)) // 실습 과제 2. 스태미너 구현
        {               // - Shfit 누른 동안에는 스태미나가 서서히 소모된다. (3초)

            float factor = _isCliming ? ClimingStaminaCosumeFactor : 1;
            Stamina -= StaminaConsumeSpeed * factor * Time.deltaTime; // 초당 33씩 소모

            if (Stamina > 0)
            {
                speed = RunSpeed;
            }


        }
        else if ((_characterController.isGrounded))
        {
            // - 아니면 스태미나가 소모 되는 속도보다 빠른 속도로 충전된다 (2초)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전

        }

        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSliderUI.value = Stamina / MaxStamina;  // 0 ~ 1;//

        // 땅이면 점프 횟수 초기화
        if (_characterController.isGrounded)
        {
            if (_yVelocity < -10)
            {
                DamageInfo damage = new DamageInfo(DamageType.Normal, 10 * (int)(_yVelocity / 10f));
            }

            PlayerHealthUI();
            _isJumping = false;
            _yVelocity = 0f;
            JumpRemainCount = JumpMaxCount;
        }

        // 점프 구현
        // 1. 만약에 [Spacebar] 버튼을 누르는 순간 && (땅이거나 or 점프 횟수가 남아있다면)
        if (Input.GetKeyDown(KeyCode.Space) && (_characterController.isGrounded || (_isJumping && JumpRemainCount > 0)))
        {
            _isJumping = true;

            JumpRemainCount--;

            // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.
            _yVelocity = JumpPower;
        }



    }
    public void Hit(DamageInfo damageInfo)
    {

        Health -= damageInfo.Amount;
        _animator.SetLayerWeight(-1, Health / (float)MaxHealth);
        StartCoroutine(HitEffect_Coroutine(0.3f));
        CameraManager.Instance.CameraShake.Shake();
        if (Health <= 0)
        {
            GameManager.Instance.GameOver();
            HitEffectImageUI.enabled = true;
            gameObject.SetActive(false);
        }
    }

    IEnumerator ShakingCamera_Coroutine()
    {
        Vector3 cameraOrigin = Camera.main.transform.position;
        float x = Random.Range(-5f, 5f) * 0.2f;
        Vector3 shakeCamera = cameraOrigin + new Vector3(x, x, 0);



        Camera.main.transform.position = shakeCamera;
        Debug.Log("카메라 흔들");
        yield return new WaitForSeconds(30f);
        Camera.main.transform.position = cameraOrigin;
        Debug.Log("카메라 원래대로");
    }

    IEnumerator HitEffect_Coroutine(float delay)
    {
        HitEffectImageUI.enabled = true;
        yield return new WaitForSeconds(0.2f);
        HitEffectImageUI.enabled = false;
    }

    private void PlayerHealthUI()
    {
        PlayerSliderUI.value = (float)Health / (float)MaxHealth;
    }

}