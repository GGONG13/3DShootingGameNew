using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    // �÷��̾ �����ϰ� ��ü�� ������ �ڱ� �ڽ��� ������� �ϴ� �ڵ� �ۼ�
    // �ڱ� �ڽ��� ���� ������Ʈ�� ������� �ϴ� �ڵ� �ۼ�

    // �ǽ� ���� 8. ����ź�� ������ ��(�������) ���� ����Ʈ�� �ڱ� ��ġ�� �����ϱ�
    public GameObject bombeffect;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bomb = Instantiate(bombeffect);
        bomb.transform.position = this.transform.position;

        this.gameObject.SetActive(false);
    }
}
