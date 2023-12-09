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
        if (isLocalPlayer) // �̰� �ù� ���� localPlayer �Ƴ൵ ������ �� ���� ���� ���� ��� ����. �׷��� ���� MainMenu_KYS���� �� ó�� ����. isowned �ƴϸ� disable �ǵ���.
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
        // �� ������ �� �ؾ��ϴ� �ǰ� Ȯ���غ���
        mainCanvas.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        Debug.Log("cl:" + conn);

        // --------------------------------------- MainMenu_KYS �ʿ��� isOwned�� Awake���� üũ�ϴ� ����� üũ�� �� �Ǿ�, MainMenuCanvas �ܿ��� MainMenuCanvas�� ��Ȱ��ȭ �� �� ����
                                                    // ��¿ �� ���� ���ʿ��� ã�Ƽ� ���� �� �ƴ� MainMenuCanvas�� ��Ȱ��ȭ �Ϸ��� ���� �˲�� �� ����
                                                    // �㳪, MainMenu_KYS�� Start���� isOwned�� üũ�ϴ� �� üũ�Ǿ�, �Ʒ��� �޼ҵ���� ������� �ʰ� �Ǿ���
        //RpcDisableWhenNotOwner();
        //TargetEnableMyCanvas(mainCanvas);
    }

    // only works on ��������
    [TargetRpc]
    private void TargetEnableMyCanvas(GameObject go)
    {
        Debug.Log("TargetEnableMyCanvas");
        go.SetActive(true);
    }



    [ClientRpc(includeOwner = false)]
    private void RpcDisableWhenNotOwner()
    {
        // �̰� ���� ��
        Debug.Log("IS NOT OWNER");

        CmdTurnOffWhatNotMyCanvas();
    }

    [Command(requiresAuthority =false)]
    private void CmdTurnOffWhatNotMyCanvas()
    {
        Debug.Log("CmdTurnOffWhatNotMyCanvas");
        TargetTurnOffWhatNotMyCanvas();
    }

    // �̷��� ���� �ְ� ���� ������ ���� �ʰڳ�?
    //[TargetRpc]
    [ClientRpc]
    private void TargetTurnOffWhatNotMyCanvas()
    {
        Debug.Log("TargetTurnOffWhatNotMyCanvas");
        GameObject[] sad = GameObject.FindGameObjectsWithTag("Test");

        // �� �žƴ� MainMenuCanvas ��
        for (int i = 0; i < sad.Length; i++)
        {
            if (sad[i].TryGetComponent(out NetworkIdentity identity))
            {
                Debug.Log("���̵�ƼƼ ã��");
                CmdTurnOffWhatNotMyCanvas2222(identity);

                //Debug.Log("�� �žƴϸ�" + identity.connectionToClient + ":" + connectionToClient);
                //if(identity.connectionToClient != connectionToClient)
                //{
                //    Debug.Log("��");
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
            Debug.Log("��");
            TargetTurnOffWhatNotMyCanvas2222(identity.gameObject);
            identity.gameObject.SetActive(false);
        }
    }

    // only works on �Ĺ�����
    //[TargetRpc]
    // �̷��� ���� �ְ� ���� ������ ���� �ʰڳ�?
    [ClientRpc]
    private void TargetTurnOffWhatNotMyCanvas2222(GameObject go)
    {
        Debug.Log("TargetTurnOffWhatNotMyCanvas2222");
        go.SetActive(false);
    }
}
