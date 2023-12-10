using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Network Room Manager를 사용할 때에 Player Prefab에 부여될 스크립트
public class MyNetworkPlayer : NetworkBehaviour
{
    void Start()
    {
        if (!isLocalPlayer) return;

        Debug.Log("나의 id는" + GameManager.Instance.LocalUserData.Id);

        CmdGetOpponentData(GameManager.Instance.LocalUserData.Id);
    }

    [Command]
    private void CmdGetOpponentData(string id)
    {
        Debug.Log("받은 id는" + id);

        Debug.Log("My Network Player Start. My Network Room Manager의 User Data 수는 " + MyNetworkRoomManager.singleton.UserDatum.Count);

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

        // 정보 채우기
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
