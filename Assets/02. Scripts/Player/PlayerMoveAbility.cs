using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMoveAbility : MonoBehaviour, iHitalbe
{
    // ��ǥ: Ű���� ����Ű(wasd)�� ������ ĳ���͸� �ٶ󺸴� ���� �������� �̵���Ű�� �ʹ�. 
    // �Ӽ�:
    // - �̵��ӵ�
    public float MoveSpeed = 5;     // �Ϲ� �ӵ�
    public float RunSpeed = 10;    // �ٴ� �ӵ�

    public float Stamina = 100;             // ���¹̳�
    public const float MaxStamina = 100;    // ���¹̳� �ִ뷮
    public float StaminaConsumeSpeed = 33f; // �ʴ� ���¹̳� �Ҹ�
    public float StaminaChargeSpeed = 50;  // �ʴ� ���¹̳� ������

    [Header("���¹̳� �����̴� UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;
    private Animator _animator;

    // ��ǥ: �����̽��ٸ� ������ ĳ���͸� �����ϰ� �ʹ�.
    // �ʿ� �Ӽ�:
    // - ���� �Ŀ� ��
    public float JumpPower = 3f;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // ���� ����:
    // 1. ���࿡ [Spacebar] ��ư�� ������..
    // 2. �÷��̾�� y�࿡ �־� ���� �Ŀ��� �����Ѵ�

    [Header("�÷��̾� ü�� �����̴� UI")]
    public int Health;
    public int MaxHealth = 100;
    public Slider PlayerSliderUI;

    public float speed;

    // ��ǥ: ĳ���Ϳ� �߷��� �����ϰ� �ʹ�.
    // �ʿ� �Ӽ�:


    private float _gravity = -20;
    // - ������ �߷� ����: y�� �ӵ�
    public float _yVelocity = 0f;
    // ���� ����:
    // 1. �߷� ���ӵ��� �����ȴ�.
    // 2. �÷��̾�� y�࿡ �־� �߷��� �����Ѵ�.

    // 02.16 ��ǥ : ���� ����ִ� ���¿��� �����̽� �ٸ� ������ ��Ÿ�⸦ �ϰ� �ʹ�.
    // �ʿ� �Ӽ� :
    // - ��Ÿ�� �Ŀ�
    public float ClimingPower = 7f;
    // - ��Ÿ�� ����
    private bool _isCliming = false;
    // - ��Ÿ�� ���¹̳� �Ҹ� ����
    public float ClimingStaminaCosumeFactor = 1.5f;
    // ���� ����
    // 1. ���࿡ ���� ��� �ִµ�
    // 2. [SpaceBar] ��ư�� ������ ������
    // 3. ���� Ÿ�ڴ�.
   

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

    // ���� ����
    // 1. Ű �Է� �ޱ�
    // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���ⱸ�ϱ�
    // 3. �̵��ϱ�

    void Update()
    {
        // ���� ����
        // 1. ���࿡ ���� ��� �ִµ�
        if (_characterController.collisionFlags == CollisionFlags.Sides)
        // ĳ���� �����ؼ� ���� ��Ʈ�ѷ�
        {

            // 2. [SpaceBar] ��ư�� ������ ������
            if (Stamina > 0 && Input.GetKey(KeyCode.Space))
            {
                // 3. ���� Ÿ�ڴ�.
                _isCliming = true;
                _yVelocity = ClimingPower;
                Stamina -= StaminaConsumeSpeed * 1.5f * Time.deltaTime; // �ʴ� 33�� �Ҹ�

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
            // FPS ī�޶� ���� ��ȯ
            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            // TPS ī�޶� ���� ��ȯ
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }
        if (GameManager.Instance.State == GameState.Start)
        {            // 1. Ű �Է� �ޱ�
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� �������� ���ⱸ�ϱ�
            Vector3 dir = new Vector3(h, 0, v);             // ���� ��ǥ�� (������ ��������) 
            dir.Normalize();
            // Transforms direction from local space to world space.
            dir = Camera.main.transform.TransformDirection(dir); // �۷ι� ��ǥ�� (������ ��������)

            // 3-1. �߷� ����
            // 1. �߷� ���ӵ��� �����ȴ�.
            _yVelocity += _gravity * Time.deltaTime;

            // 2. �÷��̾�� y�࿡ �־� �߷��� �����Ѵ�.

            dir.y = _yVelocity;

            // 3-2. �̵��ϱ�
            //transform.position += speed * dir * Time.deltaTime;
            _characterController.Move(dir * MoveSpeed * Time.deltaTime);
            _animator.SetFloat("Move", dir.magnitude);

        }

        // �ǽ� ���� 1. Shift ������ ������ ���� �ٱ�
        float speed = MoveSpeed; // 5
        if (_isCliming || Input.GetKey(KeyCode.LeftShift)) // �ǽ� ���� 2. ���¹̳� ����
        {               // - Shfit ���� ���ȿ��� ���¹̳��� ������ �Ҹ�ȴ�. (3��)

            float factor = _isCliming ? ClimingStaminaCosumeFactor : 1;
            Stamina -= StaminaConsumeSpeed * factor * Time.deltaTime; // �ʴ� 33�� �Ҹ�

            if (Stamina > 0)
            {
                speed = RunSpeed;
            }


        }
        else if ((_characterController.isGrounded))
        {
            // - �ƴϸ� ���¹̳��� �Ҹ� �Ǵ� �ӵ����� ���� �ӵ��� �����ȴ� (2��)
            Stamina += StaminaChargeSpeed * Time.deltaTime; // �ʴ� 50�� ����

        }

        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSliderUI.value = Stamina / MaxStamina;  // 0 ~ 1;//

        // ���̸� ���� Ƚ�� �ʱ�ȭ
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

        // ���� ����
        // 1. ���࿡ [Spacebar] ��ư�� ������ ���� && (���̰ų� or ���� Ƚ���� �����ִٸ�)
        if (Input.GetKeyDown(KeyCode.Space) && (_characterController.isGrounded || (_isJumping && JumpRemainCount > 0)))
        {
            _isJumping = true;

            JumpRemainCount--;

            // 2. �÷��̾�� y�࿡ �־� ���� �Ŀ��� �����Ѵ�.
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
        Debug.Log("ī�޶� ���");
        yield return new WaitForSeconds(30f);
        Camera.main.transform.position = cameraOrigin;
        Debug.Log("ī�޶� �������");
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