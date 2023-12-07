using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : MonoBehaviour // temp
{
    [SerializeField] private GameObject _logInSignUpPanel;
    [SerializeField] private GameObject _userProfilePanel;

    [SerializeField] private TMP_Text _nickName;
    [SerializeField] private TMP_Text _level;

    private void Awake()
    {
        ShowLoginPanel();
    }

    // 게임 시작 버튼
    public void Btn_StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowLoginPanel()
    {
        _logInSignUpPanel.SetActive(true);
        _userProfilePanel.SetActive(false);
    }

    public void ShowProfile()
    {
        _logInSignUpPanel.SetActive(false);

        _nickName.text = DbAccessManager_KYS.Instance.UserData.Nickname;
        _level.text = $"{DbAccessManager_KYS.Instance.UserData.Level}";

        _userProfilePanel.SetActive(true);
    }
}
