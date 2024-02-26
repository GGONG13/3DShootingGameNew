using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombFireAbility : MonoBehaviour
{
    // ��ǥ : ���콺 ������ ��ư�� ������ �ü��� �ٶ󺸴� �������� ����ź�� ������ �ʹ�.
    // �ʿ� �Ӽ�
    // - ����ź ������
    public GameObject BombPrefab;
    // - ����ź ������ ��ġ
    public Transform FirePosition;
    // - ����ź ������ �Ŀ�
    public float ThrowPower = 15f;


    public int poolSize = 10;
    private List<GameObject> pool;


    public int Bombmax = 10;
    public int CurrentBomb;

    public Text text;


    private void Start()
    {

        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);
            bomb.SetActive(false);
            pool.Add(bomb);
        }
        CurrentBomb = Bombmax;
        BombCountUI();
    }

    private void Update()
    {
        // ����ź ��ô**
        // ���� ���� :
        // 1. ���콺 ������ ��ư�� ����    
        
            if (Input.GetMouseButtonDown(1) && CurrentBomb > 0 && GameManager.Instance.State == GameState.Start)
        {
            CurrentBomb--;
            // 2. ����ź ������ ��ġ���ٰ� ����ź ����
            /*  GameObject bomb = Instantiate(BombPrefab);*/

            GameObject bomb = null;
            for (int j = 0; j < poolSize; j++)
            {
                if (pool[j].activeInHierarchy == false)
                {
                    bomb = pool[j];
                    bomb.SetActive(true);
                    break;
                }
            }
            bomb.transform.position = FirePosition.position;

            // 3. �ü��� �ٶ󺸴� ����(ī�޶� �ٶ� ���� ���� = ī�޶��� ����)���� ����ź ��ô
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            // ForceMode - ��� ���� ���Ұ��ΰ�? Impulse -> ����������
            BombCountUI();
        }
    }


    void BombCountUI()
    {
        text.text = $"Bomb {CurrentBomb}/{Bombmax}"; 
    }



}
