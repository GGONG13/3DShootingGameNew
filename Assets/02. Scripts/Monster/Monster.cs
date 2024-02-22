using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum MonsterState // 몬스터의 상태
{
    Idle,              // 대기
    Trace,             // 추적
    attack,            // 공격
    Return,            // 복귀
    Damage,            // 상처
    HitedDie                // 죽음
}

public class Monster : MonoBehaviour, iHitalbe
{
    private MonsterState _CurrentState = MonsterState.Idle;

    [Range(0f, 100f)]
    public int Health;
    public int MaxHealth = 100;
    [Header("몬스터 체력 슬라이더 UI")]
    public Slider MonsterSliderUI;
    /*    [Header("아이템 프리펩")]
        public GameObject HealthPrefab;
        public GameObject StaminaPrefab;
        public GameObject BulletPrefab;*/
    public Transform Target; // 플레이어
    private Vector3 _monsterPosition;
    private Vector3 _dir;
    public float FindDistance = 6;
    public float AttactDistance = 1;
    public float moveSpeed = 10f;
    public float moveDistance = 40f;
    private CharacterController _characterController;

    void Start()
    {
        Init();
       _monsterPosition = transform.position;
        Target = GameObject.FindGameObjectWithTag("Player").transform;
       _characterController = GetComponent<CharacterController>();
    }


    private void Update()
    {
        monsterUI();
        // 상태패턴 : 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나둔다
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다.
        switch(_CurrentState) 
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Trace:
                Trace();
                break;
            case MonsterState.attack:
                Attack();
                break;
            case MonsterState.Return:
                Comeback();
                break;
            case MonsterState.Damage:
                Damaged();
                break;
            case MonsterState.HitedDie:
                HitedDie();
                break;
        }
    }
    private void Idle()
    {
        // Idle 상태일때의 행동 코드 작성
        transform.LookAt(Target);

        // 몬스터의 아이들 애니메이션 재생
        // 플레이어와의 거리가 일정범위안이면
        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환 : Idle => Trace");
            _CurrentState = MonsterState.Trace;
        }
    }
    private void Trace()
    {
        // 플레이어에게 다가간다.

         
        _dir = Target.position - transform.position;
        _dir.y = 0;
        _dir.Normalize();
        _characterController.Move(_dir * moveSpeed * Time.deltaTime);
      //   transform.position += _dir * moveSpeed * Time.deltaTime;
        transform.LookAt(Target);
        Debug.Log("움직임?");
        if (Vector3.Distance(Target.position, transform.position) < AttactDistance) 
        {
            _CurrentState = MonsterState.attack;
        }
        if (Vector3.Distance(_monsterPosition, transform.position) > 20)
        {
            _CurrentState = MonsterState.Return;
        }
    }
    private void Attack() 
    {
        Debug.Log("상태 전환 : Trace => Attack");
        if (Vector3.Distance(_monsterPosition, transform.position) > 20)
        {
            _CurrentState = MonsterState.Return;
        }
    }

    private void Comeback() 
    {
        _dir = _monsterPosition - transform.position;
        _dir.y = 0;
        _dir.Normalize();
        _characterController.Move(_dir * moveSpeed * Time.deltaTime);
       // transform.LookAt(_monsterPosition);
        
        transform.forward = _dir;
        Debug.Log("상태 전환 : Attack => Comeback");

        if (Vector3.Distance(transform.position, _monsterPosition) < 1f)
        {
            _CurrentState = MonsterState.Idle;
            Debug.Log("상태 전환 : Comeback => Idle");
        }
    }
    private void Damaged()
    {
        _CurrentState = MonsterState.Damage;
        Debug.Log("현재 상태 : Damage");
        StartCoroutine(Damaged_coroutine());
        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환 : Damage => Trace");
            _CurrentState = MonsterState.Trace;
        }
    }
    private void HitedDie() 
    {
        
       
    }
    public void Hit(int damage)
    {
        Health -= damage;
        Damaged();
        monsterUI();
        if (Health <= 0)
        {
            Die();
            _CurrentState = MonsterState.HitedDie;
            // Destroy(gameObject);
        }
    }

    public void Init()
    {
        Health = MaxHealth;
        monsterUI();
    }


    public void monsterUI()
    {
        MonsterSliderUI.value = (float)Health / (float)MaxHealth;
    }

    void Die()
    {
        ItemObjectFactory.Instance.MakePercent(transform.position);
        this.gameObject.SetActive(false);
    }


    void DropItems() 
    {
       /* GameObject itemObject = null;
        float random = Random.Range(0, 50);
        if (random <= 20f)
        {
            itemObject = Instantiate(HealthPrefab);
            HealthPrefab.SetActive(true);
            HealthPrefab.transform.position = transform.position;
            GameObject player = GameObject.Find("player");
            Vector3 playerPosition = player.transform.position; 
            float number = Time.deltaTime * random;
            float distant = Vector3.Distance(HealthPrefab.transform.position, playerPosition);
            if (distant < 3)
            {
                transform.position = Vector3.Slerp(HealthPrefab.transform.position, playerPosition, number);
            }

        }
        else if (random <= 40)
        {
            itemObject = Instantiate(StaminaPrefab);
            StaminaPrefab.SetActive(true);
            StaminaPrefab.transform.position = transform.position;
            GameObject player = GameObject.Find("player");
            Vector3 playerPosition = player.transform.position;
            float number = Time.deltaTime * random;
            float distant = Vector3.Distance(StaminaPrefab.transform.position, playerPosition);
            if (distant < 3)
            {
                transform.position = Vector3.Slerp(StaminaPrefab.transform.position, playerPosition, number);
            }
        }
        else if (random <= 50)
        {
            itemObject = Instantiate(BulletPrefab);
            BulletPrefab.SetActive(true);
            BulletPrefab.transform.position = transform.position;
            GameObject player = GameObject.Find("player");
            Vector3 playerPosition = player.transform.position;
            float number = Time.deltaTime * random;
            float distant = Vector3.Distance(BulletPrefab.transform.position, playerPosition);
            if (distant < 3)
            {
                transform.position = Vector3.Slerp(BulletPrefab.transform.position, playerPosition, number);
            }
        }
        if (itemObject != null)
        {
            
        }*/
    }

    private IEnumerator Damaged_coroutine()
    {
        yield return new WaitForSeconds(1f);
        _CurrentState = MonsterState.Trace;
    }
}
