using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : NetworkBehaviour // temp
{
    [SerializeField] private GameObject _logInSignUpPanel;
    [SerializeField] private GameObject _userProfilePanel;
    [SerializeField] private GameObject _logoutButton;

    private void Awake()
    {
        // 로그인 server에 클라이언트로서 연결


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

    //[TargetRpc]
    //public void TargetShowProfile()
    //{
    //    _logInSignUpPanel.SetActive(false);
    //    _logoutButton.SetActive(true);

    //    _userProfilePanel.GetComponent<PlayerProfile>().Init(GameManager.Instance.LocalUserData);

    //    _userProfilePanel.SetActive(true);
    //}

    public void ShowProfile()
    {
        _logInSignUpPanel.SetActive(false);
        _logoutButton.SetActive(true);

        _userProfilePanel.GetComponent<PlayerProfile>().Init(GameManager.Instance.LocalUserData);

        _userProfilePanel.SetActive(true);
    }
}
