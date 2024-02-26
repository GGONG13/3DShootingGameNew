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
    public Transform Target; // 플레이어
    private Vector3 _monsterPosition;
    private Vector3 _dir;


    private Vector3 _itemStartPos;
    public float triggerDistant = 2f;
    public float movingSpeed = 0.3f;
    private float _movingProgress = 0f; // 시간을 저장할 변수

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        _ItemState = ItemState.Idle;
        _itemStartPos = transform.position;
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

        float distante = Vector3.Distance(transform.position, Target.position);
        transform.Rotate(0, 200 * Time.deltaTime, 0);
        if (distante < triggerDistant)
        {
            _ItemState = ItemState.Moving;
            Debug.Log("무빙으로 바뀌는 중");
        }
    }
    public void Init()
    {
        _movingProgress = 0;
        _movingCoroutine = null;
        _ItemState = ItemState.Idle;
    }
    private Coroutine _movingCoroutine;
    void Moving()
    {

       StartCoroutine(Moving_Coroutine());

    }

    private IEnumerator Moving_Coroutine() 
    {
        /*        if (_movingProgress == 0f)
                {
                    _itemStartPos = transform.position;
                    Vector3 dir = transform.position - Target.position;
                    dir.Normalize();
                }*/
        
        _movingProgress += Time.deltaTime / movingSpeed;
        _itemStartPos = transform.position;
        while (_movingProgress < 1)
        {
            transform.position = Vector3.Slerp(_itemStartPos, Target.position, _movingProgress);
            Debug.Log("아이템 이동 중");
            yield return null;
        }
        ItemManager.Instance.AddItem(itemType);
       // ItemManager.Instance.RefreshUI();
        Debug.Log(itemType + "아이템이 추가되었습니다.");
        this.gameObject.SetActive(false);


        /*
                if (_movingProgress > 1)
                {
                    _movingProgress = 0;

                    ItemManager.Instance.AddItem(itemType);
                    ItemManager.Instance.RefreshUI();
                    Debug.Log(itemType);
                    this.gameObject.SetActive(false);

                }*/


    }

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다 (도형이나 색을 다르게해서 구별되게)

    
    // 과제 31. 몬스터가 죽으면 아이템이 드랍 (Health 20%, Stamina 20%, Bullet 10%)
    // 과제 32. 일정거리가 되면 아이템이 베지어 곡선으로 날아오게 (시작 : 아이템, 끝 : 나, 중간 : 랜덤) 

}
