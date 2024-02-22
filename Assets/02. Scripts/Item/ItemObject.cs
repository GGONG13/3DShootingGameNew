using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum ItemState
{
    Idle,
    Moving
}


public class ItemObject : MonoBehaviour
{
   public ItemType itemType;

    private ItemState _ItemState = ItemState.Idle;
    public Transform Target; // �÷��̾�
    private Vector3 _monsterPosition;
    private Vector3 _dir;


    private Vector3 _itemStartPos;
    private Vector3 _itemEndPos;
    public float triggerDistant = 2f;
    public float movingSpeed = 2f;
    private float _movingProgress = 0f;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        _ItemState = ItemState.Idle;
    }

    private void Update()
    {
        switch (_ItemState)
        {
            case ItemState.Idle:
                itemIdle();
                break;
            case ItemState.Moving:
                Moving();
                break;
        }

    }

    void itemIdle()
    {

        if (Vector3.Distance(transform.position, Target.position) < triggerDistant)
        {
            _ItemState = ItemState.Moving;
            Debug.Log("�������� �ٲ�� ��");
        }
    }

    void Moving()
    {
        if (_movingProgress == 0f)
        {
            _itemStartPos = transform.position;
            Vector3 dir = transform.position - Target.position;
            dir.Normalize();
        }
        
        _movingProgress += Time.deltaTime / 1;
        transform.position = Vector3.Slerp(_itemStartPos, Target.position, _movingProgress);
        Debug.Log("������ �̵� ��");

        if (_movingProgress > 1)
        {
            _movingProgress = 0;

            ItemManager.Instance.AddItem(itemType);
            ItemManager.Instance.RefreshUI();
            Debug.Log(itemType);
            this.gameObject.SetActive(false);

        }
    }

    // Todo 1. ������ �������� 3��(Health, Stamina, Bullet) ����� (�����̳� ���� �ٸ����ؼ� �����ǰ�)

    
    // ���� 31. ���Ͱ� ������ �������� ��� (Health 20%, Stamina 20%, Bullet 10%)
    // ���� 32. �����Ÿ��� �Ǹ� �������� ������ ����� ���ƿ��� (���� : ������, �� : ��, �߰� : ����) 

}
