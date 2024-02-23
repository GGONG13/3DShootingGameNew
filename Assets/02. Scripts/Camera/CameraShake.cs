using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // ��ǥ : ī�޶� ���� �ð����� �����ϰ� ���� �ʹ�.
    // �ʿ� �Ӽ�
    // - ����ŷ �ð�
    public float _shakingDuration = 0.2f;
    // - ����ŷ ���� �ð�
    private float _shakingTimer;
    // - ����ŷ �Ŀ�
    public float _shakingPower = 0.025f;
    // - ����ŷ ���̳�?
    public bool _isShaking = false; // �����Ҷ����� �Ǿ�� �ϴ���?�� false / true 

    // ���� ���� :
    // 1. �ð��� �帥��
    // 2. �����ϰ� ����
    // 3. ���� �ð��� ������ �ʱ�ȭ

    public void Shake()
    {
        _shakingTimer = 0f;
        _isShaking = true;
    }

    private void Update()
    {
       if (!_isShaking) 
        {
            return;
        }

        // ���� ���� :
        // 1. �ð��� �帥��
        _shakingTimer += Time.deltaTime;

        // 2. �����ϰ� ����
        transform.position = Vector3.zero + Random.insideUnitSphere * _shakingPower;
        // 3. ���� �ð��� ������ �ʱ�ȭ
        if (_shakingTimer >= _shakingDuration)
        {
            _isShaking = false;
            transform.position = Vector3.zero;
        }
       
    }

}
