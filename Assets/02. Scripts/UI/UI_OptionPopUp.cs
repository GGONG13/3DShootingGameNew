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
        // 사운드, 효과음 등
        // 초기화 함수 등
        gameObject.SetActive(true);
    }
    public void Close() 
    {
        // 사운드, 효과음 등
        // 초기화 함수 등
        gameObject.SetActive(false);
    }

    public void OnContinueButtonClicked()
    {
        Debug.Log("계속하기 버튼을 클릭했습니다.");
        Close();
        GameManager.Instance.Continue();
    }
    public void OnReplayButtonClicked() 
    {
        Debug.Log("다시하기 버튼을 클릭했습니다.");
    }
    public void OnGameOverButtonClicked()
    {
        Debug.Log("게임종료 버튼을 클릭했습니다.");
        Close ();
        GameManager.Instance.GameOver();
    }
}
