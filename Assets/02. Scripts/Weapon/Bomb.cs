using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Purchasing;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    // �÷��̾ �����ϰ� ��ü�� ������ �ڱ� �ڽ��� ������� �ϴ� �ڵ� �ۼ�
    // �ڱ� �ڽ��� ���� ������Ʈ�� ������� �ϴ� �ڵ� �ۼ�

    // �ǽ� ���� 8. ����ź�� ������ ��(�������) ���� ����Ʈ�� �ڱ� ��ġ�� �����ϱ�

    // ��ǥ : ������ ���� ���� ������ ��� ����
    // �ʿ� �Ӽ� :
    // - ���� 
    public float ExplosionRadius = 3;
    // ���� ���� :
    // 1. ���� �� 
    // 2. ���� �ȿ� �ִ� ��� �ݶ��̴��� ã�´�.
    // 3. ã�� �ݶ��̴� �߿��� Ÿ�� ������ (iHitable) ������Ʈ�� ã�´�.
    // 4. Hit()�Ѵ�.

    public GameObject bombeffect;
    public int Damage = 60;
    public int Health;
    private Collider[] _colliders = new Collider[10];
    private void OnCollisionEnter(Collision collision)
    {


        // ���� ���� :
        // 1. ���� �� 
        this.gameObject.SetActive(false); // â�� �ִ´�

        GameObject bomb = Instantiate(bombeffect);
        bomb.transform.position = this.transform.position;

        // 2. ���� �ȿ� �ִ� ��� �ݶ��̴��� ã�´�.
        int layer = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player");
        int count = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _colliders, layer);
        // Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, 1 << 8 | 1 << 9); <- �̷��� ����ص� ��
        // -> ������.������ �Լ��� Ư�� ����(radius)�ȿ� �ִ� Ư�� ���̾���� ���� ������Ʈ��
        //    �ݶ��̴� ������Ʈ���� ��� ã�� �迭�� ��ȯ�ϴ� �Լ�
        // ������ ���´� ���Ǿ�, ť��, ĸ��

        // 3. ã�� �ݶ��̴� �߿��� Ÿ�� ������ (iHitable) ������Ʈ�� ã�Ƽ� Hit()�Ѵ�.
        for (int i = 0; i < count; i++)
        {
            Collider collider = _colliders[i];
            iHitalbe hitalbe = collider.GetComponent<iHitalbe>();
            if (hitalbe != null )
            {
                hitalbe.Hit(Damage);
            }
        }




    }

}

