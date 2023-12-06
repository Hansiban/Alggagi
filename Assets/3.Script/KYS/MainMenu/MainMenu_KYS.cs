using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : MonoBehaviour // temp
{
    // test dbaccessmanager

    // 게임 시작 버튼
    public void Btn_StartGame(string sceneName)
    {
        Debug.Log("Login Button Pressed");

        string query = "select * from user";
        var dd = DbAccessManager_KYS.Instance.Select(query, "user");

        Debug.Log("결과 : " +dd);

        //SceneManager.LoadScene(sceneName);
    }

    public void ShowProfile()
    {
        // disable Login_SignUp GO & enable Profile after fetching user data
    }
}
