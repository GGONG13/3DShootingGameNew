using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    Start,
    FPS,
    TPS,
    Top,
    Back,
}

// ����: ī�޶� �����ϴ� ������
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private FPSCamera _FPSCamera;
    private TPSCamera _TPSCamera;

    public float RotationSpeed = 400;

    public float X { get; private set; }
    public float Y { get; private set; }

    public Vector2 XY => new Vector2(X, Y);

    public CameraMode Mode = CameraMode.Start;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _FPSCamera = GetComponent<FPSCamera>();
        _TPSCamera = GetComponent<TPSCamera>();
    }

    private void Start()
    {
        // ���콺 Ŀ�� ���ְ�, ����
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SetCameraMode(CameraMode.FPS);
    }

    private void LateUpdate()
    {
        // 3. ���콺 �Է��� �޴´�.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 4. ���콺 �Է¿� ���� ȸ�� ������ �����Ѵ�.
        X += mouseX * RotationSpeed * Time.deltaTime;
        Y += mouseY * RotationSpeed * Time.deltaTime;

        Y = Mathf.Clamp(Y, -90, 90);
    }

    public void SetCameraMode(CameraMode mode)
    {
        if (Mode == mode)
        {
            return;
        }

        if (Mode == CameraMode.FPS)
        {
            //Y += _TPSCamera.Offset.y;
        }

        Vector3 currentRotation = transform.eulerAngles;

        Mode = mode;

        //X = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        //Y = Mathf.Atan2(forward.y, Mathf.Sqrt(forward.x * forward.x + forward.z * forward.z)) * Mathf.Rad2Deg;


        _FPSCamera.enabled = (mode == CameraMode.FPS);
        _TPSCamera.enabled = (mode == CameraMode.TPS);
    }
}