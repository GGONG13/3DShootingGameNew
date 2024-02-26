using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 역할 : 게임 관리자
// -> 게임 전체의 상태를 알리고, 시작과 끝을 텍스트로 나타낸다.

public enum GameState
{
    Ready, // 대기
    Start, // 시작
    Over   // 오버
}

public class GameManager : MonoBehaviour
{
    // 게임의 상태는 처음에 "준비" 상태
    public GameState State { get; private set; } = GameState.Ready;
    public Text StateTextUI;
    //    public Color StateColor; 이런식으로 외부에서 텍스트에 색을 따로 입힐수도 있음

    // 게임 상태
    // 1. 게임 준비 상태 (Ready)
    // 2. 1.6초 후에 게임 시작 상태 (Start!)
    // 3. 0.4초 후에 텍스트 사라지고..
    // 4. 플레이를 하다가
    // 5. 플레이어 체력이 0이 되면 "게임 오버" 상태

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
        // 게임 상태
        // 1. 게임 준비 상태 (Ready)
        State = GameState.Ready;
        StateTextUI.gameObject.SetActive(true);
        Refresh();
        // 2. 1.6초 후에 게임 시작 상태 (Start!)
        yield return new WaitForSeconds(1.6f);
        State = GameState.Start;
        Refresh();
        // 3. 0.4초 후에 텍스트 사라지고..
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
            StateTextUI.text = "게임 시작!";
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
