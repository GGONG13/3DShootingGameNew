using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeleteType
{
    Destroy,
    Inactive,
}

public class DestroyTime : MonoBehaviour
{
    public DeleteType DeleteType;
    public float DestoryTime = 1.5f;
    private float _timer = 0;


    private void OnDisable()
    {
        // 여기에 _timer=0; 초기화화는 Init();함수 넣어서 초기화할수도 있음
        // 사라지기 전에 초기화하는 것
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= DestoryTime)
        {
            if (DeleteType == DeleteType.Destroy)
            {
                Destroy(this.gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                _timer = 0;
            }
        }
    }
}
