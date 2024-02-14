using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // ��ǥ : Ű���� ����Ű(wasd)�� ���� ĳ���͸� �ٶ󺸴� ���� �������� �̵���Ű�� �ʹ�.
    // �Ӽ� :
    // - �̵��ӵ�
    float MoveSpeed = 5f; // �Ϲ� �ӵ�
    float RunSpeed = 10f; // �ٴ� �ӵ�
    public Slider Slider;
    public float currentStamina;
    public const float maxStamina = 100;
    public float StaminaConsumSpeed = 20f;
    public float StaminaChargeSpeed = 50;
    public float currentTime;

    private CharacterController _characterController;

    // ��ǥ : ĳ���Ϳ� �߷��� �����ϰ� �ʹ�.
    // �ʿ� �Ӽ� :
    // - �߷� ��
    private float _gravity = -20; // �߷� ����
    // - ������ �߷� ���� : y�� �ӵ�
    private float _yVelocity = 0;

    // ��ǥ : �����̽� �ٸ� ������ ĳ���͸� �����ϰ� �ʹ�.
    // �ʿ� �Ӽ� : 
    // - ���� �Ŀ� ��
    public float JumpPower = 10;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // ���� ���� :
    // 1. ���࿡ [SpaceBar] ��ư�� ������
    // 2. �÷��̾� y�࿡�� ���� �Ŀ��� �����Ѵ�.



    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        currentStamina = maxStamina;
        Slider.maxValue = maxStamina;
        Slider.value = currentStamina;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha9))
        {
            CameraManager.Instance.SetCamera(true);
        }
        else if (Input.GetKey(KeyCode.Alpha0))
        {
            CameraManager.Instance.SetCamera(false);
        }
        // ��������
        // 1. Ű �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� ����(Local ��ǥ�� ����)���� ���� ���ϱ�
        Vector3 dir = new Vector3(h, 0, v); // ���� ��ǥ��
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir); // Local -> World�� �ٲ��� / �۷ι� ��ǥ��


        if (_characterController.isGrounded) 
        {
            _isJumping = false;
            _yVelocity = 0;

            JumpRemainCount = JumpMaxCount;
        }
        // ���� ���� :
        // 1. ���࿡ [SpaceBar] ��ư�� ������
        if (Input.GetKeyDown(KeyCode.Space) && JumpRemainCount > 0) // GetKeyDown -> ���� �������� true / isGrounded ���϶���
        {
            _isJumping = true;
            JumpRemainCount--; // ��� ��
            // 2. �÷��̾� y�࿡�� ���� �Ŀ��� �����Ѵ�.
            _yVelocity = JumpPower;
        }


        // ���� ���� :
        // 3-1. �߷� ���ӵ� ���
        //      �߷� ���ӵ��� �����ȴ�.

            _yVelocity += _gravity * Time.deltaTime;



        // 2. �÷��̾�� y�࿡ �־� �߷��� �����Ѵ�.
          dir.y = _yVelocity;
        // 3-2. �̵��ϱ� 
        float Speed = MoveSpeed; // 5
        // transform.position += MoveSpeed * dir * Time.deltaTime;
        _characterController.Move(dir * Speed * Time.deltaTime);

        // �ǽ� ���� 1. Shift ������ ������ ���� �ٱ� (�̵� �ӵ� 10)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentStamina -= StaminaConsumSpeed * Time.deltaTime; // �ʴ� 20�� �Ҹ�
            Speed = RunSpeed; // 10
        }
        else
        {
            currentStamina += StaminaChargeSpeed * Time.deltaTime; // �ʴ� 50�� ����
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        Slider.value = currentStamina;


    }
}
