using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CameraMode
{
    FPS,
    TPS,
    Top,
    Bottom
}
public class CameraManager : MonoBehaviour
{



    public static CameraManager Instance { get; private set; }
    public GameObject MainCamera;
    public FPSCamera FPSCamera;
    public TPSCamera TPSCamera;

    public CameraMode Mode { get; set; }


    /** 카메라 회전 **/
    // 목표: 마우스를 조작하면 카메라를 캐릭터 중심에 따라 그 방향으로 회전시키고 싶다.
    // 필요 속성:
    // - 회전 속도
    public float RotationSpeed = 200;
    // - 누적할 x각도와 y각도
    private float _mx = 0;
    private float _my = 0;

    private void Awake()
    {
        FPSCamera = MainCamera.GetComponent<FPSCamera>();
        TPSCamera = MainCamera.GetComponent<TPSCamera>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetCameraMode(CameraMode.FPS);
       /* SetCamera(true);*/
    }
    public void SetCameraMode(CameraMode mode)
    {
        Mode = mode;
/*        FPSCamera.enabled = (mode == CameraMode.FPS);*/
    }
    public void SetCamera(bool isFPS)
    {
        if (isFPS)
        {
            FPSCamera.enabled = true;
            TPSCamera.enabled = false;
        }
        else 
        {
            TPSCamera.enabled = true;
            FPSCamera.enabled = false;
        }
    }
}
