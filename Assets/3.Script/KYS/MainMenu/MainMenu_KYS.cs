using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : MonoBehaviour // temp
{
    [SerializeField] private GameObject _logInSignUpPanel;
    [SerializeField] private GameObject _userProfilePanel;
    [SerializeField] private GameObject _logoutButton;

    [SerializeField] private PlayerProfile _profile;

    private void Awake()
    {
        ShowLoginPanel();
    }

    // 게임 시작 버튼
    public void Btn_StartGame(string sceneName)
    {
        // check if already full
        SceneManager.LoadScene(sceneName);
    }

    public void ShowLoginPanel()
    {
        _logInSignUpPanel.SetActive(true);
        _userProfilePanel.SetActive(false);
        _logoutButton.SetActive(false);
    }

    public void ShowProfile()
    {
        _logInSignUpPanel.SetActive(false);
        _logoutButton.SetActive(true);

        _profile.Init(DbAccessManager_KYS.Instance.UserData);

        _userProfilePanel.SetActive(true);
    }
}
