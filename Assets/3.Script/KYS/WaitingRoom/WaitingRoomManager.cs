using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomManager : MonoBehaviour
{
    public void Btn_TempGoToMainMenu(string sceneName) => SceneManager.LoadScene(sceneName);
}
