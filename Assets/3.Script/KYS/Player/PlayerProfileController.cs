using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerProfileController : MonoBehaviour
{
    // profile picture
    [SerializeField] TMP_Text _nickTxt;
    [SerializeField] TMP_Text _levelTxt;

    private UserDataModel_KYS _userData;


    public void Init(UserDataModel_KYS userData)
    {
        _userData = userData;
    }
}
