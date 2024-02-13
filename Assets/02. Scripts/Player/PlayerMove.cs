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

    private void Start()
    {
        currentStamina = maxStamina;
        Slider.maxValue = maxStamina;
        Slider.value = currentStamina;
    }
    // Update is called once per frame
    void Update()
    {
        // ��������
        // 1. Ű �Է� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 'ĳ���Ͱ� �ٶ󺸴� ����'�� ����(Local ��ǥ�� ����)���� ���� ���ϱ�
        Vector3 dir = new Vector3(h, 0, v); // ���� ��ǥ��
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir); // Local -> World�� �ٲ��� / �۷ι� ��ǥ��
        // 3. �̵��ϱ� 
        transform.position += MoveSpeed * dir * Time.deltaTime;

        float Speed = MoveSpeed; // 5
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
