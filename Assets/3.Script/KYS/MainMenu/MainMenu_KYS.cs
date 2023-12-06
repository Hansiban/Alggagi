using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : MonoBehaviour // temp
{
    // test dbaccessmanager

    // 게임 시작 버튼
    public void Btn_StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowProfile()
    {
        // disable Login_SignUp GO & enable Profile after fetching user data
    }
}
