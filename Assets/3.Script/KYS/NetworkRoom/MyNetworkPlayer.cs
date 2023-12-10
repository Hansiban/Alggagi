using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Network Room Manager�� ����� ���� Player Prefab�� �ο��� ��ũ��Ʈ
public class MyNetworkPlayer : NetworkBehaviour
{
    void Start()
    {
        if (!isLocalPlayer) return;

        Debug.Log("���� id��" + GameManager.Instance.LocalUserData.Id);

        CmdGetOpponentData(GameManager.Instance.LocalUserData.Id);
    }

    [Command]
    private void CmdGetOpponentData(string id)
    {
        Debug.Log("���� id��" + id);

        Debug.Log("My Network Player Start. My Network Room Manager�� User Data ���� " + MyNetworkRoomManager.singleton.UserDatum.Count);

        var myData = MyNetworkRoomManager.singleton.UserDatum[id];
        var oppData = MyNetworkRoomManager.singleton.UserDatum.Where(x => x.Key != id).FirstOrDefault().Value;

        Debug.Log($"myData == null {myData == null}\noppData == null {oppData == null}");

        if (myData != null)
            Debug.Log(myData.ToString());
        else if (oppData != null)
            Debug.Log(oppData.ToString());

        TargetGetOpponentData(oppData.Id, oppData.Pwd, oppData.Nick, oppData.Lvl, oppData.Exp, oppData.Win, oppData.Lose, oppData.Draw);
    }

    [TargetRpc]
    private void TargetGetOpponentData(string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw)
    {
        var oppData = UserDataModel_KYS.GetCopy(id, pwd, nick, lvl, exp, win, lose, draw);

        Debug.Log("oppoData : " + oppData.ToString());
        Debug.Log("myData : " + GameManager.Instance.LocalUserData.ToString());

        // ���� ä���
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
