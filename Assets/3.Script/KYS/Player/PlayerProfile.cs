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

    public void Init(string nick, int lvl)
    {
        _nickTxt.text = nick;
        _levelTxt.text = lvl.ToString();

        IsInitialized = true;
    }
}
