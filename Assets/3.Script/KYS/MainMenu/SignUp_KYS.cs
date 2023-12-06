using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class SignUp_KYS : MonoBehaviour
{
    [SerializeField] private GameObject _signUpModal;

    [SerializeField] private GameObject _nickNameInvalidationText;
    [SerializeField] private GameObject _idInvalidationText;

    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _pwdInputField;

    private void Awake()
    {
        _nickNameInvalidationText.SetActive(false);
        _idInvalidationText.SetActive(false);
    }

    // 회원가입 버튼
    public void Btn_OpenSignUpModal()
    {
        _nicknameInputField.text = string.Empty;
        _idInputField.text = string.Empty;
        _pwdInputField.text = string.Empty;

        _signUpModal.SetActive(true);
    }

    // 회원가입 창 닫기 버튼
    public void Btn_CloseSignUpModal() => _signUpModal.SetActive(false);

    // 회원가입 창의 제출 버튼
    public void Btn_Submit()
    {
        if (CheckSignUpValidation())
            SignUp();
    }

    private bool CheckSignUpValidation()
    {
        return CheckIfIdValid() && CheckIfNicknameValid();
    }

    // 아이디 중복 검사
    private bool CheckIfIdValid()
    {
        string cmdTxt = $"SELECT * FROM user WHERE id = \"{_idInputField.text}\"";

        var res = DbAccessManager_KYS.Instance.Select(cmdTxt);

        bool isValid = string.IsNullOrEmpty(res);

        _idInvalidationText.SetActive(!isValid);

        return isValid;
    }

    // 닉네임 중복 검사
    private bool CheckIfNicknameValid()
    {
        string cmdTxt = $"SELECT * FROM user WHERE nickname = \"{_nicknameInputField.text}\"";

        var res = DbAccessManager_KYS.Instance.Select(cmdTxt);

        bool isValid = string.IsNullOrEmpty(res);

        _nickNameInvalidationText.SetActive(!isValid);

        return isValid;
    }


    private int test = 6;

    private void SignUp()
    {
        // TODO : get rid of test
        string cmdTxt = $"INSERT INTO user VALUES (\"{_idInputField.text}\", \"{_pwdInputField.text}\", \"{_nicknameInputField.text}\", {test}, {test}, {test}, {test}, {test});";

        var res = DbAccessManager_KYS.Instance.Insert(cmdTxt);

        Debug.Log("SignUp " + res);
    }

    public void OnSignUpValidated()
    {
        // MainMenu_KYS.ShowProfile()
    }
}
