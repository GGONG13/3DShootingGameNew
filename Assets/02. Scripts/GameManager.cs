using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� : ���� ������
// -> ���� ��ü�� ���¸� �˸���, ���۰� ���� �ؽ�Ʈ�� ��Ÿ����.

public enum GameState
{
    Ready, // ���
    Start, // ����
    Over   // ����
}

public class GameManager : MonoBehaviour
{
    // ������ ���´� ó���� "�غ�" ����
    public GameState State { get; private set; } = GameState.Ready;
    public Text StateTextUI;
    //    public Color StateColor; �̷������� �ܺο��� �ؽ�Ʈ�� ���� ���� �������� ����

    // ���� ����
    // 1. ���� �غ� ���� (Ready)
    // 2. 1.6�� �Ŀ� ���� ���� ���� (Start!)
    // 3. 0.4�� �Ŀ� �ؽ�Ʈ �������..
    // 4. �÷��̸� �ϴٰ�
    // 5. �÷��̾� ü���� 0�� �Ǹ� "���� ����" ����

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine(Start_Coroutine());
    }

    private IEnumerator Start_Coroutine()
    {
        // ���� ����
        // 1. ���� �غ� ���� (Ready)
        State = GameState.Ready;
        StateTextUI.gameObject.SetActive(true);
        Refresh();
        // 2. 1.6�� �Ŀ� ���� ���� ���� (Start!)
        yield return new WaitForSeconds(1.6f);
        State = GameState.Start;
        Refresh();
        // 3. 0.4�� �Ŀ� �ؽ�Ʈ �������..
        yield return new WaitForSeconds(0.4f);
        StateTextUI.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        State = GameState.Over;
        StateTextUI.gameObject.SetActive(true);
        Refresh();
    }
    public void Refresh()
    {
        switch(State)
        {
            case GameState.Ready:
            StateTextUI.text = "Ready...";
            StateTextUI.color = Color.white;
            StateTextUI.fontSize = 200;
                break;
            case GameState.Start:
            StateTextUI.text = "���� ����!";
            StateTextUI.color = new Color(1f, 0.5506037f, 0f, 1f);
            StateTextUI.fontStyle = FontStyle.Bold;
            StateTextUI.fontSize = 120;
                break;
            case GameState.Over:
            StateTextUI.text = "GameOver";
            StateTextUI.color = Color.cyan;
            StateTextUI.fontStyle = FontStyle.Normal;
            StateTextUI.fontSize = 140;
                break;
        }
    }
}
