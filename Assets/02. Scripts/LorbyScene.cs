using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum SceneNames
{
    Lobby,
    Loading,
    Main
}
public class LorbyScene : MonoBehaviour
{
    // ����� ������ ���� �����ϰų� (ȸ������), ����� �����͸� �о�(�α���)
    // ����� �Է°� ��ġ�ϴ��� �˻�(�α���)�Ѵ�.

    public TMP_InputField IDInputField;         // ���̵� �Է�â
    public TMP_InputField PasswordInputField;   // ��й�ȣ �Է�â
    public TextMeshProUGUI NotifyTextUI;        // �˸� �ؽ�Ʈ

    private void Start()
    {
        IDInputField.text = string.Empty;
        PasswordInputField.text = string.Empty;
        NotifyTextUI.text = string.Empty;
    }

    // ȸ������ ��ư Ŭ��
    public void OnClickRegisterButton()
    {
        string id = IDInputField.text;
        string pw = PasswordInputField.text;
        // 0. ���̵� �Ǵ� ��й�ȣ�� �Է����� ���� ���
        if (id == string.Empty || pw == string.Empty)
        {
            NotifyTextUI.text = "���̵�� ��й�ȣ�� ��Ȯ�ϰ� �Է����ּ���";
            return;
        }
        // 1. �̹� ���� �������� ȸ�������� �Ǿ��ִ� ���
        if (PlayerPrefs.HasKey(id)) 
        {
            NotifyTextUI.text = "�̹� �����ϴ� �����Դϴ�.";
        }
        // 2. ȸ�� ���Կ� �����ϴ� ���
        else 
        {
            NotifyTextUI.text = "ȸ�����Կ� �����Ͽ����ϴ�.";
            PlayerPrefs.SetString(id, pw);
        }
        IDInputField.text = string.Empty;
        PasswordInputField.text = string.Empty;
    }

    // �α��� ��ư Ŭ��
    public void OnClickLoginButton()
    {
        string id = IDInputField.text;
        string pw = PasswordInputField.text;
        // 0. ���̵� �Ǵ� ��й�ȣ �Է� X -> "���̵�� ��й�ȣ�� ��Ȯ�ϰ� �Է����ּ���"
        if (id == string.Empty || pw == string.Empty)
        {
            NotifyTextUI.text = "���̵�� ��й�ȣ�� ��Ȯ�ϰ� �Է����ּ���";
            return;
        }
        // 1. ���� ���̵� �� ���         -> "���̵�� ��й�ȣ�� Ȯ�����ּ���."
        if (!PlayerPrefs.HasKey(id))
        {
            NotifyTextUI.text = "���̵� Ȯ�����ּ���";
            return;
        }
        // 2. Ʋ�� ��й�ȣ               -> "���̵�� ��й�ȣ�� Ȯ�����ּ���."
        if (pw != PlayerPrefs.GetString(id))
        {
            NotifyTextUI.text = "��й�ȣ�� Ȯ�����ּ���.";
            return;
        }
        // 3. �α��� ����                 -> ���� ������ �̵�
        SceneManager.LoadScene((int)SceneNames.Loading); 
        GameManager.Instance.Continue();
    }
    
}
