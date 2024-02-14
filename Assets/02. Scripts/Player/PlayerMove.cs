using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 목표 : 키보드 방향키(wasd)에 따라 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다.
    // 속성 :
    // - 이동속도
    float MoveSpeed = 5f; // 일반 속도
    float RunSpeed = 10f; // 뛰는 속도
    public Slider Slider;
    public float currentStamina;
    public const float maxStamina = 100;
    public float StaminaConsumSpeed = 20f;
    public float StaminaChargeSpeed = 50;
    public float currentTime;

    private CharacterController _characterController;

    // 목표 : 캐릭터에 중력을 적용하고 싶다.
    // 필요 속성 :
    // - 중력 값
    private float _gravity = -20; // 중력 변수
    // - 누적할 중력 변수 : y축 속도
    private float _yVelocity = 0;

    // 목표 : 스페이스 바를 누르면 캐릭터를 점프하고 싶다.
    // 필요 속성 : 
    // - 점프 파워 값
    public float JumpPower = 10;
    public int JumpMaxCount = 2;
    public int JumpRemainCount;
    private bool _isJumping = false;
    // 구현 순서 :
    // 1. 만약에 [SpaceBar] 버튼을 누르면
    // 2. 플레이어 y축에게 점프 파워를 적용한다.



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
        // 구현순서
        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. '캐릭터가 바라보는 방향'을 기준(Local 좌표계 기준)으로 방향 구하기
        Vector3 dir = new Vector3(h, 0, v); // 로컬 좌표계
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir); // Local -> World로 바꿔줌 / 글로벌 좌표계


        if (_characterController.isGrounded) 
        {
            _isJumping = false;
            _yVelocity = 0;

            JumpRemainCount = JumpMaxCount;
        }
        // 구현 순서 :
        // 1. 만약에 [SpaceBar] 버튼을 누르면
        if (Input.GetKeyDown(KeyCode.Space) && JumpRemainCount > 0) // GetKeyDown -> 누른 순간에만 true / isGrounded 땅일때만
        {
            _isJumping = true;
            JumpRemainCount--; // 깎는 중
            // 2. 플레이어 y축에게 점프 파워를 적용한다.
            _yVelocity = JumpPower;
        }


        // 구현 순서 :
        // 3-1. 중력 가속도 계산
        //      중력 가속도가 누적된다.

            _yVelocity += _gravity * Time.deltaTime;



        // 2. 플레이어에게 y축에 있어 중력을 적용한다.
          dir.y = _yVelocity;
        // 3-2. 이동하기 
        float Speed = MoveSpeed; // 5
        // transform.position += MoveSpeed * dir * Time.deltaTime;
        _characterController.Move(dir * Speed * Time.deltaTime);

        // 실습 과제 1. Shift 누르고 있으면 빨리 뛰기 (이동 속도 10)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentStamina -= StaminaConsumSpeed * Time.deltaTime; // 초당 20씩 소모
            Speed = RunSpeed; // 10
        }
        else
        {
            currentStamina += StaminaChargeSpeed * Time.deltaTime; // 초당 50씩 충전
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        Slider.value = currentStamina;


    }
}
