using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// for Login
public class TestDummyPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject _mainCanvas;

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

    }

    private void Start()
    {
        if (isLocalPlayer) // 이게 시발 지가 localPlayer 아녀도 들어오고 쳐 지랄 난리 생쑈 까고 있음. 그래서 내가 MainMenu_KYS에서 똥 처리 해줌. isowned 아니면 disable 되도록.
        {
            Debug.Log("I am local player");
            CmdSpawnMainMenuCanvas(connectionToClient);
        }
        else
        {
            Debug.Log("NOT!!!! I am NOT!!! local player");
        }
    }


    [Command]
    private void CmdSpawnMainMenuCanvas(NetworkConnectionToClient conn)
    {
        Debug.Log("local player" + connectionToClient.connectionId);
        GameObject mainCanvas = Instantiate(_mainCanvas);

        //mainCanvas.SetActive(false);

        //NetworkServer.Spawn(mainCanvas, conn);
        NetworkServer.Spawn(mainCanvas, connectionToClient);
        // 이 행위도 꼭 해야하는 건가 확인해보기
        mainCanvas.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        Debug.Log("cl:" + conn);

        // --------------------------------------- MainMenu_KYS 쪽에서 isOwned를 Awake에서 체크하니 제대로 체크가 안 되어, MainMenuCanvas 단에서 MainMenuCanvas를 비활성화 할 수 없어
                                                    // 어쩔 수 없이 이쪽에서 찾아서 본인 것 아닌 MainMenuCanvas만 비활성화 하려고 지랄 똥꼬쇼를 한 흔적
                                                    // 허나, MainMenu_KYS의 Start에서 isOwned를 체크하니 잘 체크되어, 아래의 메소드들은 사용하지 않게 되었음
        //RpcDisableWhenNotOwner();
        //TargetEnableMyCanvas(mainCanvas);
    }

    // only works on 선발주자
    [TargetRpc]
    private void TargetEnableMyCanvas(GameObject go)
    {
        Debug.Log("TargetEnableMyCanvas");
        go.SetActive(true);
    }



    [ClientRpc(includeOwner = false)]
    private void RpcDisableWhenNotOwner()
    {
        // 이거 돌긴 돎
        Debug.Log("IS NOT OWNER");

        CmdTurnOffWhatNotMyCanvas();
    }

    [Command(requiresAuthority =false)]
    private void CmdTurnOffWhatNotMyCanvas()
    {
        Debug.Log("CmdTurnOffWhatNotMyCanvas");
        TargetTurnOffWhatNotMyCanvas();
    }

    // 이러면 새로 애가 들어올 때마다 돌지 않겠냐?
    //[TargetRpc]
    [ClientRpc]
    private void TargetTurnOffWhatNotMyCanvas()
    {
        Debug.Log("TargetTurnOffWhatNotMyCanvas");
        GameObject[] sad = GameObject.FindGameObjectsWithTag("Test");

        // 내 거아닌 MainMenuCanvas 끔
        for (int i = 0; i < sad.Length; i++)
        {
            if (sad[i].TryGetComponent(out NetworkIdentity identity))
            {
                Debug.Log("아이덴티티 찾음");
                CmdTurnOffWhatNotMyCanvas2222(identity);

                //Debug.Log("내 거아니면" + identity.connectionToClient + ":" + connectionToClient);
                //if(identity.connectionToClient != connectionToClient)
                //{
                //    Debug.Log("끔");
                //    sad[i].SetActive(false);
                //}
            }
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdTurnOffWhatNotMyCanvas2222(NetworkIdentity identity)
    {
        Debug.Log("CmdTurnOffWhatNotMyCanvas2222");
        Debug.Log("2222" + connectionToClient + ":" + connectionToClient);
        if (identity.connectionToClient != connectionToClient)
        {
            Debug.Log("끔");
            TargetTurnOffWhatNotMyCanvas2222(identity.gameObject);
            identity.gameObject.SetActive(false);
        }
    }

    // only works on 후발주자
    //[TargetRpc]
    // 이러면 새로 애가 들어올 때마다 돌지 않겠냐?
    [ClientRpc]
    private void TargetTurnOffWhatNotMyCanvas2222(GameObject go)
    {
        Debug.Log("TargetTurnOffWhatNotMyCanvas2222");
        go.SetActive(false);
    }
}
