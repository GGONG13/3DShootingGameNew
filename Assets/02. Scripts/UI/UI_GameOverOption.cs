using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOverOption : MonoBehaviour
{


    private void Awake()
    {
        this.gameObject.SetActive(false);
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
