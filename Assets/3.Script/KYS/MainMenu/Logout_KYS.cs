using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class Logout_KYS : MonoBehaviour
{
    private const string MAINMENU_NAME = "MainMenu_KYS";

    public void Btn_Logout()
    {
        // 현재 씬이 MainMenu 씬이라면 프로필 패널 끄고 로그인 패널 보여주기
        if (SceneManager.GetActiveScene().name.Equals(MAINMENU_NAME))
            gameObject.GetComponent<MainMenu_KYS>().ShowLoginPanel();
        else // 현재 씬이 MainMenu 씬이 아니라면 MainMenu 씬으로 이동
            SceneManager.LoadSceneAsync(MAINMENU_NAME);

        GetComponent<MainMenu_KYS>().DisableStartButton();

        // 로그인 유저 데이터 리셋
        GameManager.Instance.RemoveLocalUserData();
    }
}