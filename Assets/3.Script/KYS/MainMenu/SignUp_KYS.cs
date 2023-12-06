using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class SignUp_KYS : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _pwdInputField;

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
        // check if id exists
    }

    public void OnSignUpValidated()
    {
        // MainMenu_KYS.ShowProfile()
    }
}
