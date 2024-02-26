using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombFireAbility : MonoBehaviour
{
    // 목표 : 마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
    // 필요 속성
    // - 수류탄 프리팹
    public GameObject BombPrefab;
    // - 수류탄 던지는 위치
    public Transform FirePosition;
    // - 수류탄 던지는 파워
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
        // 수류탄 투척**
        // 구현 순서 :
        // 1. 마우스 오른쪽 버튼을 감지    
        
            if (Input.GetMouseButtonDown(1) && CurrentBomb > 0 && GameManager.Instance.State == GameState.Start)
        {
            CurrentBomb--;
            // 2. 수류탄 던지는 위치에다가 수류탄 생성
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

            // 3. 시선이 바라보는 방향(카메라가 바라 보는 방향 = 카메라의 전방)으로 수류탄 투척
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            // ForceMode - 어떻게 힘을 가할것인가? Impulse -> 순간적으로
            BombCountUI();
        }
    }


    void BombCountUI()
    {
        text.text = $"Bomb {CurrentBomb}/{Bombmax}"; 
    }



}
