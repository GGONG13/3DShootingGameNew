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

public enum MonsterState // ������ ����
{
    Idle,              // ���
    Trace,             // ����
    attack,            // ����
    Return,            // ����
    Damage,            // ��ó
    HitedDie,          // ����
    Browsing           // ����
}


public class Monster : MonoBehaviour, iHitalbe
{
    private MonsterState _CurrentState = MonsterState.Idle;
    private ItemState _ItemState = ItemState.Idle;
    [Range(0f, 100f)]
    public int Health;
    public int MaxHealth = 100;
    [Header("���� ü�� �����̴� UI")]
    public Slider MonsterSliderUI;
    /*    [Header("������ ������")]
        public GameObject HealthPrefab;
        public GameObject StaminaPrefab;
        public GameObject BulletPrefab;*/
    public Transform Target; // �÷��̾�
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
        // ���� ���Ϳ� �ִϸ����Ͱ� �޷����� �ʱ� ������ �ڽľȿ��� ã�ƾ���
    }


    private void Update()
    {
        monsterUI();
        
        
        // �������� : ���¿� ���� �ൿ�� �ٸ��� �ϴ� ����
        // 1. ���Ͱ� ���� �� �ִ� �ൿ�� ���� ���¸� ���д�
        // 2. ���µ��� ���ǿ� ���� �ڿ������� ��ȯ(Transition)�ǰ� �����Ѵ�.
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
        // Idle �����϶��� �ൿ �ڵ� �ۼ�
        transform.LookAt(Target);
        _waitTime += Time.deltaTime;
        // ������ ���̵� �ִϸ��̼� ���
        // �÷��̾���� �Ÿ��� �����������̸�
        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ : Idle => Trace");
            _CurrentState = MonsterState.Trace;
            _animator.SetTrigger("IdleToTrace");
            _waitTime = 0;
        }

        if (BrowsingTarget != null && _waitTime >= IDLE_DURATION) 
        {
            _waitTime = 0;
            _CurrentState = MonsterState.Browsing;
            Debug.Log("���� ��ȯ : Idle => Browsing");
            _animator.SetTrigger("IdleToBrowsing");
        }
  
    }
    private void Trace()
    {
        // �÷��̾�� �ٰ�����.

         
        _dir = Target.position - transform.position;
        _dir.y = 0;
        _dir.Normalize();
        // _characterController.Move(_dir * moveSpeed * Time.deltaTime);
        //   transform.position += _dir * moveSpeed * Time.deltaTime;
        //   transform.LookAt(Target);

        // �׺���̼��� �����ϴ� �ּ� �Ÿ��� ���� ������ �Ÿ��� ����
        _navMeshAgent.stoppingDistance = AttactDistance;

        // �׺���̼��� �������� �÷��̾��� ��ġ�� �Ѵ�. ���ݱ��� ��� �ڵ�� ����ȣ���̶� �ּ�ó��
        _navMeshAgent.destination = Target.position;

        Debug.Log("���� ���� : Trace");
        if (Vector3.Distance(Target.position, transform.position) > AttactDistance) 
        {
            _CurrentState = MonsterState.attack;
            Debug.Log("���� ��ȯ : Trace => Attack");
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
            Debug.Log("���� ��ȯ : Attack => Trace");
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
            Debug.Log("���ȴ�!");
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

        // �׺���̼��� �����ϴ� �ּ� �Ÿ��� ���� ������ ����
         _navMeshAgent.stoppingDistance = 2;

        // �׺���̼��� �������� ������ ������ ��ġ�� �Ѵ�. ���ݱ��� ��� �ڵ�� ����ȣ���̶� �ּ�ó��
        _navMeshAgent.destination = _monsterPosition;

        //  _characterController.Move(_dir * moveSpeed * Time.deltaTime);
        // transform.LookAt(_monsterPosition);

        // transform.forward = _dir;
        Debug.Log("���� ��ȯ : Attack => Comeback");
        _animator.SetTrigger("AttacToComeBack");

        if (Vector3.Distance(transform.position, _monsterPosition) <= 2)
        {
            _CurrentState = MonsterState.Idle;
            _animator.SetTrigger("ComeBackToIdle");
            Debug.Log("���� ��ȯ : Comeback => Idle");
        }
    }
    private void Damaged()
    {
        // 1. Damaged �ִϸ��̼� ���� (0.5��)
        // todo : �ִϸ��̼� ����
        // 2. Lerp �̿��ؼ� �˹� (0.5��)
        // 2-1. �˹� ����/���� ��ġ�� ���Ѵ�.
        if (_knockbackProgress == 0) 
        {
            _knockbackStartPosition = transform.position;
            Vector3 dir = transform.position - Target.position;
            dir.y = 0;
            dir.Normalize();
            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_Duration;

        // 2-2. Lerp�� �̿��ؼ� �˹��ϱ�
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        _CurrentState = MonsterState.Damage;
        Debug.Log("���� ���� : Damage");
        _animator.SetTrigger("Damaged");
        // StartCoroutine(Damaged_coroutine());
        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0;
            Debug.Log("���� ��ȯ : Damage => Trace");
            _animator.SetTrigger("DamagedToTrace");
            _CurrentState = MonsterState.Trace;
        }
    }
    private void HitedDie() 
    {
        _CurrentState = MonsterState.HitedDie;
        Debug.Log("���� ����");
    }

    private void Browsing()
    {
        _navMeshAgent.stoppingDistance = 0;
       // _navMeshAgent.destination = BrowsingTarget.position;
        _navMeshAgent.SetDestination(BrowsingTarget.position);

        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ : Idle => Trace");
            _animator.SetTrigger("IdleToTrace");
            _CurrentState = MonsterState.Trace;
        }
        else if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= 0.2 && _navMeshAgent.remainingDistance != Mathf.Infinity)
        {
            _CurrentState = MonsterState.Return;
            Debug.Log("���� ��ȯ : Browsing => Comeback");
            _animator.SetTrigger("BrowsingToComeBack");
        }
    }

    public void Hit(DamageInfo damage)
    {

        if (damage.DamageType == DamageType.Cirtical)
        {
            // ���带 ���丮 �������� �����ϱ� 
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
