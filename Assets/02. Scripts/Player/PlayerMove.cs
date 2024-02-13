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

    private void Start()
    {
        currentStamina = maxStamina;
        Slider.maxValue = maxStamina;
        Slider.value = currentStamina;
    }
    // Update is called once per frame
    void Update()
    {
        // 구현순서
        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. '캐릭터가 바라보는 방향'을 기준(Local 좌표계 기준)으로 방향 구하기
        Vector3 dir = new Vector3(h, 0, v); // 로컬 좌표계
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir); // Local -> World로 바꿔줌 / 글로벌 좌표계
        // 3. 이동하기 
        transform.position += MoveSpeed * dir * Time.deltaTime;

        float Speed = MoveSpeed; // 5
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
