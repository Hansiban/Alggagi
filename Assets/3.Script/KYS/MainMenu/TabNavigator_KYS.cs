using System;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

class TabNavigator_KYS : MonoBehaviour
{
    [SerializeField] private GameObject _signUpModal;

    [SerializeField] private Selectable[] _loginSelectables;
    [SerializeField] private Selectable[] _signUpSelectables;

    private Selectable[] _selectablesToHandle
        => _signUpModal.activeSelf ? _signUpSelectables : _loginSelectables;
    
    
}