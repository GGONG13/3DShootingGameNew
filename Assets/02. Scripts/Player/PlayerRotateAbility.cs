using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateAbility : MonoBehaviour
{  
    // ��ǥ: ���콺�� �����ϸ� ī�޶� �� �������� ȸ����Ű�� �ʹ�.
    // �ʿ� �Ӽ�:
    // - ȸ�� �ӵ�
    public float RotationSpeed = 200; // �ʴ� 200������ ȸ�� ������ �ӵ�
    // ������ x������ y����
    private float _mx = 0;








    void Update()
    {
        if (GameManager.Instance.State == GameState.Start)
        {
            // 1. ���콺 �Է�(drag) �޴´�.
            float mouseX = Input.GetAxis("Mouse X");

            // 2. ���콺 �Է� ����ŭ x���� �����Ѵ�.
            _mx += mouseX * RotationSpeed * Time.deltaTime;
            _mx = Mathf.Clamp(_mx, -270f, 270f);

            // 3. ������ ���� ���� ȸ���Ѵ�.
            transform.eulerAngles = new Vector3(0, _mx, 0);
        }
    }
}
