using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Purchasing;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    // 플레이어를 제외하고 물체에 닿으면 자기 자신을 사라지게 하는 코드 작성
    // 자기 자신의 게임 오브젝트를 사라지게 하는 코드 작성

    // 실습 과제 8. 수류탄이 폭발할 때(사라질때) 폭발 이펙트를 자기 위치에 생성하기

    // 목표 : 수류찬 폭발 범위 데미지 기능 구현
    // 필요 속성 :
    // - 범위 
    public float ExplosionRadius = 3;
    // 구현 순서 :
    // 1. 터질 때 
    // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
    // 3. 찾은 콜라이더 중에서 타격 가능한 (iHitable) 오브젝트를 찾는다.
    // 4. Hit()한다.

    public GameObject bombeffect;
    public int Damage = 60;
    public int Health;
    private Collider[] _colliders = new Collider[10];
    private void OnCollisionEnter(Collision collision)
    {


        // 구현 순서 :
        // 1. 터질 때 
        this.gameObject.SetActive(false); // 창고에 넣는다

        GameObject bomb = Instantiate(bombeffect);
        bomb.transform.position = this.transform.position;

        // 2. 범위 안에 있는 모든 콜라이더를 찾는다.
        int layer = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player");
        int count = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _colliders, layer);
        // Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, 1 << 8 | 1 << 9); <- 이렇게 사용해도 됨
        // -> 피직스.오버랩 함수는 특정 영역(radius)안에 있는 특정 레이어들의 게임 오브젝트의
        //    콜라이더 컴포넌트들을 모두 찾아 배열로 반환하는 함수
        // 영역의 형태는 스피어, 큐브, 캡슐

        // 3. 찾은 콜라이더 중에서 타격 가능한 (iHitable) 오브젝트를 찾아서 Hit()한다.
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

