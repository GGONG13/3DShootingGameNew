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

public enum MonsterState // ������ ����
{
    Idle,              // ���
    Trace,             // ����
    attack,            // ����
    Return,            // ����
    Damage,            // ��ó
    HitedDie                // ����
}

public class Monster : MonoBehaviour, iHitalbe
{
    private MonsterState _CurrentState = MonsterState.Idle;

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
        // �������� : ���¿� ���� �ൿ�� �ٸ��� �ϴ� ����
        // 1. ���Ͱ� ���� �� �ִ� �ൿ�� ���� ���¸� ���д�
        // 2. ���µ��� ���ǿ� ���� �ڿ������� ��ȯ(Transition)�ǰ� �����Ѵ�.
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
        // Idle �����϶��� �ൿ �ڵ� �ۼ�
        transform.LookAt(Target);

        // ������ ���̵� �ִϸ��̼� ���
        // �÷��̾���� �Ÿ��� �����������̸�
        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ : Idle => Trace");
            _CurrentState = MonsterState.Trace;
        }
    }
    private void Trace()
    {
        // �÷��̾�� �ٰ�����.

         
        _dir = Target.position - transform.position;
        _dir.y = 0;
        _dir.Normalize();
        _characterController.Move(_dir * moveSpeed * Time.deltaTime);
      //   transform.position += _dir * moveSpeed * Time.deltaTime;
        transform.LookAt(Target);
        Debug.Log("������?");
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
        Debug.Log("���� ��ȯ : Trace => Attack");
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
        Debug.Log("���� ��ȯ : Attack => Comeback");

        if (Vector3.Distance(transform.position, _monsterPosition) < 1f)
        {
            _CurrentState = MonsterState.Idle;
            Debug.Log("���� ��ȯ : Comeback => Idle");
        }
    }
    private void Damaged()
    {
        _CurrentState = MonsterState.Damage;
        Debug.Log("���� ���� : Damage");
        StartCoroutine(Damaged_coroutine());
        if (Vector3.Distance(Target.position, transform.position) <= FindDistance)
        {
            Debug.Log("���� ��ȯ : Damage => Trace");
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
