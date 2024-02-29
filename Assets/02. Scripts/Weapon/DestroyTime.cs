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
