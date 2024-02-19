using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 3인칭 슈팅 (Third Person Shooter)
// 게임상의 캐릭터가 보는 시점이 아닌, 캐릭터를 보는 시점 즉, 3인칭 관찰자 시점의 카메라
public class TPSCamera : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 3f, -3f);

    private void CalculateCameraAngles()
    {
        Vector3 targetToCameraVector = transform.position - Target.position;

        // 수평 회전 각도 x를 계산합니다.
        float x = Mathf.Atan2(targetToCameraVector.x, targetToCameraVector.z) * Mathf.Rad2Deg;

        // 수직 회전 각도 y를 계산합니다.
        // 타겟으로부터 카메라까지의 벡터를 수평면에 투영한 후, 이와 카메라 방향 벡터 사이의 각도를 계산합니다.
        float y = Mathf.Asin(targetToCameraVector.y / targetToCameraVector.magnitude) * Mathf.Rad2Deg;
    }

    public float a;
    private void LateUpdate()
    {
        transform.position = Target.position + Offset;
        transform.LookAt(Target);

        Vector2 xy = CameraManager.Instance.XY;
        transform.RotateAround(Target.position, Vector3.up, xy.x);
        transform.RotateAround(Target.position, transform.right, -xy.y);


        transform.position = Target.position - transform.forward * Offset.magnitude + Vector3.up * (Offset.y - a);
    }

}
