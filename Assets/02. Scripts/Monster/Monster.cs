using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static DamageInfo;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum MonsterState // 몬스터의 상태
{
    Idle,              // 대기
    Trace,             // 추적
    attack,            // 공격
    Return,            // 복귀
    Damage,            // 상처
    HitedDie,          // 죽음
    Browsing           // 순찰
}


public class Monster : MonoBehaviour, iHitalbe
{
    private MonsterState _CurrentState = MonsterState.Idle;
    private ItemState _ItemState = ItemState.Idle;
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
    public float AttactDistance = 2.5f;
    public float FollowDistance = 2f;
    public float moveSpeed = 10f;
    public float moveDistance = 40f;
    public float _timer = 0;

    float _attackTimer = 2f;
    // private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private const float KNOCKBACK_Duration = 0.2f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.5f;
    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;

    public Transform BrowsingTarget;
    public float _waitTime = 0;
    private const float IDLE_DURATION = 3;
    float _progress;
    private bool _isMoving = true;

    public GameObject BloodPrab;

    void Start()
    {        
        Init();
        _ItemState = ItemState.Idle;
       _monsterPosition = transform.position;
        Target = GameObject.FindGameObjectWithTag("Player").transform;
      // _characterController = GetComponent<CharacterController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = moveSpeed;
        _animator = GetComponentInChildren<Animator>(); 
        // 현재 몬스터에 애니메이터가 달려있지 않기 때문에 자식안에서 찾아야함
    }


    private void Update()
    {
        monsterUI();
        
        
        // 상태패턴 : 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나둔다
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다.
        switch (_CurrentState) 
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
            case MonsterState.Browsing:
                Browsing();
                break;
        }

    }
    private void Idle()
    {
        // Idle 상태일때의 행동 코드 작성
        transform.LookAt(Target);
        _waitTime += Time.deltaTime;
        // 몬스터의 아이들 애니메이션 재생
        // 플레이어와의 거리가 일정범위안이면
        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환 : Idle => Trace");
            _CurrentState = MonsterState.Trace;
            _animator.SetTrigger("IdleToTrace");
            _waitTime = 0;
        }

        if (BrowsingTarget != null && _waitTime >= IDLE_DURATION) 
        {
            _waitTime = 0;
            _CurrentState = MonsterState.Browsing;
            Debug.Log("상태 전환 : Idle => Browsing");
            _animator.SetTrigger("IdleToBrowsing");
        }
  
    }
    private void Trace()
    {
        // 플레이어에게 다가간다.

         
        _dir = Target.position - transform.position;
        _dir.y = 0;
        _dir.Normalize();
        // _characterController.Move(_dir * moveSpeed * Time.deltaTime);
        //   transform.position += _dir * moveSpeed * Time.deltaTime;
        //   transform.LookAt(Target);

        // 네비게이션이 접근하는 최소 거리를 공격 가능한 거리로 설정
        _navMeshAgent.stoppingDistance = AttactDistance;

        // 네비게이션의 목적지를 플레이어의 위치로 한다. 지금까지 썼던 코드는 하위호완이라 주석처리
        _navMeshAgent.destination = Target.position;

        Debug.Log("현재 상태 : Trace");
        if (Vector3.Distance(Target.position, transform.position) > AttactDistance) 
        {
            _CurrentState = MonsterState.attack;
            Debug.Log("상태 전환 : Trace => Attack");
            _animator.SetTrigger("TraceToAttack");
        }
        if (Vector3.Distance(_monsterPosition, transform.position) > 20)
        {
            _CurrentState = MonsterState.Return;
            _animator.SetTrigger("TraceToComeBack");
        }
    }
    private void Attack() 
    {
        _timer += Time.deltaTime;
        if (Vector3.Distance(Target.position, transform.position) > FollowDistance)
        {
            _CurrentState = MonsterState.Trace;
            Debug.Log("상태 전환 : Attack => Trace");
            _animator.SetTrigger("AttacToTrace");
        }

        if (_timer >= _attackTimer && Vector3.Distance(Target.position, transform.position) > 10f)
        {
            PlayerAttack();
        }
    }

    public void PlayerAttack()
    {
        iHitalbe iHitalbe = Target.GetComponent<iHitalbe>();
        int damage = 10;
        if (iHitalbe != null)
        {
            Debug.Log("때렸다!");
            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, damage);
            _animator.SetTrigger("Attack");
            iHitalbe.Hit(damageInfo);
            _timer = 0;
        }
    }

    private void Comeback() 
    {
        _dir = _monsterPosition - transform.position;
        _dir.y = 0;
        _dir.Normalize();

        // 네비게이션이 접근하는 최소 거리를 오차 범위로 설정
         _navMeshAgent.stoppingDistance = 2;

        // 네비게이션의 목적지를 원래의 몬스터의 위치로 한다. 지금까지 썼던 코드는 하위호완이라 주석처리
        _navMeshAgent.destination = _monsterPosition;

        //  _characterController.Move(_dir * moveSpeed * Time.deltaTime);
        // transform.LookAt(_monsterPosition);

        // transform.forward = _dir;
        Debug.Log("상태 전환 : Attack => Comeback");
        _animator.SetTrigger("AttacToComeBack");

        if (Vector3.Distance(transform.position, _monsterPosition) <= 2)
        {
            _CurrentState = MonsterState.Idle;
            _animator.SetTrigger("ComeBackToIdle");
            Debug.Log("상태 전환 : Comeback => Idle");
        }
    }
    private void Damaged()
    {
        // 1. Damaged 애니메이션 실행 (0.5초)
        // todo : 애니메이션 실행
        // 2. Lerp 이용해서 넉백 (0.5초)
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0) 
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - Target.position;
            dir.y = 0;
            dir.Normalize();
            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_Duration;

        // 2-2. Lerp를 이용해서 넉백하기
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        _CurrentState = MonsterState.Damage;
        Debug.Log("현재 상태 : Damage");
        _animator.SetTrigger("Damaged");
        // StartCoroutine(Damaged_coroutine());
        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0;
            Debug.Log("상태 전환 : Damage => Trace");
            _animator.SetTrigger("DamagedToTrace");
            _CurrentState = MonsterState.Trace;
        }
    }
    private void HitedDie() 
    {
        _CurrentState = MonsterState.HitedDie;
        Debug.Log("몬스터 죽음");
    }

    private void Browsing()
    {
        _navMeshAgent.stoppingDistance = 0;
       // _navMeshAgent.destination = BrowsingTarget.position;
        _navMeshAgent.SetDestination(BrowsingTarget.position);

        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환 : Idle => Trace");
            _animator.SetTrigger("IdleToTrace");
            _CurrentState = MonsterState.Trace;
        }
        else if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= 0.2 && _navMeshAgent.remainingDistance != Mathf.Infinity)
        {
            _CurrentState = MonsterState.Return;
            Debug.Log("상태 전환 : Browsing => Comeback");
            _animator.SetTrigger("BrowsingToComeBack");
        }
    }

    public void Hit(DamageInfo damage)
    {

        if (damage.DamageType == DamageType.Cirtical)
        {
            // 블러드를 팩토리 패턴으로 구현하기 
             BloodFactory.Instance.Make(damage.Position, damage.Normal);
        }
        Health -= damage.Amount;
        Damaged();
        monsterUI();
        if (Health <= 0)
        {
            StartCoroutine(DieAnim());
            HitedDie();
        }
    }

    public void Init()
    {
        _waitTime = 0;
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

    private IEnumerator DieAnim() 
    {
        _animator.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        if (null != _animator)
        {
            this.gameObject.SetActive(false);
            ItemObjectFactory.Instance.MakePercent(transform.position);
        }
        yield return null;

    }
    private IEnumerator Damaged_coroutine()
    {
        yield return new WaitForSeconds(1f);
        _CurrentState = MonsterState.Trace;
    }
}
