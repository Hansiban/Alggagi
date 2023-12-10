using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

class PlayerProfile : MonoBehaviour
{
    [SerializeField] TMP_Text _nickTxt;
    [SerializeField] TMP_Text _levelTxt;

    public bool IsInitialized { get; private set; } = false;

    public void Init(UserDataModel_KYS userData)
    {
        _nickTxt.text = userData.Nick;
        _levelTxt.text = userData.Lvl.ToString();

        IsInitialized = true;
    }
}
