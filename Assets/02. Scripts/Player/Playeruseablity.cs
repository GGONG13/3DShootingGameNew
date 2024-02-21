using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playeruseablity : MonoBehaviour
{
    public Transform playerPositon;
    public float triggerDistance = 5f;
    public float speed = 1f; 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            bool result = ItemManager.Instance.TryUseItem(ItemType.Health);
            if (result)
            {
                // todo : ������ ȿ���� ���
                // todo : ��ƼŬ �ý��� ���
                ItemManager.Instance.RefreshUI();
            }
            else
            {
                Debug.Log("�������� �����մϴ�.");
            }
            Debug.Log("�ｺ ������ ���");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ItemManager.Instance.TryUseItem(ItemType.Stamina);
            ItemManager.Instance.RefreshUI();
            Debug.Log("���׹̳� ������ ���");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ItemManager.Instance.TryUseItem(ItemType.Bullet);
            ItemManager.Instance.RefreshUI();
            Debug.Log("�Ѿ� ������ ���");
        }
    }
    private void LateUpdate()
    {
        float distance = Vector3.Distance(transform.position, playerPositon.position);
        if (distance <= triggerDistance)
        {
            // Slerp�� ����� �������� �÷��̾�� �ε巴�� �̵���ŵ�ϴ�.
            transform.position = Vector3.Slerp(transform.position, playerPositon.position, speed * Time.deltaTime);
        }
    }
}
