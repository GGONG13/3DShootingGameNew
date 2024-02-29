using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OptionPopUp : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        // ����, ȿ���� ��
        // �ʱ�ȭ �Լ� ��
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        // ����, ȿ���� ��
        // �ʱ�ȭ �Լ� ��
        gameObject.SetActive(false);
    }

    public void OnContinueButtonClicked()
    {
        Debug.Log("����ϱ� ��ư�� Ŭ���߽��ϴ�.");
        Close();
        GameManager.Instance.Continue();
    }
    public void OnReplayButtonClicked() 
    {
        Debug.Log("�ٽ��ϱ� ��ư�� Ŭ���߽��ϴ�.");
    }
    public void OnGameOverButtonClicked()
    {
        Debug.Log("�������� ��ư�� Ŭ���߽��ϴ�.");
        Close ();
        GameManager.Instance.GameOver();
    }
}
