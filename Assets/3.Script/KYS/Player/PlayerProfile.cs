using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

class PlayerProfile : MonoBehaviour
{
    [SerializeField] private TMP_Text _nickTxt;
    [SerializeField] private TMP_Text _lvlTxt;

    private string _nick;
    public string Nick 
    {
        get => _nick;
        private set
        {
            _nick = value;
            _nickTxt.text = _nick;
        }
    }

    private int _lvl;
    public int Lvl
    {
        get => _lvl;
        private set
        {
            _lvl = value;
            //_lvlTxt.text = _lvl.ToString();
        }
    }

    public bool IsInitialized { get; private set; } = false;

    public void Init(string nick, int lvl)
    {
        Nick = nick;
        Lvl = lvl;

        IsInitialized = true;
    }
}
