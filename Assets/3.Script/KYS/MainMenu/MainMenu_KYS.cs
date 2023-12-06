using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : MonoBehaviour
{
    // test dbaccessmanager

    // 게임 시작 버튼
    public void Btn_StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 로그인 버튼
    public void Btn_Login()
    {

    }

    private void ValidateLogin()
    {

    }

    // 회원가입 버튼
    public void Btn_SignUp()
    {

    }

    private void ShowSignUpPanel()
    {

    }

    // 회원가입 창의 제출 버튼
    public void Btn_Submit()
    {

    }

    private void ValidateSignUp()
    {

    }
}
