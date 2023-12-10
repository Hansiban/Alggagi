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
        _signUpModal.SetActive(false);

        _nickNameInvalidationText.SetActive(false);
        _idInvalidationText.SetActive(false);
    }

    // 회원가입 버튼
    public void Btn_OpenSignUpModal()
    {
        // 모든 인풋필드를 빈 값으로 초기화
        _nicknameInputField.text = string.Empty;
        _idInputField.text = string.Empty;
        _pwdInputField.text = string.Empty;

        _signUpModal.SetActive(true);
    }

    // 회원가입 창 닫기 버튼
    public void Btn_CloseSignUpModal() => CloseSignUpModal();

    private void CloseSignUpModal() => _signUpModal.SetActive(false);

    // 회원가입 창의 제출 버튼
    public void Btn_Submit()
    {
        if (!CheckSignUpValidation())
            return;

        SignUp();
        CloseSignUpModal();
    }

    private bool CheckSignUpValidation()
    {
        bool isIdValid = CheckIfIdValid();
        bool isNickNameValid = CheckIfNicknameValid();

        return isIdValid && isNickNameValid;
    }

    // 아이디 중복 검사
    private bool CheckIfIdValid()
    {
        string cmdTxt = $"SELECT * FROM user WHERE id = \"{_idInputField.text}\"";

        // if does not exist, valid
        bool isValid = !DbAccessManager_KYS.Instance.Select(cmdTxt);

        Debug.LogWarning("Id " + isValid);

        _idInvalidationText.SetActive(!isValid);

        return isValid;
    }

    // 닉네임 중복 검사
    private bool CheckIfNicknameValid()
    {
        string cmdTxt = $"SELECT * FROM user WHERE nick = \"{_nicknameInputField.text}\"";

        // if does not exist, valid
        bool isValid = !DbAccessManager_KYS.Instance.Select(cmdTxt);

        Debug.LogWarning("Nick reverse " + DbAccessManager_KYS.Instance.Select(cmdTxt));
        Debug.LogWarning("Nick " + isValid);

        _nickNameInvalidationText.SetActive(!isValid);

        return isValid;
    }

    private void SignUp()
    {
        string cmdTxt = $"INSERT INTO user VALUES (\"{_idInputField.text}\", \"{_pwdInputField.text}\", \"{_nicknameInputField.text}\"," +
                                                    $"0, 0, 0, 0, 0);";

        var res = DbAccessManager_KYS.Instance.Insert(cmdTxt);

        if(res)
            _signUpModal.SetActive(false);
    }
}
