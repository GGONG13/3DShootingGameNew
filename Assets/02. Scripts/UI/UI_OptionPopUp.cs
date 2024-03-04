using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // �׻� ���� �ִ� ���� �׻� �ε��ϰڴٴ� �ε��� ��
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // ���Ŵ����� ()�� ���� �ε��Ϸ�
        SceneManager.LoadScene(0); // ���� ���� 0������ ������
        GameManager.Instance.Continue();
    }
    public void OnGameOverButtonClicked()
    {
        Debug.Log("�������� ��ư�� Ŭ���߽��ϴ�.");
        // ���� �� ���� ���� ��� �����ϴ� ���
        Application.Quit();

        // GameManager.Instance.GameOver();

#if UNITY_EDITOR
        // ����Ƽ �����Ϳ��� �������� ��� �����ϴ� ���
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
