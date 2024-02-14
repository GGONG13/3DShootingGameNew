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


    /** ī�޶� ȸ�� **/
    // ��ǥ: ���콺�� �����ϸ� ī�޶� ĳ���� �߽ɿ� ���� �� �������� ȸ����Ű�� �ʹ�.
    // �ʿ� �Ӽ�:
    // - ȸ�� �ӵ�
    public float RotationSpeed = 200;
    // - ������ x������ y����
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
