using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI Text;

    public int NextScene;

    private void Start()
    {
        LoadNextScene();
        Slider.gameObject.SetActive(true);
        Text.text = string.Empty;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene((int)NextScene);
    }
}
