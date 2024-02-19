using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    // 플레이어를 제외하고 물체에 닿으면 자기 자신을 사라지게 하는 코드 작성
    // 자기 자신의 게임 오브젝트를 사라지게 하는 코드 작성

    // 실습 과제 8. 수류탄이 폭발할 때(사라질때) 폭발 이펙트를 자기 위치에 생성하기
    public GameObject bombeffect;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bomb = Instantiate(bombeffect);
        bomb.transform.position = this.transform.position;

        this.gameObject.SetActive(false);
    }
}
