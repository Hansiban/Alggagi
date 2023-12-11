using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomManager : MonoBehaviour
{
    public void Btn_TempGoToMainMenu(string sceneName) => SceneManager.LoadScene(sceneName);

    private MyNetworkRoomManager manager;

    private void Start()
    {
        manager = FindObjectOfType<MyNetworkRoomManager>();

        Debug.Log(SceneManager.GetActiveScene().name + "이 클라 쪽에서 로드됐습니다");

        Debug.Log("networkAddress before" + manager.networkAddress);
        manager.networkAddress = "172.30.1.32";
        Debug.Log("networkAddress after" + manager.networkAddress);
        //MyNetworkRoomManager.singleton.transport = new KcpTransport() { port = 7777 };
        manager.StartClient();
    }
}
